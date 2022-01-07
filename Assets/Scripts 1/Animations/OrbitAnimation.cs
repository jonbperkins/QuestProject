using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OrbitAnimation : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 orbitCenterPosition;
    public float degPerSec = 270f;
    public float orbitDist = 0.05f;
    public Vector3 orbitAxis = new Vector3 (0, 1f, 0);
    public float currentAngle = 0;
    private Quaternion startingRotation;
    private Vector3 startingPosition;

    private float rampValue;
    public bool temporaryAnimation = true;
    public float animationTime = 15f;
    private float percentComplete = 0;


    public float elapsedTime = 0;

    void Start()
    {
        initialPosition = transform.localPosition;

    }

    void Update()
    {
        if (temporaryAnimation == true)
        {
            percentComplete = elapsedTime / animationTime;
            calculateRampValue();

        }

        startingRotation = transform.rotation;
        startingPosition = transform.position;
        float xPosition = Mathf.Sin(degPerSec / 360 * elapsedTime * 2 * Mathf.PI) * orbitDist * rampValue;
        float zPosition = Mathf.Cos(degPerSec / 360 * elapsedTime * 2 * Mathf.PI) * orbitDist * rampValue;



        elapsedTime += Time.deltaTime;


        transform.localPosition = new Vector3 (xPosition + initialPosition.x, initialPosition.y, zPosition + initialPosition.z);

        transform.rotation = startingRotation;

        if (elapsedTime > animationTime)
        {
            Destroy(this);  //Remove animation script once timer is done. 
        }


    }

    public void AddOrbitDist()
    {
        orbitDist += .05f;
        //animationTime += 3f;
    }

    private void calculateRampValue()
    {
        if (temporaryAnimation == true)
        {
            rampValue = 0.5f * (1 + Mathf.Sin(2 * Mathf.PI * percentComplete + Mathf.PI * 1.5f));
        }

    }
}
