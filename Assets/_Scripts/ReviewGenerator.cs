using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewGenerator : MonoBehaviour {

    string s;
    string[] dishes;
    string[] whyIHate;
    string[] fingerLine;
    string[] finalSay;
    string[] burnedLine;
    // Use this for initialization

    void Start () {
        dishes = new string[] { "stew", "soup", "chili", "ministrone" };
        whyIHate = new string[] { "I am deathly allergic to <INGREDIENT> ",
            "I HATE <INGREDIENT>!!! ",
            "<INGREDIENT> gives me horrible diarrhea. ",
            "I'm actually <INGREDIENT> intolerant. "
        };
        fingerLine = new string[]
        {
            "Not to be a stickler, but I'm pretty sure there was a finger in my food. ",
            "I'm a little alarmed to have found a finger in my food. ",
            "Last time I checked, <DISH> isn't supposed to have fingers in it. "
        };
        burnedLine = new string[]
        {
            "It was burnt to a crisp! ",
            "It was cooking so long that the ingredients melted! "
        };
        finalSay = new string[]
        {
            "WORST. ANNIVERSARY. EVER!",
            "How could following a simple recipe be so difficult?!",
            "Even the plumber can't fix my toilet now.",
            "This experience was worse than when I forgot the safe word!"
        };
	}

    public string Generate(List<string> recipe, List<string> wrongIngredients, int fingersLost, bool burned, bool hasFingers)
    {
        //Debug.Log("RECIPE:");
        //foreach (string r in recipe)
        //    Debug.Log(r);
        //Debug.Log("\nWRONG INGREDIENTS:");
        //foreach (string r in wrongIngredients)
        //    Debug.Log(r);
        //Debug.Log("FINGERS LOST: " + fingersLost);
        //Debug.Log("BURNED: " + burned);
        //Debug.Log("HAS FINGERS: " + hasFingers);

        string dish = dishes[Random.Range(0, dishes.Length)];
        string wrongIngredient = "";
        if (wrongIngredients.Count > 0)
             wrongIngredient = wrongIngredients[Random.Range(0, wrongIngredients.Count)];
        s = "";

        s += "I ate at Daddy's and ordered a ";
        if (recipe.Count > 3)
        {
            for (int i = 0; i < 3; i++)
            {
                s += recipe[i] + " ";
            }
        }
        else
            foreach (string r in recipe)
                s += r + " ";
        s+= dish + ", ";

        if (wrongIngredients.Count > 0)
        {
            s += "and I'm pretty sure there was some " + wrongIngredient + " in it. ";
            s += whyIHate[Random.Range(0, whyIHate.Length)].Replace("<INGREDIENT>", wrongIngredient);
        }

        if (burned)
        {
            s += burnedLine[Random.Range(0, burnedLine.Length)];
        }

        if (!hasFingers)
        {
            return s += "It's a shame they closed down after the new chef lost all his fingers, but it's worse that I had to be the one to eat them.";
        }

        if (fingersLost > 0)
        {
            s += fingerLine[Random.Range(0, fingerLine.Length)].Replace("<DISH>", dish);
        }

        s += finalSay[Random.Range(0, finalSay.Length)];
        Debug.Log(s);

        return s;
    }
}
