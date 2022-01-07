using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UPandDownAnimation : MonoBehaviour
{
    private Vector3 initialPosition;
    //public float xMoveAmount;
    public float sinusoidAmount;
    //public float zMoveAmount;
    public float totalYMoveCm = 5;
    public float cyclesPerSec = 0.5f;
    private float prevSinusoidAmount;
   // public ControlState controlState;

    //public bool sinusoidal = true;
    public float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
       
        initialPosition = transform.localPosition;
       // controlState = GameObject.Find("UserCreation").GetComponent<ControlState>();  

    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        sinusoidAmount = (1-Mathf.Cos(2.0f * 3.14f * elapsedTime * cyclesPerSec)) * totalYMoveCm/100;
        transform.localPosition += new Vector3(0, sinusoidAmount - prevSinusoidAmount, 0);
        prevSinusoidAmount = sinusoidAmount;
    }

    void AddUpDown()
    {
        totalYMoveCm += 2;
    }
}
