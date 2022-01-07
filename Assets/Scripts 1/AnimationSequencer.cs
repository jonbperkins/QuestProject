using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationGroup
{
    public int groupNumber;
    public List<int> objectNumbers;
    public List<float> startTimes;
    public int startIndex = 0;
    public List<AnimationSegment> animationSegments;
}

public class AnimationSequencer : MonoBehaviour
{
    public AudioClip[] songs;
    public int songNumber = 0;
    public float songTime;

    public  List<AnimationGroup> animationGroups;
    public ControlState controlState;
    private AudioSource audioSource;

    public bool recordGroup = false;
    public bool recordAllGroups = false;
    //public List<float> group1StartTimes = new List<float>();
    //public int group1StartTimesIndex = 0;
    //public List<float> group1Objects;

    public bool play = false;

    public SceneData sceneData;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {


        if (recordAllGroups == true)
        {
            Start_RecordAll();
        }

        if (play == true)
        {
            Play();
        }

        if (play != true && recordAllGroups != true && audioSource.isPlaying == true)
        {
            audioSource.Stop();
        }
               


      //  if (controlState.leftTriggerOnPress == true)
      //  {

    //    }
        


    }

    private void Play()  //TODO:  
    {
        PlayAudio(songNumber);
        for (int i = 0; i < animationGroups.Count-1; i++)  //i keeps track of each Animation Group, a Set of Objects that will each be animated at each start time.
        {
            float nextStartTime = animationGroups[i].startTimes[animationGroups[i].startIndex];
            if (audioSource.time > nextStartTime)
            {
                for (int j = 0; j < animationGroups[i].objectNumbers.Count-1; j++)  //j keeps track of each object that is in this animation group
                {
                    for (int k = 0; k < animationGroups[i].animationSegments.Count-1; k++)  //k keeps track of each animation segment that will be added to each object in animation group
                    {
                        sceneData.objectList[animationGroups[i].objectNumbers[j]].AddComponent<AnimationInstance>().animationSegment = animationGroups[i].animationSegments[k];
                    }
                }
                animationGroups[i].startIndex++;
            }
        }

        if (audioSource.time > (songs[songNumber].length-1))
        {
            play = false;
            Debug.Log("Play song is over");
        }
    }

    private void Start_RecordAll() 
    {
        PlayAudio(songNumber);

        //Record group 0 trigger when left trigger is pressed   TODO:  Set up so that it can have different combinations of what is being recorded.
        if (controlState.leftTriggerOnPress == true)
        {
            animationGroups[0].startTimes.Add(audioSource.time);  //This records the time
                for (int i = 0; i < animationGroups[0].objectNumbers.Count; i++)  //This adds animations to current objects so that the animations are seen as recording is happening. 
                {
                    for (int j = 0; j < animationGroups[0].animationSegments.Count; j++)
                    {
                        sceneData.objectList[animationGroups[0].objectNumbers[i]].AddComponent<AnimationInstance>().animationSegment = animationGroups[0].animationSegments[j];
                    }
                }
        }

        //Record group 1 trigger when left grip is pressed  
        if (controlState.leftGripOnPress == true)
        {
            animationGroups[1].startTimes.Add(audioSource.time);  //This records the time
            for (int i = 0; i < animationGroups[1].objectNumbers.Count; i++)  //This adds animations to current objects so that the animations are seen as recording is happening. 
            {
                for (int j = 0; j < animationGroups[1].animationSegments.Count; j++)
                {
                    sceneData.objectList[animationGroups[1].objectNumbers[i]].AddComponent<AnimationInstance>().animationSegment = animationGroups[1].animationSegments[j];
                }
            }
        }




        if (audioSource.time > (songs[songNumber].length - 1))
        {
            recordAllGroups = false;
            Debug.Log("Recording All Group is OVer");
        }

        //Record group 0 trigger when left trigger is pressed
   
        if (controlState.leftTriggerOnPress == true)
        {
            animationGroups[0].startTimes.Add(audioSource.time);
        }
        if (audioSource.time > (songs[songNumber].length - 1))
        {
            recordAllGroups = false;
            Debug.Log("Recording All Group is OVer");
        }


    }



    private void PlayAudio(int songNumber)
    {
        if (songs[songNumber] != null)
        {
            audioSource.clip = songs[songNumber];
            if (audioSource.isPlaying != true)
            {
                audioSource.Play();
            }

        }
    }
}
