using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ControlState", menuName = "ControlStateScriptableObject", order = 51)]

public class ControlState : ScriptableObject
{
    public enum ControlMode {Create, Menu,  AnimateMenu, AnimationGroupSelect, AnimateTool, SelectTool,  DeleteTool, AnimateLockTool, Record, Player};

    public ControlMode mode = ControlMode.Create;  //Start w/ Create Mode

    public float zoomFactor;  //1 == No Zoom
    public Vector3 zoomRefPoint;
    public Vector3 userCreationPos;

    public bool snapToGridModeOn;
    public bool stretchModeOn;
    public bool repeatModeOn;

    public Vector3 HMDPosVector;

    public Vector3 rightHandPosVector;
    public Vector3 rightHandVelocity;
    public Vector3 leftHandPosVector;
    public Vector3 leftHandVelocity;

    public Quaternion rightHandRotationVector;
    public Quaternion leftHandRotationVector;

    public bool rightTriggerPress;
    public bool rightTriggerOnPress;  //True for one frame after rightTriggerPress goes False>True

    public bool leftTriggerPress;
    public bool leftTriggerOnPress;  //True for one frame after leftTriggerPress goes False>True

    public bool rightButtonAPress;
    public bool rightButtonAOnPress;
    public bool rightButtonBPress;
    public bool rightButtonBOnPress;

    public bool leftButtonXPress;
    public bool leftButtonXOnPress;
    public bool leftButtonYPress;
    public bool leftButtonYOnPress;


    public Vector2 rightJoystickPosition;
    public Vector2 leftJoystickPosition;

    public bool rightGripPress;
    public bool leftGripPress;
    public bool rightGripOnPress;
    public bool leftGripOnPress;

    public bool leftOptionOnPress;

    public Vector3 pointerPosition;
    public Quaternion pointerRotation;
}