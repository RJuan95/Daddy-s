using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChopMaster : MonoBehaviour {

    public GameObject hand;
    public GameLoop gameLoop;
    public GameObject[] arrows;
    public int minChops, maxChops;
    public Vector3 arrowSequenceLoc;
    public GameObject chosenIngredient;
    public GameObject SliceAnimation;
    public GameObject blood;
    public float timeBetweenArrows;

    ConveyorManager conveyor;
    HandManager handManager;
    GameObject audioManager;
    int sequenceIterator;
    GameObject[] arrowSequence;
    int currSequenceIdx;
    int wrongIngredients;
    bool activated;
    bool madeArrowSequenceList;
    string currInput;
    float arrowTime;
    float arrowTimePassed;

    // Use this for initialization
    void Start() {
        audioManager = GameObject.Find("AudioManager");
        conveyor = GameObject.FindGameObjectWithTag("conveyor").GetComponent<ConveyorManager>();
        handManager = hand.GetComponent<HandManager>();
        activated = false;
        madeArrowSequenceList = false;
        currSequenceIdx = 0;
        sequenceIterator = 0;
        arrowTime = timeBetweenArrows;
    }

    // Update is called once per frame
    void Update() {
        if (gameLoop.delay >= 15)
        {
            if (activated)
            {
                GenerateArrowSequence();
                if (!gameLoop.GetRecipe().Contains(chosenIngredient.tag))
                    gameLoop.incNumWrongIngredients();
                //THIS IS FOR PUNISHING TOO MUCH OF ONE INGREDIENT
                //if (!conveyor.GetNeededIngredients().Contains(chosenIngredient.tag))
                //    gameLoop.incNumWrongIngredients();
            }
            else if (chosenIngredient != null)
            {
                if (handleInput())
                {
                    gameLoop.cutSuccessfully = true;
                    gameLoop.vegetableCut = chosenIngredient.tag;
                    if (!gameLoop.GetRecipe().Contains(chosenIngredient.tag))
                        gameLoop.incWrongIngredientsChopped();
                    Reset();
                }
            }

            if (conveyor.movingVeggie)
            {
                handManager.GoToCutPos();
            }
        }
    }

    void GenerateArrowSequence()
    {
        float arrowDist = 1;
        if (!madeArrowSequenceList)
        {
            arrowSequence = new GameObject[Random.Range(minChops, maxChops + 1)];
            madeArrowSequenceList = true;
        }
        arrowTimePassed += Time.deltaTime;
        if (arrowTimePassed > arrowTime)
        {
            int arrowIdx = Random.Range(0, 4);
            Vector3 loc = arrowSequenceLoc + new Vector3(arrowDist * sequenceIterator, arrowSequenceLoc.y, arrowSequenceLoc.z);
            arrowSequence[sequenceIterator] = Instantiate(arrows[arrowIdx], loc, Quaternion.identity) as GameObject;
            sequenceIterator++;
            arrowTime *= 1.15f;
            arrowTimePassed = 0;
            audioManager.GetComponent<AudioManager>().PlayArrowDrop();
        }
        if (sequenceIterator == arrowSequence.Length)
        {
            activated = false;
            arrowTime = timeBetweenArrows;
        }
    }

    bool handleInput()
    {
        if (handManager.GetFingerCount() <= 0)
            gameLoop.setNoFingers();
        if (currSequenceIdx < arrowSequence.Length && Input.anyKeyDown)
        {
            currInput = arrowSequence[currSequenceIdx].tag;
            if (correctInput(currInput)) 
            {
                GameObject slice = Instantiate(SliceAnimation, chosenIngredient.transform.position, Quaternion.identity) as GameObject;
                slice.GetComponent<SliceAnim>().sliceDir = currInput;
                Destroy(arrowSequence[currSequenceIdx]);
                if (currSequenceIdx < arrowSequence.Length)
                    currSequenceIdx++;
                audioManager.GetComponent<AudioManager>().PlaySuccess();

            }
            else if (!Input.GetKeyDown("space"))
            {
                if (handManager.GetFingerCount() >= 1)
                {
                    audioManager.GetComponent<AudioManager>().PlayFailure();
                    int fingerLost = Random.Range(0, hand.transform.childCount);
                    while (!hand.transform.GetChild(fingerLost).GetComponent<SpriteRenderer>().enabled)
                        fingerLost = Random.Range(0, hand.transform.childCount);
                    GameObject bloodSpatter = Instantiate(blood, hand.transform.GetChild(fingerLost).position, Quaternion.identity) as GameObject;
                    bloodSpatter.transform.parent = transform;
                    hand.transform.GetChild(fingerLost).GetComponent<SpriteRenderer>().enabled = false;
                    gameLoop.incFingersLostThisRound();
                }
                return false;
            }
            return false;
        }
        else if (currSequenceIdx < arrowSequence.Length)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool correctInput(string current)
    {
        if (currInput == "up")
        {
            return (Input.GetKeyDown("up") || Input.GetKeyDown("w"));
        }
        else if (currInput == "down")
        {
            return (Input.GetKeyDown("down") || Input.GetKeyDown("s"));
        }
        else if (currInput == "left")
        {
            return (Input.GetKeyDown("left") || Input.GetKeyDown("a"));
        }
        else if (currInput == "right")
        {
            return (Input.GetKeyDown("right") || Input.GetKeyDown("d"));
        }
        else
            return false;
    }

    public void Activate()
    {
        activated = true;
    }

    public void Reset()
    {
        currSequenceIdx = 0;
        sequenceIterator = 0;
        if (arrowSequence != null) 
            foreach (GameObject a in arrowSequence)
                Destroy(a);
        if (chosenIngredient != null)
        {
            chosenIngredient.transform.position = conveyor.GetSpawnPoint();
            conveyor.ingredientPool.Add(chosenIngredient);
        }
        chosenIngredient = null;
        madeArrowSequenceList = false;
        handManager.GoToGrabPos();
    }

    public int totalWrongIngredients()
    {
        return wrongIngredients;
    }
}
