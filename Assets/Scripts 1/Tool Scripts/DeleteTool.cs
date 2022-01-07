using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeleteTool : MonoBehaviour
{
    public SceneData sceneData;
    public ControlState controlState;
    public SelectCollider selectCollider;  //Contains list of gameobjects that are selected (collide w/ selector sphere)

    private bool deleteToolIsActive;
    public bool requireTriggerToSelect = false;

    void Start()
    {

    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.DeleteTool)
        {
            selectCollider.ChangeSphereSize(3f);
            DeleteSelectedObjects();
        }

    }


    private void DeleteSelectedObjects()
    {
        int SelectColliderObjectCount = selectCollider.GetListSize();
        if (controlState.rightTriggerPress == true || requireTriggerToSelect == false)
        {
            for (int i = 0; i < SelectColliderObjectCount; i++)
            {
                sceneData.GetComponent<SceneData>().Remove(selectCollider.Pop());
            }
        }
        else  //If trigger is required, but isn't pressed then remove from selectCollider list but don't delete
        {
            for (int i = 0; i < SelectColliderObjectCount; i++)
            {
                selectCollider.Pop();
            }
        }

    }
}
