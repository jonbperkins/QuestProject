using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoRedoTool : MonoBehaviour
{

    public ControlState controlState;  //Holds state of controllers
    public SceneData userCreation;
    float startTimer = .35f;
    float minTimer = .05f;
    float timer = .35f;
    float index = 0;

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.Create && (controlState.leftButtonXOnPress || controlState.leftButtonYOnPress))
        {
            timer = startTimer;
            UndoRedo();
        }

        else if (controlState.mode == ControlState.ControlMode.Create && (controlState.leftButtonXPress || controlState.leftButtonYPress))
        {
            if (timer < 0)
            {
                UndoRedo();              //  If left hand trackpad is pressed looks to see if this was an undo or redo command
                timer = startTimer - index * .1f;
                if (timer > minTimer)
                {
                    index++;
                }
            }
            timer -= Time.deltaTime;

        }
        else
        {
            timer = startTimer;
            index = 0;
        }

    }

    public void UndoRedo()
    {
        if (controlState.leftButtonXPress)
        {
            if (userCreation.objectList.Count > 0)
            {
                userCreation.Remove(userCreation.objectList[userCreation.objectList.Count - 1]);
            }
        }
        else if (controlState.leftButtonYPress)  //For Future Use (Redo)
        {
            //GameObject tempHoldingForRedo = createdObjects[createdObjectsIndex];
            //createdObjects[createdObjectsIndex] = Instantiate(tempHoldingForRedo, UserCreatedObjectParent.transform);
            //createdObjectsIndex++;
        }
    }
}
