using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorManager : MonoBehaviour {

    public float spacing;
    public float bpm;
    public float allowedError;
    public float timeToReachEnd;
    public Vector3 chopArea;
    public GameObject[] ingredients;
    public List<GameObject> ingredientPool;
    public GameObject[] arrowMarkers;
    public ChopMaster chopMaster;
    public GameLoop gameLoop;
    GameObject audioManager;
    GameObject currentMarker;
    float tempTime;

    public bool movingVeggie;

    Transform cursor;
    Transform staff;
    Transform spawnPoint;
    Transform endPoint;
    List<GameObject> beatMarkers;
    List<float> timeTracker;
    int wrongIngredients;

    void Start () {
        movingVeggie = false;
        beatMarkers = new List<GameObject>();
        cursor = transform.GetChild(0);
        staff = transform.GetChild(1);
        spawnPoint = transform.GetChild(2);
        endPoint = transform.GetChild(3);
        audioManager = GameObject.Find("AudioManager");
        timeTracker = new List<float>();
        for(int i = 0; i < ingredients.Length; ++i)
        {
            for(int j = 0; j < 4; ++j)
            {
                ingredientPool.Add(Instantiate(ingredients[i], spawnPoint.position, Quaternion.identity) as GameObject);
            }
        }
    }

    void Update () {
        if (gameLoop.delay >= 15 && gameLoop.gameStarted)
        {
            tempTime += Time.deltaTime;

            if (tempTime >= spacing)
            {
                tempTime = 0;
                instantateIngredient();
            }

            MoveConveyor();
            if (Input.GetKeyDown(KeyCode.Space) && chopMaster.chosenIngredient == null)
            {
                float error = CalculateError();
                if (error > allowedError || error == -1)
                {
                    StopChopping();
                    //audioManager.GetComponent<AudioManager>().PlayFailure();
                }
                else
                {
                    StartChopping();
                    audioManager.GetComponent<AudioManager>().PlayGrab();
                }
            }

            if (movingVeggie)
            {
                currentMarker.transform.position = Vector3.Lerp(currentMarker.transform.position, chopArea, Time.deltaTime * 5);
                if (Vector3.Distance(currentMarker.transform.position, chopArea) < .1f)
                    movingVeggie = false;
            }
        }
    }

    void MoveConveyor()
    {
        for(int i = 0; i < timeTracker.Count; ++i)
        {
            timeTracker[i] += Time.deltaTime/timeToReachEnd;
            beatMarkers[i].transform.position = Vector3.Lerp(spawnPoint.position, endPoint.position, timeTracker[i]);
            if (beatMarkers[i].transform.position == endPoint.position)
            {
                beatMarkers[i].transform.position = spawnPoint.position;
                ingredientPool.Add(beatMarkers[i]);
                beatMarkers.Remove(beatMarkers[i]);
                timeTracker.Remove(timeTracker[i]);
            }
        }
    }

    float CalculateError()
    {
        GameObject currentMarker = beatMarkers[0];
        if (currentMarker == null)
        {
            return -1;
        }
        else
        {
            float err = Vector3.Distance(cursor.transform.position, currentMarker.transform.position);
            return err;
        }
    }

    void StartChopping()
    {
        float dist1 = Vector3.Distance(cursor.position, beatMarkers[0].transform.position);
        float dist2 = Vector3.Distance(cursor.position, beatMarkers[1].transform.position);
        currentMarker = dist1 < dist2 ? beatMarkers[0] : beatMarkers[1];
        int j = beatMarkers.IndexOf(currentMarker);
        beatMarkers.Remove(currentMarker);
        timeTracker.Remove(timeTracker[j]);
        movingVeggie = true;
        if (chopMaster.chosenIngredient == null)
        {
            chopMaster.chosenIngredient = currentMarker.gameObject;
            chopMaster.Activate();
        }
        else
        {
            chopMaster.Reset();
            chopMaster.chosenIngredient = currentMarker.gameObject;
            chopMaster.Activate();
        }
    }

    void StopChopping()
    {
        if (chopMaster.chosenIngredient != null)
            chopMaster.Reset();
    }


    void instantateIngredient()
    {
        int ATTEMPTS = 3;
        
        GameObject ingredient = ingredientPool[Random.Range(0, ingredientPool.Count)];
       
        for (int i = 0; i < ATTEMPTS; i++)
        {
            if (!GetNeededIngredients().Contains(ingredient.tag))
            {
                ingredient = ingredientPool[Random.Range(0, ingredientPool.Count)];
            }
        }

        beatMarkers.Add(ingredient);
        ingredientPool.Remove(ingredient);
        timeTracker.Add(new float());
    }

    public List<string> GetNeededIngredients()
    {
        List<string> l = new List<string>();
        for (int i = 0; i < gameLoop.ingredients.Length; i++)
        {
            if (gameLoop.numIngredient[i] > 0)
                l.Add(gameLoop.ingredients[i].tag);
        }

        return l;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawnPoint.position;
    }
}
