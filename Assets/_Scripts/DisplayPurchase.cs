using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPurchase : MonoBehaviour {

    public int inv1, inv2, inv3, inv4, inv5, inv6, inv7, inv8;
    List<int> Ingred = new List<int>();
    public List<int> recipe;

	// Use this for initialization
	void Start () {
        Ingred.Add(1);
        Ingred.Add(2);
        Ingred.Add(3);
        Ingred.Add(4);
        Ingred.Add(5);
        Ingred.Add(6);
        Ingred.Add(7);
        Ingred.Add(8);
        //create_recipe();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void create_recipe ()
    {
        System.Random rand = new System.Random();
        int numIngred = rand.Next(4, 12);

        for (int i = 0; i < numIngred; i++)
        {
            int index = rand.Next(8);
            recipe.Add(Ingred[index]);

            switch (Ingred[index])
            {
                case 1:
                    inv1++;
                    break;
                case 2:
                    inv2++;
                    break;
                case 3:
                    inv3++;
                    break;
                case 4:
                    inv4++;
                    break;
                case 5:
                    inv5++;
                    break;
                case 6:
                    inv6++;
                    break;
                case 7:
                    inv7++;
                    break;
                case 8:
                    inv8++;
                    break;
            }
        }
    }
}
