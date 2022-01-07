using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Replayer : MonoBehaviour
{
    public ControlState controlState;
    List<TargetData> targetList;
    public GameObject RightHandBallPrefab;
    public GameObject LeftHandBallPrefab;
    public MusicPlayer musicPlayer;
    public Recorder recorderData;
    public bool isReplaying = false;
    float nextTime = 0f;
    float currentMusicTime = 0f;
    int replayCounter = 0;

    public GameObject rightPlayerHand;
    public GameObject rightHandController;
    public GameObject leftPlayerHand;
    public GameObject leftHandController;

   // public GameObject userCreatedObjectParent;

    void Start()
    {
        targetList = new List<TargetData>();
    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.Player  || controlState.mode == ControlState.ControlMode.Record)  //move part of this to recorder script
        {
     //       userCreatedObjectParent.SetActive(true);

            rightPlayerHand.SetActive(true);
            leftPlayerHand.SetActive(true);
            rightHandController.SetActive(false);
            leftHandController.SetActive(false);

            currentMusicTime = musicPlayer.GetSongTime();
            if (isReplaying)
            {
                WaitForNext();
            }
            else
            {
                StartReplay();
            }

        }

        else
        {
            rightPlayerHand.SetActive(false);
            leftPlayerHand.SetActive(false);
            rightHandController.SetActive(true);
            leftHandController.SetActive(true);
            isReplaying = false;
        }


    }

    void StartReplay()
    { 
        musicPlayer.StartMusic();
        recorderData.isRecording = false;
        isReplaying = true;
        targetList = recorderData.targetList;
        replayCounter = 0;
    }

    void WaitForNext()
    {
        nextTime = GetTargetTime();
        if (musicPlayer.GetSongTime() >= (nextTime-RightHandBallPrefab.GetComponent<Target>().preTime))
        {
            Vector3 nextPosition = GetTargetPosition();
            bool isRightHand = GetBoolIsRightHand();
            if (isRightHand == false)
            {
                Instantiate(LeftHandBallPrefab, nextPosition, Quaternion.identity);
            }
            else if (isRightHand == true)
            {
                Instantiate(RightHandBallPrefab, nextPosition, Quaternion.identity);
            }
            NextItem();
        }
    }

    public float GetTargetTime()
    {
        return targetList[replayCounter].audioTime;
    }
    public Vector3 GetTargetPosition()
    {
        return targetList[replayCounter].position;
    }
    public bool GetBoolIsRightHand()
    {
        return targetList[replayCounter].isRightHand;
    }
    public void NextItem()
    {
        if (replayCounter < targetList.Count - 1)
        {
            replayCounter++;
        }
    }
}
