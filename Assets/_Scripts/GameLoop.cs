using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public int minIngredients, maxIngredients;
    public int maxSingleIngredient;
    public GameObject[] ingredients;
    public int[] numIngredient;
    public Sprite empty, full;
    public ChopMaster chopMaster;
    public HandManager hand;
    public string vegetableCut;
    public GameObject audioManager;

    ReviewGenerator reviewGenerator;
    List<string> recipe;
    List<string> choppedIngredients;
    int numWrongIngredients;
    int fingersLostThisRound;
    bool hasFingers;

    int total, lives;
    float timer;
    public float delay;
    float totalTime;
    float timeVar1, timeVar2;
    float volumeTemp;

    bool flagFire;
    bool flagSmoke;
    bool flagBubbles;
    bool flagLiveLost;
    bool flagLostAllFingers;

    GameObject player, bar, sign, meter, fire, smoke, bubbles;
    GameObject starSet, starLeft, starMiddle, starRight, review, title, inspection;
    GameObject scoreObj;
    Text scoreTxt;

    float barVar;
    public bool cutSuccessfully;
    int completedRecipes;
    float totalIngredientsChopped;
    float wrongIngredientsChopped;
    int pressCheck;

    bool displayingScore;
    public bool gameStarted = false;

    System.Random rand = new System.Random();
    
    // Use this for initialization
    void Start()
    {
        scoreObj = GameObject.FindGameObjectWithTag("Score");
        scoreTxt = scoreObj.GetComponent<Text>();
        flagFire = true;
        flagSmoke = true;
        flagBubbles = true;
        flagLiveLost = true;
        flagLostAllFingers = true;
        displayingScore = false;
        recipe = new List<string>();
        choppedIngredients = new List<string>();
        reviewGenerator = GetComponent<ReviewGenerator>();
        hasFingers = true;
        cutSuccessfully = false;
        numIngredient = new int[ingredients.Length];
        timeVar1 = 8.0f;
        timeVar2 = 10.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        bar = GameObject.FindGameObjectWithTag("bar");
        sign = GameObject.FindGameObjectWithTag("sign");
        meter = GameObject.FindGameObjectWithTag("meter");
        fire = GameObject.FindGameObjectWithTag("fire");
        smoke = GameObject.FindGameObjectWithTag("smoke");
        bubbles = GameObject.FindGameObjectWithTag("bubbles");
        starSet = GameObject.FindGameObjectWithTag("starSet");
        starLeft = GameObject.FindGameObjectWithTag("star1");
        starMiddle = GameObject.FindGameObjectWithTag("star2");
        starRight = GameObject.FindGameObjectWithTag("star3");
        review = GameObject.FindGameObjectWithTag("Review");
        inspection = GameObject.FindGameObjectWithTag("Inspection");
        audioManager = GameObject.Find("AudioManager");
        barVar = .78f;
        delay = 15.0f;
        totalTime = 100.0f;
        numWrongIngredients = 0;
        fingersLostThisRound = 0;
        wrongIngredientsChopped = 0;
        totalIngredientsChopped = 0;
        completedRecipes = 0;
        hasFingers = true;
        lives = 3;
        completedRecipes = 0;
        total = 0;
        starSet.transform.localScale = new Vector3(.45f, 1f, .45f);
        starLeft.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
        starMiddle.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
        starRight.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
        initializeRecipe();
        pressCheck = 0;
        hand.Reset();
        review.transform.localScale = new Vector3(0f, 1.5f, 20.7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            timer += Time.deltaTime;
            delay += Time.deltaTime;
            volumeTemp += timer / (totalTime * totalTime * totalTime);
            scoreTxt.text = "Score: " + ((completedRecipes+1) * (totalIngredientsChopped - wrongIngredientsChopped * 2) * 1000 - ((hand.transform.childCount - hand.GetFingerCount()) * completedRecipes * totalIngredientsChopped));
        }
        else
        {
            scoreTxt.text = "";
            review.transform.GetChild(1).gameObject.SetActive(false);
            review.transform.localScale = new Vector3(1.5f, 1.5f, 20.7f);
            if (Input.anyKeyDown)
            {
                audioManager.GetComponent<AudioManager>().potboilingEvent.setVolume(volumeTemp);
                audioManager.GetComponent<AudioManager>().ambientEvent.start();
                audioManager.GetComponent<AudioManager>().potboilingEvent.start();
                audioManager.GetComponent<AudioManager>().PlayBackgroundMusic();
                review.transform.GetChild(1).gameObject.SetActive(true);
                review.transform.GetChild(0).gameObject.SetActive(false);
                review.transform.localScale = new Vector3(0f, 1.5f, 20.7f);
                gameStarted = true;
                audioManager.GetComponent<AudioManager>().PlayMenuAdvance();
            }
        }

        //No more fingers
        if (flagLostAllFingers && !hasFingers && !hand.isBleeding()) {
            scoreTxt.text = "";
            lives = 0;
            delay = 0;
            flagLostAllFingers = false;
            hand.GoToGrabPos();
            audioManager.GetComponent<AudioManager>().potboilingEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            string reviewString = reviewGenerator.Generate(recipe, GetWrongIngredients(choppedIngredients), fingersLostThisRound, timer > totalTime, hasFingers);
            review.transform.localScale = new Vector3(1.5f, 1.5f, 20.7f);
            inspection.GetComponent<Text>().text = reviewString;
            chopMaster.Reset();
        }

        //Other loss condition
        else if (flagLiveLost && total <= 0 && (numWrongIngredients != 0 || fingersLostThisRound != 0 || timer > totalTime )) {
            scoreTxt.text = "";
            lives--;
            delay = 0;
            flagLiveLost = false;
            if (timer > totalTime) {
                timeVar1 = 8.0f;
                timeVar2 = 10.0f;
            }
            audioManager.GetComponent<AudioManager>().potboilingEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            string reviewString = reviewGenerator.Generate(recipe, GetWrongIngredients(choppedIngredients), fingersLostThisRound, timer > totalTime, hasFingers);
            review.transform.localScale = new Vector3(1.5f, 1.5f, 20.7f);
            inspection.GetComponent<Text>().text = reviewString;
            chopMaster.Reset();
        }

        //Round over
        else if (delay < 3.0f) {
            hand.GoToGrabPos();
            if (!hasFingers)
            {
                if (delay % 1 < .5) { starSet.transform.localScale = new Vector3(.45f, 1f, .45f); }
                else { starSet.transform.localScale = new Vector3(0f, 1f, .45f); }
            }
            update_lives(lives);
        }

        //Score is displaying on game over
        else if (displayingScore)
        {
            delay = 4;
            float score = completedRecipes * (totalIngredientsChopped - wrongIngredientsChopped * 2 )* 1000 - ((hand.transform.childCount - hand.GetFingerCount()) * completedRecipes * totalIngredientsChopped);
            if (score < 0) score = 0;
            if (totalIngredientsChopped == 0) totalIngredientsChopped = 1;
            inspection.GetComponent<Text>().text = "Completed Recipes: " + completedRecipes + "\nFingers Remaining: " + hand.GetFingerCount() +
                "\nTotal Ingredients Chopped: " + totalIngredientsChopped + "\nIngredient accuracy: "+ (int)((1-wrongIngredientsChopped/totalIngredientsChopped) * 100) + "%" +
                "\nScore: " + (int)score +  "\n\nPress any key to try again!";
            if (Input.anyKeyDown){
                audioManager.GetComponent<AudioManager>().PlayMenuAdvance();
                Start();
            }
        }

        //Review Over/Game Over
        else if (delay >= 3.0f && delay < 15.0f)
        {
            delay = 4.0f;

            if (Input.anyKeyDown && lives > 0) {
                audioManager.GetComponent<AudioManager>().PlayMenuAdvance();
                review.transform.localScale = new Vector3(0f, 1.5f, 20.7f);
                hand.GoToGrabPos();
                delay = 15.0f;
            }
            else if (Input.anyKeyDown && lives <= 0) {
                displayingScore = true;
                audioManager.GetComponent<AudioManager>().PlayMenuAdvance();
            }
        }

        //everytime player completes a recipe, a new one generates with new timer constraints;
        else if (total <= 0 && delay >= 15.0f && lives > 0)
        {
            if(hasFingers)
                completedRecipes++;
            initializeRecipe();
        }

        if (cutSuccessfully){
            totalIngredientsChopped++;
            total = 0;
            choppedIngredients.Add(vegetableCut);
            for (int i=0; i<ingredients.Length; i++)
            {
                if (vegetableCut == ingredients[i].tag)
                    if (numIngredient[i] > 0)
                        numIngredient[i]--;
                total += numIngredient[i];
            }
           
            cutSuccessfully = false;
        }

        if (timer > totalTime && lives > 0 && delay >= 15.0f)
        {
            total = 0;
            sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);
            meter.GetComponent<SpriteRenderer>().sprite = full;
            bar.transform.localScale = new Vector3(0f, .7f, 1f);
            if (flagFire)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Pause();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Pause();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Play();
                flagFire = false;
            }
        }

        else if (timer / totalTime <= .5f && lives > 0 && delay >= 15.0f)
        {
            sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f);
            bar.transform.localScale = new Vector3(timer * barVar / totalTime, .7f, 1f);
            bar.GetComponent<SpriteRenderer>().color = new Vector4(1f - (timer * 2 / totalTime), 1f, 0f, 1f);
            if (flagSmoke)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Play();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Pause();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Pause();
                flagSmoke = false;
            }
        }

        else if (timer / totalTime >= .95f && lives > 0 && delay >= 15.0f)
        {
            if (timer % .25 < .125) { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            bar.transform.localScale = new Vector3(timer * barVar / totalTime, .7f, 1f);
            bar.GetComponent<SpriteRenderer>().color = new Vector4(-1f + (timer * 2 / totalTime), 2f - (timer * 2 / totalTime), 0f, 1f);
            if (flagBubbles)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Pause();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Play();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Pause();
                flagBubbles = false;
            }
        }

        else if (timer / totalTime >= .85f && lives > 0 && delay >= 15.0f)
        {
            if (timer % .5 < .25) { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            bar.transform.localScale = new Vector3(timer * barVar / totalTime, .7f, 1f);
            bar.GetComponent<SpriteRenderer>().color = new Vector4(-1f + (timer * 2 / totalTime), 2f - (timer * 2 / totalTime), 0f, 1f);
            if (flagBubbles)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Pause();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Play();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Pause();
                flagBubbles = false;
            }
        }

        else if (timer / totalTime >= .75f && lives > 0 && delay >= 15.0f)
        {
            if (timer % 1 < .5) { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            bar.transform.localScale = new Vector3(timer * barVar / totalTime, .7f, 1f);
            bar.GetComponent<SpriteRenderer>().color = new Vector4(-1f + (timer * 2 / totalTime), 2f - (timer * 2 / totalTime), 0f, 1f);
            if (flagBubbles)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Pause();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Play();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Pause();
                flagBubbles = false;
            }
        }

        else if (lives > 0 && delay >= 15.0f)
        {
            sign.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f);
            bar.transform.localScale = new Vector3(timer * barVar / totalTime, .7f, 1f);
            bar.GetComponent<SpriteRenderer>().color = new Vector4(-1f + (timer * 2 / totalTime), 2f - (timer * 2 / totalTime), 0f, 1f);
            if (flagBubbles)
            {
                smoke.GetComponent<ParticleSystem>().Clear();
                smoke.GetComponent<ParticleSystem>().Pause();
                bubbles.GetComponent<ParticleSystem>().Clear();
                bubbles.GetComponent<ParticleSystem>().Play();
                fire.GetComponent<ParticleSystem>().Clear();
                fire.GetComponent<ParticleSystem>().Pause();
                flagBubbles = false;
            }
        }
    }

    void initializeRecipe()
    {
        audioManager.GetComponent<AudioManager>().potboilingEvent.setTimelinePosition(0);
        audioManager.GetComponent<AudioManager>().potboilingEvent.start();
        volumeTemp = 0;

        flagLiveLost = true;
        recipe.Clear();
        choppedIngredients.Clear();
        finish_recipe();
        create_recipe();
        SetRecipe();
        total = 0;
        foreach (int i in numIngredient) total += i;
        timeVar1 = timeVar1 * .85f;
        timeVar2 = timeVar2 * .85f;
        timer = 0;
        totalTime = (total - total * .25f) * timeVar1 + (timeVar2 % 10);
        Debug.Log("total: "+total + "  totalTime: " + totalTime);
        meter.GetComponent<SpriteRenderer>().sprite = empty;
        bar.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 0f, 1f);
        numWrongIngredients = 0;
        fingersLostThisRound = 0;

        flagFire = true;
        flagSmoke = true;
        flagBubbles = true;
    }

    void create_recipe()
    {
        int numIngred = rand.Next(minIngredients, maxIngredients);

        for (int i = 0; i < numIngred; i++)
        {
            int index = rand.Next(numIngredient.Length);

            while (numIngredient[index] >= maxSingleIngredient)
                index = rand.Next(numIngredient.Length);
            numIngredient[index]++;
        }
    }

    void finish_recipe()
    {
        for (int i = 0; i < numIngredient.Length - 1; i++) { numIngredient[i] = 0; }
    }

    void update_lives(int currLives)
    {
        if (currLives <= 0)
        {
            if (delay > 3) { starLeft.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            else if (delay % 1 < .5) { starLeft.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { starLeft.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
        }
        else if (currLives <= 1)
        {
            if (delay > 3) { starMiddle.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            else if (delay % 1 < .5) { starMiddle.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { starMiddle.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
        }
        else if (currLives <= 2)
        {
            if (delay > 3) { starRight.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
            else if (delay % 1 < .5) { starRight.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f); }
            else { starRight.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 0f); }
        }
    }

    public void incNumWrongIngredients()
    {
        numWrongIngredients++;
    }

    public void incFingersLostThisRound()
    {
        fingersLostThisRound++;
    }

    public void setNoFingers()
    {
        hasFingers = false;
    }

    void SetRecipe()
    {
        for (int i = 0; i < numIngredient.Length; i++)
            if (numIngredient[i] > 0)
                recipe.Add(ingredients[i].tag);
    }

    public List<string> GetRecipe()
    {
        return recipe;
    }

    List<string> GetWrongIngredients(List<string> choppedIngredients)
    {
        List<string> wrongIngredients = new List<string>();
        foreach (string s in choppedIngredients)
        {
            if (!recipe.Contains(s))
            {
                wrongIngredients.Add(s);
            }
        }
        return wrongIngredients;
    }

    public void incWrongIngredientsChopped()
    {
        wrongIngredientsChopped++;
    }
}
