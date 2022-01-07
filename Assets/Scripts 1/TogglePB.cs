using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TogglePB : MonoBehaviour
{

    public bool value;
    private bool lastValue;
    public bool onRise;
    public bool onFall;

    public string buttonName;

    public AudioSource audioSource;
    public AudioClip onSound;
    public AudioClip offSound;

    public GameObject indicator;

    public UnityEvent onEvent;
    public UnityEvent offEvent;

    // Start is called before the first frame update
    void Start()
    {
        indicator = transform.Find("Indicator").gameObject;
        audioSource = GetComponent<AudioSource>();
        this.GetComponentInChildren<TMPro.TextMeshPro>().text = buttonName;
    }
    // Update is called once per frame
    void Update()
    {
        ShowIndicator();
        CheckRiseFall();
    }

    private void ShowIndicator()
    {
        if (value == true)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    private void CheckRiseFall()
    {
        if (value == true && lastValue == false)
        {
            audioSource.clip = onSound;
            audioSource.Play();
            onRise = true;
            onFall = false;
        }

        else if (value == false && lastValue == true)
        {
            audioSource.clip = onSound;
            audioSource.Play();
            onRise = false;
            onFall = true;
        }
        else
        {
            onRise = false;
            onFall = false;
        }

        lastValue = value;
    }

    public void Toggle()
    {
        if (value == true)
        {
            value = false;
            offEvent.Invoke();
        }
        else if (value == false)
        {
            value = true;
            onEvent.Invoke();
        }
    }
}
[System.Serializable]
public class ToggleEvent : UnityEvent { }