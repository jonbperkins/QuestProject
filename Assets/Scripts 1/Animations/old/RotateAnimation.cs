using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    private Quaternion initialRotation;
    public float xRotationAmount;
    public float yRotationAmount;
    public float zRotationAmount;
    public float xRotateDegreesPerSec = 50f;
    public float yRotateDegreesPerSec = 0f;
    public float zRotateDegreesPerSec = 0f;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.localRotation;
        xRotationAmount = initialRotation.eulerAngles.x;
        yRotationAmount = initialRotation.eulerAngles.y;
        zRotationAmount = initialRotation.eulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, yRotateDegreesPerSec * Time.deltaTime, 0), Space.Self);
        transform.Rotate(new Vector3(xRotateDegreesPerSec * Time.deltaTime, yRotateDegreesPerSec * Time.deltaTime, zRotateDegreesPerSec * Time.deltaTime), Space.Self);
    }

    public void AddRotation()
    {
        zRotateDegreesPerSec += 22.5f;
    }
}
