using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetToRecord : MonoBehaviour
{
    private Recorder recorder;
    private Replayer replayer;
    bool isColliding = false;
    //public Vector3 moveAmount;
    //public float rotateDegPerSecond;
   // public float startZoomFactor;
    //public float endZoomFactor;
    
    // Start is called before the first frame update
    void Start()
    {
        replayer = (Replayer)FindObjectOfType(typeof(Replayer));
        recorder = (Recorder)FindObjectOfType(typeof(Recorder));

    }

    // Update is called once per frame
    void Update()
    {
        if (replayer.isReplaying == true)
        {
      //      this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
       else
        {
      //      this.GetComponent<MeshRenderer>().enabled = true;
            this.GetComponentInChildren<MeshRenderer>().enabled = true;
        }
        isColliding = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isColliding) return;
        isColliding = true; 

        if (collider.tag == "RightHand" && recorder.isRecording == true)
        {
            // recorder.RecordRHObject(this.transform, moveAmount, rotateDegPerSecond, startZoomFactor, endZoomFactor);  //todo Add additional parmaters to customize target behaviour
            recorder.RecordRHObject(this.transform);

        }

        else if (collider.tag == "LeftHand" && recorder.isRecording == true)
        {
            recorder.RecordLHObject(this.transform);
        }
    }
}
