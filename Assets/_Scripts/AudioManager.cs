using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    string test = "event:/Test";
    string success = "event:/Success";
    string failure = "event:/NoteFailure";
    string ambience = "event:/Ambient Track";
    string grab = "event:/GrabIngredient";
    string loss = "event:/Loss";
    string potBoiling = "event:/PotBoiling";
    string veggieScream = "event:/VeggieScream";
    string backgroundMusic = "event:/BackgroundMusic";
    string arrowDrop = "event:/ArrowDrop";
    string menuAdvance = "event:/MenuAdvance";
    [FMODUnity.EventRef]

    public FMOD.Studio.EventInstance ambientEvent;
    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance potboilingEvent;

    GameObject mainCamera;
    GameObject gameLoop;
    void Start () {
        ambientEvent = FMODUnity.RuntimeManager.CreateInstance(ambience);
        potboilingEvent = FMODUnity.RuntimeManager.CreateInstance(potBoiling);

        mainCamera = GameObject.Find("Main Camera");

       // PlayBackgroundMusic();
        ambientEvent.setVolume(0.5f);
        //ambientEvent.start();
        //potboilingEvent.start();
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void PlaySuccess()
    {
        FMODUnity.RuntimeManager.PlayOneShot(success, mainCamera.transform.position);
    }

    public void PlayFailure()
    {
        FMODUnity.RuntimeManager.PlayOneShot(failure, mainCamera.transform.position);
    }

    public void PlayGrab()
    {
        FMODUnity.RuntimeManager.PlayOneShot(grab, mainCamera.transform.position);
        FMODUnity.RuntimeManager.PlayOneShot(veggieScream, mainCamera.transform.position);
    }

    public void PlayLoss()
    {
        FMODUnity.RuntimeManager.PlayOneShot(loss, mainCamera.transform.position);
    }

    public void PlayBackgroundMusic()
    {
        FMODUnity.RuntimeManager.PlayOneShot(backgroundMusic, mainCamera.transform.position);
    }

    public void PlayArrowDrop()
    {
        FMODUnity.RuntimeManager.PlayOneShot(arrowDrop, mainCamera.transform.position);
    }

    public void PlayMenuAdvance()
    {
        FMODUnity.RuntimeManager.PlayOneShot(menuAdvance, mainCamera.transform.position);
    }
}
