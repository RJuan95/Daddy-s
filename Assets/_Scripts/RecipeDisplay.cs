using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeDisplay : MonoBehaviour {
    public GameObject numSprite;
    public Sprite[] Snum;
    GameLoop inventory;
    Transform RecipeTransform;

    // Use this for initialization
    void Start () {
        inventory = GetComponent<GameLoop>();
        RecipeTransform = GameObject.Find("Recipe").transform;
        instantiateIngredients(4, 2);

        for (int i = 1; i < RecipeTransform.childCount; i++)
        {
            hide(RecipeTransform.GetChild(i).gameObject);
        }
     
    }

	// Update is called once per frame
	void Update () {
        show();
    }

    void hide(GameObject veg) {
        for (int i = 1; i < RecipeTransform.childCount; i++)
            if (veg.tag == RecipeTransform.GetChild(i).tag)
                RecipeTransform.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
    }

    void show ()
    {
        for (int i = 0; i < inventory.numIngredient.Length; i++)
        {
            if (inventory.numIngredient[i] > 0)
            {
                //+1 because of recipeBG object
                RecipeTransform.GetChild(i + 1).GetComponent<SpriteRenderer>().enabled = true;
                RecipeTransform.GetChild(i + 1).GetChild(0).GetComponent<SpriteRenderer>().sprite = Snum[inventory.numIngredient[i]];
            }
            else
            {
                RecipeTransform.GetChild(i + 1).GetChild(0).GetComponent<SpriteRenderer>().sprite = null;
                RecipeTransform.GetChild(i + 1).GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void instantiateIngredients(int rows, int columns)
    {
        float offset_x = 2.75f;
        float offset_y = -1f;
        int iterator = 0;
        Vector3 localPos = Vector3.zero;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (iterator < inventory.ingredients.Length)
                {
                    localPos = new Vector3(offset_x * j, offset_y * i, -.1f);
                    GameObject ingredient = Instantiate(inventory.ingredients[iterator++], Vector3.zero, Quaternion.identity);
                    ingredient.transform.parent = RecipeTransform;
                    ingredient.transform.localPosition = localPos;
                    ingredient.transform.localScale = RecipeTransform.localScale;
                    ingredient.transform.localScale = ingredient.transform.localScale * .6f;
                    GameObject numObject = Instantiate(numSprite, transform.position, Quaternion.identity);
                    numObject.transform.parent = ingredient.transform;
                    numObject.transform.localPosition = new Vector3(offset_x / 2, 0, -.1f);
                    numObject.transform.localScale = RecipeTransform.localScale;
                }
            }
        }
    }
}
