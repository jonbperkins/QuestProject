using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectTool : MonoBehaviour
{
    public SceneData userCreation;
    public ControlState controlState;

    public List<GameObject> selectedObjects;

    public SelectCollider selectCollider;  //Contains list of gameobjects that are selected (collide w/ selector sphere)



    private GameObject tempParentObj;  //Selected objects will be parented to this object when moving or scaling. 

    private bool selectToolIsActive = false;
    public bool requireTriggerToSelect = true;
    public bool selectedObjectsHeldRH = false;
    public float flashTimerOn = 1f;
    public float flashTimerOff = 1f;
    private float flashTimer = 0;

    private float previousControllerSeparation;
    float scaleFactor = 1;

    void Start()
    {
        tempParentObj = new GameObject();
        tempParentObj.name = "SelectTool Parent Obj";
    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.SelectTool)
        {
            selectCollider.ChangeSphereSize(1.5f);
            CheckForNewSelectedObjects();
            CheckForMoveUI();
            //FlashSelectedObjects();
            if (controlState.leftTriggerPress == true)  //Left trigger used to hide selected objects
            {
                foreach (GameObject obj in selectedObjects)  
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                {
                    foreach (GameObject obj in selectedObjects)
                    {
                        obj.SetActive(true);
                    }
                }
            }
            selectToolIsActive = true;
        }

        else if (controlState.mode != ControlState.ControlMode.SelectTool && selectToolIsActive == true)  //Once SelectTool is no longer active, undo the temporary parent and ensure each is not disactive due to flashing
        {
            ResetObjectsUponExit();
        }
    }

    private void ResetObjectsUponExit()
    {
        foreach (GameObject obj in selectedObjects)
        {
            obj.transform.SetParent(userCreation.gameObject.transform);
            obj.SetActive(true);
        }
        selectedObjects.Clear();
    }

    private void CheckForNewSelectedObjects()
    {
        int selectColliderObjectCount = selectCollider.GetListSize();
        if (controlState.rightTriggerPress == true || requireTriggerToSelect == false)
        {
            for (int i = 0; i < selectColliderObjectCount; i++)
            {
                ProcessSelectedObject(selectCollider.Pop());  //objList[0] is different each time, as the Pop() function removes the previous object. 
            }
        }
        else  //Unless the right trigger is pressed, remove objects from the Selection Collider list.
        {
            for (int i = 0; i < selectColliderObjectCount; i++)
            {
                selectCollider.Pop();
            }
        }

    }

    void ProcessSelectedObject(GameObject obj)
    {
        if (selectedObjects.Contains(obj) == false)
        {
            selectedObjects.Add(obj);
        }
        else
        {
            selectedObjects.Remove(obj);
        }
    }

    private void FlashSelectedObjects()
    {
        flashTimer -= Time.deltaTime;

        if (flashTimer < flashTimerOff)
        {
            foreach (GameObject obj in selectedObjects)
            {
                obj.SetActive(false);
            }
        }
        
        if (flashTimer < 0)
        {
            flashTimer = flashTimerOff + flashTimerOn;
            foreach (GameObject obj in selectedObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    private void CheckForMoveUI()  //Objects are moved using the right grip button.  
    {
        if (controlState.rightGripOnPress == true)  //Called once when right grid first pressed.  Could update to only work if right hand is contacting the selected object group. 
        {
            tempParentObj.transform.position = controlState.pointerPosition;
            tempParentObj.transform.rotation = controlState.pointerRotation;
            tempParentObj.transform.localScale = new Vector3(1, 1, 1);
            scaleFactor = 1;
            foreach (GameObject obj in selectedObjects)
            {
                obj.transform.SetParent(tempParentObj.transform);
            }
            selectedObjectsHeldRH = true;
        }

        if (controlState.rightGripPress == true)  //Called once when right grid first pressed.  
        {
            tempParentObj.transform.position = controlState.pointerPosition;
            tempParentObj.transform.rotation = controlState.pointerRotation;

            if (controlState.rightTriggerOnPress == true)  //Copy selected objects if the right trackpad is pressed while moving the objects (right grip also pressed)
            {
                foreach (GameObject obj in selectedObjects)
                {
                    GameObject newObject = Instantiate(obj, userCreation.gameObject.transform, true);
                    newObject.name = newObject.name.Replace("(Clone)", "");  //Get rid of text as object name is used for SAVE & LOAD
                    userCreation.Add(newObject);
                }
            }

        }

        if (controlState.rightGripPress == true && controlState.leftGripPress == true)
        {
            ScaleSelectedObjects();
        }

        if (selectedObjectsHeldRH == true && controlState.rightGripPress == false)  //To get out of Moving selected objects, reset parent to the userCreation (main parent object). 
        {
            selectedObjectsHeldRH = false;
            foreach (GameObject obj in selectedObjects)
            {
                obj.transform.SetParent(userCreation.gameObject.transform);
            }
        }
    }

    public void ScaleSelectedObjects()
    {
        float controllerSeparation = Mathf.Abs((controlState.rightHandPosVector - controlState.leftHandPosVector).magnitude);
        float controllerSeparationChange = controllerSeparation - previousControllerSeparation;
        if (controlState.leftGripOnPress == true)  //The first time this function is called, don't scale objects to prevent using an invalid value for previous controller separation.
        {
            controllerSeparationChange = 0f;
        }

        scaleFactor *= (1 + controllerSeparationChange);
        tempParentObj.transform.localScale = new Vector3(1, 1, 1) * scaleFactor;
        
        previousControllerSeparation = controllerSeparation;


    }



}
