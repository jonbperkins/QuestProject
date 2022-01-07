using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustingPB : MonoBehaviour
{
    public float value;
    public string valueString;  //PB value, converted to string w/ 2 decimal places

    public float xValue;
    public float xMin;
    public float xMax;

    public float yValue;
    public float yMin;
    public float yMax;

    public float zValue;
    public float zMin;
    public float zMax;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (value < 10)
        {
            valueString = "x: " + value.ToString("F2") + "\ny:" + value.ToString("F2") + "\nz:" + value.ToString("F2");
        }
        else  //Lose decimal point when >10
        {
            //value = Mathf.Floor(value);
            valueString = "x: " + value.ToString("F0") + "\ny:" + value.ToString("F0") + "\nz:" + value.ToString("F0"); ;
        }
        this.GetComponentInChildren<TMPro.TextMeshPro>().text = valueString;
    }

    public void Increase()
    {
        value *= (1 + Time.deltaTime);
//        if (value >= 99f) value += 100f;
//        else if (value > 9.9f) value += 10f;
//        else if (value > .99f) value += 1f;
//        else if (value > .099f) value += .1f;
 //       else value += .01f;
    }

    public void Decrease()
    {
        value /= (1 + Time.deltaTime);
        // if (value < .01f) value = 0f;
        // else if (value < .1f) value -= .01f;
        // else if (value < 1f) value -= .1f;
        // else if (value < 5f) value -= 1f;
        // else if (value < 20f) value -= 2f;
        // else value -= 10f;

    }
}
