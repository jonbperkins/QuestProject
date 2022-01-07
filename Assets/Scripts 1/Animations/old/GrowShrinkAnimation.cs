using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowShrinkAnimation : MonoBehaviour
{
    private Vector3 initialScale;
    public float sinusoidAmount;

    public float shrinkPercent = 10;
    public float cyclesPerSec = 0.15f;

    public float elapsedTime = 0;

    void Start()
    {

        //initialScale = transform.localPosition;
        initialScale = transform.localScale;
    }

    void Update()
    {

        sinusoidAmount = (1.01f - Mathf.Cos(2.0f * 3.14f * elapsedTime * cyclesPerSec)) * 0.5f;  //Sin wave between 0 and 1


        transform.localScale = initialScale  - initialScale * sinusoidAmount * shrinkPercent * .01f;
        elapsedTime += Time.deltaTime;
    }

    public void AddGrowShrink()
    {
        shrinkPercent += 10;
        if (shrinkPercent > 90)
        {
            shrinkPercent = 99f;
        }
    }
}
