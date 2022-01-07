using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetControllerInput : MonoBehaviour
{
    
    public GameObject RightHandObject;
    public ControlState controlState;  //Scriptable Object that will hold all of the controls related variables
    
    public GameObject LeftHandObject;

    public Transform HMDObject;


    public Vector3 pointerOffset = new Vector3 (0, 0.025f, .1f);

    private Vector3 previousRHPos;  // Used to calculate hand velocity
    private Vector3 previousLHPos;  // Used to calculate hand velocity

    void Start()
    {

    }

    void Update()
    {

        controlState.rightTriggerPress = OVRInput.Get(OVRInput.RawButton.RIndexTrigger);  // True = Trigger is being pressed/held
        controlState.leftTriggerPress = OVRInput.Get(OVRInput.RawButton.LIndexTrigger);  // True = Trigger is being pressed/held
        controlState.rightTriggerOnPress = OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger);   //True for one frame, after trigger is pressed
        controlState.leftTriggerOnPress = OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger); ;   //True for one frame, after trigger is pressed

        controlState.rightButtonAPress = OVRInput.Get(OVRInput.RawButton.A);
        controlState.rightButtonBPress = OVRInput.Get(OVRInput.RawButton.B);
        controlState.rightButtonAOnPress = OVRInput.GetDown(OVRInput.RawButton.A);
        controlState.rightButtonBOnPress = OVRInput.GetDown(OVRInput.RawButton.B);

        controlState.leftButtonXPress = OVRInput.Get(OVRInput.RawButton.X);
        controlState.leftButtonYPress = OVRInput.Get(OVRInput.RawButton.Y);
        controlState.leftButtonXOnPress = OVRInput.GetDown(OVRInput.RawButton.X);
        controlState.leftButtonYOnPress = OVRInput.GetDown(OVRInput.RawButton.Y);


        controlState.rightJoystickPosition = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        controlState.leftJoystickPosition = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick);

        controlState.rightGripPress = OVRInput.Get(OVRInput.RawButton.RHandTrigger);
        controlState.leftGripPress = OVRInput.Get(OVRInput.RawButton.LHandTrigger); 
        controlState.rightGripOnPress = OVRInput.GetDown(OVRInput.RawButton.RHandTrigger);
        controlState.leftGripOnPress = OVRInput.GetDown(OVRInput.RawButton.LHandTrigger);


        controlState.leftOptionOnPress = OVRInput.GetDown(OVRInput.RawButton.Start);

        controlState.rightHandPosVector = RightHandObject.transform.position;
        controlState.rightHandRotationVector = RightHandObject.transform.rotation;

       //CalculateRightHandVelocity();
        //CalculateLeftHandVelocity();

        controlState.leftHandPosVector = LeftHandObject.transform.position;
        controlState.leftHandRotationVector = LeftHandObject.transform.rotation;

        controlState.pointerRotation = RightHandObject.transform.rotation;
        controlState.pointerPosition = RightHandObject.transform.position + RightHandObject.transform.rotation * pointerOffset; //Reference Point for attaching objects and tool/selection collider

        controlState.HMDPosVector = HMDObject.transform.position; 
    }

    private void CalculateLeftHandVelocity()
    {
        controlState.leftHandVelocity = (controlState.leftHandPosVector - previousRHPos) / Time.deltaTime;
        previousLHPos = controlState.leftHandPosVector;
    }

    private void CalculateRightHandVelocity()
    {
        controlState.rightHandVelocity = (controlState.rightHandPosVector - previousRHPos) / Time.deltaTime;
        previousRHPos = controlState.rightHandPosVector;
    }
}
