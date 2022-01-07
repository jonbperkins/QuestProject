using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum AnimateFunction { RotateTool, MoveTool, OrbitTool, UpDownTool, RotateFloatTool, DrumsTool, GrowShrinkTool, AnimationLockTool, DimTool };

public class AnimateTool : MonoBehaviour
{
    public SceneData userCreation;
    public ControlState controlState;
    //public AudioManager audioManager;
     public AnimationSegment activeAnimationSegment;
    public SelectCollider selectCollider;  //Contains list of gameobjects that are selected (collide w/ selector sphere)


    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.AnimateTool)
        {
            selectCollider.ChangeSphereSize(5);
            CheckForSelectedObjects();
        }

    }

    private void CheckForSelectedObjects()
    {
        int SelectColliderObjectCount = selectCollider.GetListSize();
        for (int i = 0; i < SelectColliderObjectCount; i++)
        {
            ProcessSelectedObject(selectCollider.Pop());
        }
    }

    void ProcessSelectedObject(GameObject obj)
    {
        if (obj.CompareTag("SceneObject") && activeAnimationSegment != null)
        {
            AnimationInstance animationInstance = obj.AddComponent<AnimationInstance>();
            animationInstance.animationSegment = activeAnimationSegment;
        }
    }

   
}
