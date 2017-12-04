using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatMarker : MonoBehaviour {

    bool hit;

	void Start () {
        hit = false;
	}

    public bool isHit()
    {
        return hit;
    }

    public void Hit()
    {
        hit = true;
    }
}
