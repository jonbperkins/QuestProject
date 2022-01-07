using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public enum ControlMode { Create, Menu, AnimateMenu, AnimationGroupSelect, AnimateTool, SelectTool, DeleteTool, AnimateLockTool };

    public ControlMode mode = ControlMode.Create;  //Start w/ Create Mode

    public float zoomFactor;  //1 == No Zoom
    public Vector3 zoomRefPoint;
    public Vector3 userCreationPos;

    public bool snapToGridModeOn;
    private bool useSinglePlane;
    private bool forceRightAngles;
    private bool forceHorizontal;


    public bool stretchModeOn;
    public float xStretchModifier = 1;
    public float yStretchModifier = 1;
    public float zStretchModifier = 1;

    public bool repeatModeOn;
    private float repeatDistanceMeters = .01f;
    //private bool changeRepeateDistWScale = true;
    //private bool objFaceMovementDirection = false;

    //Animation Settings
    private bool useWorldScale;

    public void SnapToGridModeTurnOn()
    {
        snapToGridModeOn = true;
    }
    public void snapToGridModeTurnOff()
    {
        snapToGridModeOn = false;
    }

    public void StretchModeTurnOn()
    {
        stretchModeOn = true;
    }
    public void stretchModeTurnOff()
    {
        stretchModeOn = false;
    }

    public void RepeatModeTurnOn()
    {
        repeatModeOn = true;
    }
    public void repeatModeTurnOff()
    {
        repeatModeOn = false;
    }


    public void IncreaseRepeatDist()
    {
        repeatDistanceMeters += .001f;
    }

    public void DecreaseRepeatDist()
    {
        if (repeatDistanceMeters > .001f)
        {
            repeatDistanceMeters -= .001f;
        }
    }

}
