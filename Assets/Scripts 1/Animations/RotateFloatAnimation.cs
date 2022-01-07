using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFloatAnimation : MonoBehaviour
{
    private Quaternion initialRotation;
    private float xRotationAmount;
    private float yRotationAmount;
    private float zRotationAmount;
    public float animationTimeLength = 2f;
    public float timer = 0;
    public float yRotateDegreesPerSec = 30f;
    public float rampedYRotateDegreesPerSec;
    public float floatHeight = .5f;
    public float initialHeight;  //transform position at beginning of each frame. 
    private float previousHeight = 0;
    private float newHeight = 0;
    public float rampValue;  //rampValue will go from 0 > 1 > 0 during the time "animationTimeLength".  
    public bool rampSinusoidalMode = true;  //When true the ramp will be sinusoidal, from 0 > 1 > 0 
    public float percentTimeComplete = 0;

    // Start is called before the first frame update
    void Start()
    {
      //  initialRotation = transform.localRotation;
       // xRotationAmount = initialRotation.eulerAngles.x;
        //yRotationAmount = initialRotation.eulerAngles.y;
        //zRotationAmount = initialRotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {

        percentTimeComplete = timer / animationTimeLength;  // 0 at start, 1 when animation should be complete
        calculateRampValue();
        RotateObject();
        FloatObject();

        if (timer > animationTimeLength)
        {
            Destroy(this);  //Remove animation script once timer is done. 
        }

    }

    private void FloatObject()
    {
        initialHeight = transform.position.y;
        newHeight = floatHeight * rampValue;

        float heightChange = newHeight - previousHeight;
        transform.localPosition += new Vector3(0, heightChange, 0);

        previousHeight = newHeight;
    }

    private void RotateObject()
    {
        rampedYRotateDegreesPerSec = yRotateDegreesPerSec * rampValue;
        transform.Rotate(new Vector3(0, rampedYRotateDegreesPerSec *10* Time.deltaTime, 0), Space.Self);
        timer += Time.deltaTime;
    }

    public void AddHeight()
    {
 //       yRotateDegreesPerSec += 22.5f;
    }

    private void calculateRampValue()
    {
        if (rampSinusoidalMode == true)
        {
            rampValue = 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * percentTimeComplete + Mathf.PI * 1.5f));
        }

    }
}
