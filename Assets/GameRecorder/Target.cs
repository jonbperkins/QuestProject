using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    public float preTime;
    public float postTime;
    public float timer = 0;
    private Recorder recorder;
    private Replayer replayer;
    public bool rightHandObj;
    Vector3 targetPos;
    Vector3 startPos;
    public Vector3 moveAmount;
    public float rotateDegPerSecond;
    public float startZoomFactor;
    public float endZoomFactor;
    private Vector3 startZoomVector;
    
    // Start is called before the first frame update
    void Start()
    {
        replayer = (Replayer)FindObjectOfType(typeof(Replayer));
        recorder = (Recorder)FindObjectOfType(typeof(Recorder));
        startZoomVector = new Vector3(.2f, .2f, .2f) * startZoomFactor;
        transform.localScale = Vector3.zero;
        targetPos = this.transform.localPosition;
        if (replayer.isReplaying == true)
        {
            startPos = this.transform.localPosition + moveAmount;
        }
        else
        {
            startPos = this.transform.localPosition;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < preTime)
        {
            this.transform.localPosition = Vector3.Lerp(startPos, targetPos, timer / preTime);
            this.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(.2f, .2f, .2f) * endZoomFactor, timer / preTime);
            this.transform.Rotate(new Vector3(0, rotateDegPerSecond * Time.deltaTime, 0));
        }
        else
        {
            this.transform.localPosition = Vector3.Lerp(targetPos, targetPos - moveAmount, (timer - preTime) / postTime);
            this.transform.localScale = Vector3.Lerp(new Vector3(.2f, .2f, .2f) * endZoomFactor, Vector3.zero, (timer - preTime) / postTime);
            this.transform.Rotate(new Vector3(0, rotateDegPerSecond * Time.deltaTime, 0));
        }
        timer += Time.deltaTime;
        if (timer > preTime + postTime)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "RightHand" && rightHandObj == true && replayer.isReplaying == true)
        {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
           Invoke("TurnRHapticOff", .1f);
           gameObject.SetActive(false);
           Destroy(gameObject, .2f);

        }

        else if (collider.tag == "LeftHand" && rightHandObj == false && replayer.isReplaying == true)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.LTouch);
            Invoke("TurnLHapticOff", .1f);
            gameObject.SetActive(false);
            Destroy(gameObject, .2f);
        }

        else if (collider.tag == "RightHand" && recorder.isRecording == true)
        {
            recorder.RecordRHObject(this.transform);

        }

        else if (collider.tag == "LeftHand" && recorder.isRecording == true)
        {
            recorder.RecordLHObject(this.transform);
        }
    }


       void TurnLHapticOff()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }

    void TurnRHapticOff()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}
