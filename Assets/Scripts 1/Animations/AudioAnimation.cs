using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAnimation : MonoBehaviour
{

    //public float beatRepeatTime = 100f;

    //private float objScaling;
    private float scaledPitch = 1;

    //private float timer = 100;  //initialize at 5 so that it will play first note immediately
    private float elapsedTime = 0;
    public float duration =100f;

    private AudioManager audioManager;
    public SimpleAudioEvent simpleAudioEvent;
    public AudioSource audioSource;
    private int note;


    // Start is called before the first frame update
    void Start()
    {

        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        simpleAudioEvent = audioManager.simpleAudioEvent;
        //note = audioManager.currentNote;

        //objScaling = (transform.localScale.x + transform.localScale.y + transform.localScale.z) * .33f ;

        //scaledPitch = .2f * Mathf.Pow(1.05946f, note);

        //if (scaledPitch < 0)
        //{
        //    scaledPitch = 0;
       // }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }

        simpleAudioEvent.PlayWithParameters(audioSource, (duration - elapsedTime) / duration, scaledPitch);

    }

    void Update()
    {
        /*
        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;
        if (timer > beatRepeatTime)
        {
            timer = 0;
            simpleAudioEvent.PlayWithParameters(audioSource, (duration - elapsedTime) / duration, scaledPitch);  //Play Audio pitch depending on object's scaling

        }


        if (elapsedTime > duration)
        {
            Destroy(this);
        }
        */
    }
}
