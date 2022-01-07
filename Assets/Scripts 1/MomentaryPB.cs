using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MomentaryPB : MonoBehaviour
{
    public string buttonName;

    public AudioSource audioSource;
    public AudioClip onSound;
    public UnityEvent pressedEvent;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        this.GetComponentInChildren<TMPro.TextMeshPro>().text = buttonName;
    }
    void Update()
    {

    }


    public void Press()
    {
        audioSource.clip = onSound;
        audioSource.Play();
        pressedEvent.Invoke();
    }
}
