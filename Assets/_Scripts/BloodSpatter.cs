using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpatter : MonoBehaviour {

    float timePassed;
    float dieTime;
    ParticleSystem particle;
	// Use this for initialization
	void Start () {
        particle = transform.GetChild(0).GetComponent<ParticleSystem>();
        dieTime = particle.main.duration + 1;
	}
	
	// Update is called once per frame
	void Update () {
        timePassed += Time.deltaTime;
        if (timePassed > dieTime)
            Destroy(gameObject);
	}
}
