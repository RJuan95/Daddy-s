using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {

    ConveyorManager conveyor;
    ChopMaster chopMaster;
    Vector3 cursorPos;
	// Use this for initialization
	void Start () {
        conveyor = GameObject.FindGameObjectWithTag("conveyor").GetComponent<ConveyorManager>();
        chopMaster = conveyor.chopMaster;
        cursorPos = conveyor.transform.GetChild(0).position;
        GoToGrabPos();
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    public void GoToGrabPos()
    {
        transform.position = new Vector3(cursorPos.x - .5f, cursorPos.y, cursorPos.z - conveyor.transform.localScale.z);
    }

    public void GoToCutPos()
    {
        Vector3 ingredientPos = chopMaster.chosenIngredient.transform.position;
        transform.position = new Vector3(ingredientPos.x - 1.5f, ingredientPos.y, ingredientPos.z);
    }

    public int GetFingerCount()
    {
        int count = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<SpriteRenderer>().enabled)
                count++;
        }
        return count;
    }

    public void Reset()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public bool isBleeding()
    {
        if (chopMaster.transform.childCount > 0)
            return true;
        return false;
    }

}
