using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public SimpleAudioEvent simpleAudioEvent;
    public int currentNote = 1;
    public int maxNote = 25;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentNote > maxNote)
        {
            currentNote = 1;
        }
    }
}
