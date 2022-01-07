using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation Segment")]

public class AnimationSegment : ScriptableObject

{
    public float delayTime = 0;
    //Rotate Variables
    public Vector3 rotateDirection;
    public float rotationDegPerSec;
    public float targetRotations;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN.  Stops after this number of rotations.
    public FunctionType functTypeRotation;
    public bool rotateTowardsRightHand;
    [Space(25)]

    //Move Variables
    public Vector3 moveDirection;
    public float moveCyclesPerSec;
    public float moveDistanceCM;
    public float targetMoveCycles;  //Used for RAMP_RTN, or SIN_RAMP_RTN. Stops after this number of cycles.
    public FunctionType functTypeMove;
    [Space(25)]

    //Scale Variables
    public Vector3 scaleDirection;
    public float scaleCyclesPerSec;
    public float scaleShrinkPercent; //-100% or above.  -100% will shrink to Scale of 0;
    public float targetScaleCycles;  //Used for CONTINUOUS, RAMP_RTN, or SIN_RAMP_RTN. 
    public FunctionType functTypeScale;
    [Space(25)]

    //Recolor Variables
    //public Material recolorMaterial;
    public float intensity;
    public float recolorCyclesPerSec;
    public float recolorTargetCycles;
    public FunctionType functTypeRecolor;

    public bool useObjectAxis;

}
