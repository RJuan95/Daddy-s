using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMaster : MonoBehaviour {
    
    public float screenDistance;
    public float bpm;
    public float allowedError;
    public GameObject beatMarker;
    public string[] sheetMusic;
    List<GameObject> beatMarkers;
    Vector3 destination;
    Transform cursor;
    int currentMeasure;
    float timeToReachEnd;
    float t;

    GameObject audioManager;

    // Use this for initialization
	void Start () {
        beatMarkers = new List<GameObject>();
        cursor = transform.GetChild(0);
        destination = new Vector3(screenDistance, cursor.localPosition.y, 0);
        timeToReachEnd = 60 / (bpm / 4);
        SetBeatMarkers(sheetMusic[currentMeasure++]);
        audioManager = GameObject.Find("AudioManager");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            if (CalculateError() > allowedError)
            {
                audioManager.GetComponent<AudioManager>().PlayFailure();

            }
            else
            {
                audioManager.GetComponent<AudioManager>().PlaySuccess();
            }
        }
        MoveCursor();
	}

    void MoveCursor()
    {
        t += Time.deltaTime / timeToReachEnd;
        cursor.localPosition = Vector3.Lerp(Vector3.zero, destination, t);
        if (cursor.localPosition.x == destination.x)
        {
            if (currentMeasure < sheetMusic.Length)
                SetBeatMarkers(sheetMusic[currentMeasure++]);
            cursor.localPosition = Vector3.zero;
            t = 0;
        }
    }

    void SetBeatMarkers(string measure)
    {
        clearMarkers();
        float markerPosX;
        for (int i = 0; i < measure.Length; i++)
        {
            if (measure[i] == 'x')
            {
                beatMarkers.Add(Instantiate(beatMarker, transform.position, Quaternion.identity) as GameObject);
                beatMarkers[beatMarkers.Count - 1].transform.parent = transform;
                markerPosX = screenDistance / measure.Length * i + screenDistance / (measure.Length * 2);
                beatMarkers[beatMarkers.Count - 1].transform.localPosition = new Vector3(markerPosX, 0, 0);
            }
        }
    }

    public float CalculateError()
    {
        float err;
        float dist = Vector3.Distance(cursor.transform.localPosition, beatMarkers[0].transform.localPosition);
        err = dist;
        for (int i = 1; i < beatMarkers.Count; i++)
        {
            dist = Vector3.Distance(cursor.transform.localPosition, beatMarkers[i].transform.localPosition);
            err = dist < err ? dist : err;
        }
        return err;
    }

    void clearMarkers()
    {
        Transform child;
        for (int i = beatMarkers.Count - 1 ; i >= 0; i--)
        {
            beatMarkers.Remove(beatMarkers[i]);
        }
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            child = transform.GetChild(i);
            if (child.tag == "BeatMarker")
                Destroy(child.gameObject);
        }
    }

    GameObject GetClosestMarker()
    {
        GameObject marker = beatMarkers[0];
        float dist = Vector3.Distance(cursor.transform.localPosition, beatMarkers[0].transform.localPosition);
        for (int i = 1; i < beatMarkers.Count; i++)
        {
            float nextDist = Vector3.Distance(cursor.transform.localPosition, beatMarkers[i].transform.localPosition);
            if (nextDist < dist)
            {
                marker = beatMarkers[i];
                dist = nextDist < dist ? nextDist : dist;
            }
        }
        return marker;
    }
}
