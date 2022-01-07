using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingDisplay : MonoBehaviour
{
    public Settings settings;

    public string settingName;
        
    public float currentSettingValue = 100;
    public float minCurrentSettingValue = 0;
    public float maxCurrentSettingValue = 10000;
    public float adjustGain = 1;
    public string valueString;  //PB value, converted to string w/ 2 decimal places

    public ControlState controlState;  //Holds state of controllers
    //public SceneData userCreation;

    bool valueUpdateStarted = false;
    float startTimer = .35f;
    float minTimer = .04f;
    float timer = .35f;
    public float index = 0;

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.Create && controlState.stretchModeOn == true)
        {
            foreach (MeshRenderer mR in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mR.enabled = true;
            }
        }

        if (controlState.mode == ControlState.ControlMode.Create && controlState.leftJoystickPosition.x > .7f)
        {
            currentSettingValue *= (1 + Time.deltaTime * adjustGain);
            //IncrementSetting();
        }
        if (controlState.mode == ControlState.ControlMode.Create && controlState.leftJoystickPosition.x < -.7f)
        {
            currentSettingValue /= (1 + Time.deltaTime * adjustGain);
            //DecrementSetting();
        }
        
        if (controlState.mode != ControlState.ControlMode.Create || controlState.stretchModeOn != true)
        {
            foreach (MeshRenderer mR in this.gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mR.enabled = false;
            }
        }

        writeValueToDisplay();
        settings.yStretchModifier = currentSettingValue * .01f;
        this.transform.position = controlState.leftHandPosVector;

        this.transform.rotation = controlState.leftHandRotationVector;
        this.transform.Rotate(90, 0, 0);
        this.transform.position += transform.forward * -.05f;
    }

    private void writeValueToDisplay()
    {
        if (currentSettingValue < 10)
        {
            valueString = "Y Stretch Pct:" + '\n' + currentSettingValue.ToString("F2");
        }
        else  //Lose decimal point when >10
        {
            valueString = "Y Stretch Pct:" + '\n' + currentSettingValue.ToString("F0");
        }
        this.GetComponentInChildren<TMPro.TextMeshPro>().text = valueString;
    }

    private void IncrementSetting()
    {
        if (valueUpdateStarted == false)
        {
            timer = startTimer;
            currentSettingValue += calculateIncrementAmount();
            valueUpdateStarted = true;
        }

        else if (valueUpdateStarted == true)
        {
            if (timer < 0)
            {
                //calculateIncrementAmount();
                currentSettingValue += calculateIncrementAmount();
                timer = startTimer - index * .02f;
                if (timer > minTimer)
                {
                    index++;
                }
            }
            timer -= Time.deltaTime;

        }

    }

    private void DecrementSetting()
    {
        if (valueUpdateStarted == false)
        {
            timer = startTimer;
            currentSettingValue -= calculateDecrementAmount();
            valueUpdateStarted = true;
        }

        else if (valueUpdateStarted == true)
        {
            if (timer < 0)
            {
                currentSettingValue -= calculateDecrementAmount();          
                timer = startTimer - index * .02f;
                if (timer > minTimer)
                {
                    index++;
                }
            }
            timer -= Time.deltaTime;

        }

        if (currentSettingValue < minCurrentSettingValue)
        {
            currentSettingValue = minCurrentSettingValue;
        }

    }

    private float calculateIncrementAmount ()
    {
        if (currentSettingValue < .1f)
        {
            return .01f;
        }
        else if (currentSettingValue < .999f)
        {
            return .01f;
        }
        else if (currentSettingValue < 10f)
        {
             return .1f;
        }
        else if (currentSettingValue < 200f)
        {
            return 1f;
        }

        else if (currentSettingValue < 1000f)
        {
            return 10f;
        }

        else if (currentSettingValue < 10000f)
        {
            return 100f;
        }

        else 
        {
            return 1000f;
        }
    }

    private float calculateDecrementAmount()
    {
        if (currentSettingValue < .11f)
        {
            return .01f;
        }
        else if (currentSettingValue < 1.1f)
        {
            return .01f;
        }
        else if (currentSettingValue < 11.1f)
        {
            return .1f;
        }
        else if (currentSettingValue < 201f)
        {
            return 1f;
        }

        else if (currentSettingValue < 1001f)
        {
            return 10f;
        }

        else if (currentSettingValue < 10001f)
        {
            return 100f;
        }

        else
        {
            return 1000f;
        }
    }

}
