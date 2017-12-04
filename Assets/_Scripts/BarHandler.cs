using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarHandler : MonoBehaviour {

    public BeatMarker[] markers;
    public int count = 0;
    // Use this for initialization
    void Start () {
        
        
    }

    public void getMarkers()
    {
        markers = GetComponentsInChildren<BeatMarker>();
    }

	// Update is called once per frame
	void Update () {
        
	}
}
