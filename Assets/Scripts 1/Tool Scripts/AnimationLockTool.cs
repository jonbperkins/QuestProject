using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationLockTool : MonoBehaviour
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

    void Start()
    {
        tempParentObj = new GameObject();
    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.AnimateLockTool && selectToolIsActive != true)  //Once SelectTool is no longer active, undo the temporary parent and ensure each is not disactive due to flashing
        {
            selectedObjects.Clear();
            foreach (GameObject obj in userCreation.objectList)
            {
                if (obj.gameObject.CompareTag("AnimationLockedObject") == true)  //Each time Animate Lock Tool is initialized, look through all scene objects for existing animation locked objects. 
                {
                    selectedObjects.Add(obj);
                }
            }
            selectToolIsActive = true;
        }

        else if (controlState.mode == ControlState.ControlMode.AnimateLockTool && selectToolIsActive == true)
        {
            selectCollider.ChangeSphereSize(1.5f);
            CheckForNewSelectedObjects();
            FlashSelectedObjects();
            selectToolIsActive = true;
        }

        else if (controlState.mode != ControlState.ControlMode.AnimateLockTool && selectToolIsActive == true)  //Once SelectTool is no longer active, undo the temporary parent and ensure each is not disactive due to flashing
        {
            ResetObjectsUponExit();
            selectToolIsActive = false;
        }
    }

    private void ResetObjectsUponExit()
    {
        foreach (GameObject obj in selectedObjects)
        {
            obj.SetActive(true);  //If object was off (for flashing), then re-enable. 
        }
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
        else  //Unless the right trigger is pressed (if enabled), remove objects from the Selection Collider list.
        {
            for (int i = 0; i < selectColliderObjectCount; i++)
            {
                selectCollider.Pop();
            }
        }

    }

    void ProcessSelectedObject(GameObject obj)
    {
        if (obj.gameObject.CompareTag("SceneObject"))
        {
            obj.gameObject.tag = "AnimationLockedObject";
            selectedObjects.Add(obj);
            
        }

        else if (obj.gameObject.CompareTag("AnimationLockedObject"))
        {
            obj.gameObject.tag = "SceneObject";
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
                if (obj != null) 
                {
                    obj.SetActive(false);
                }
                else //If an object that was animation locked was deleted, this will remove from the animation locked objects list.
                {
                    selectedObjects.Remove(obj);
                }
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
}
