using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
public class TargetData
{
    public float audioTime;
    public Vector3 position;
    public bool isRightHand;
}

public class Recorder : MonoBehaviour
{
    public ControlState controlState;
    public List<TargetData> targetList;
    public MusicPlayer musicPlayer;
    public Replayer replayer;
    public bool isRecording = false;
    public GameObject RightHandBallPrefab;
    public GameObject LeftHandBallPrefab;
   // public AudioClip songToRecord;

    //public GameObject userCreatedObjectParent;


    void Start()
    {
        targetList = new List<TargetData>();
    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.Record && isRecording == false)
        {
      //      userCreatedObjectParent.SetActive(true);

            isRecording = true;
            replayer.isReplaying = false;
            targetList.Clear();
            musicPlayer.StartMusic();
        }

        if (controlState.mode != ControlState.ControlMode.Record && controlState.mode != ControlState.ControlMode.Player)  // TODO Need to refactor, needed way to stop music not recording/playing
        {
            musicPlayer.StopMusic();
            isRecording = false; 
        }


        /*
        if (isRecording == true)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
            {
                Instantiate(RightHandBallPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch), Quaternion.identity);
                TargetData item = new TargetData();
                item.audioTime = musicPlayer.GetSongTime();
                item.isRightHand = true;
                item.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
                targetList.Add(item);
            }
            if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
            {
                Instantiate(LeftHandBallPrefab, OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch), Quaternion.identity); 
                TargetData item = new TargetData();
                item.audioTime = musicPlayer.GetSongTime();
                item.isRightHand = false;
                item.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                targetList.Add(item);
            }
        }
        */
    }

    public void RecordRHObject(Transform targetTransform)
    {
        Target obj = Instantiate(RightHandBallPrefab, targetTransform.position, targetTransform.rotation).GetComponent<Target>();
       // obj.timer = obj.preTime;
        TargetData item = new TargetData();
        item.audioTime = musicPlayer.GetSongTime();
        item.isRightHand = true;
        item.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        targetList.Add(item);
    }

    public void RecordLHObject(Transform targetTransform)
    {
        Target obj = Instantiate(LeftHandBallPrefab, targetTransform.position, targetTransform.rotation).GetComponent<Target>();
        //obj.timer = obj.preTime;
        TargetData item = new TargetData();
        item.audioTime = musicPlayer.GetSongTime();
        item.isRightHand = false;
        item.position = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        targetList.Add(item);
    }

}
