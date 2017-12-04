using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Purchase : MonoBehaviour {

    int inv;
    public int arrowID;
    GameObject var;
    DisplayPurchase vars;

    // Use this for initialization
    void Start () {
        var = GameObject.FindGameObjectWithTag("Player");
        vars = var.GetComponent<DisplayPurchase>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseDown()
    {
        if (gameObject.tag == "Arrow") {

            switch (arrowID%2)
            {
                case 0:
                    inv = -1;
                    break;
                case 1:
                    inv = 1;
                    break;
            }

            switch (arrowID / 2)
            {
                case 0:
                    vars.inv1 = vars.inv1 + inv;
                    break;
                case 1:
                    vars.inv2 = vars.inv2 + inv;
                    break;
                case 2:
                    vars.inv3 = vars.inv3 + inv;
                    break;
                case 3:
                    vars.inv4 = vars.inv4 + inv;
                    break;
                case 4:
                    vars.inv5 = vars.inv5 + inv;
                    break;
                case 5:
                    vars.inv6 = vars.inv6 + inv;
                    break;
            }
        }
    }
}