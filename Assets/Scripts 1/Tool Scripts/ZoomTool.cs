using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTool : MonoBehaviour  //Zoom tool works by assigning all 
{

    public ControlState controlState;  //Holds state of controllers

    private float previousControllerSeparation;
    private float controllerSeparation;
    private float controllerSeparationChange;

    public GameObject userCreatedObjectParent;
    public  GameObject ZoomRefEmptyGameObject;
    private Vector3 zoomReferencePoint;  //These used to move the entire drawing as both hands move
    private Vector3 previousZoomReferencePoint;
    private Vector3 zoomRotateDirection;



    // Start is called before the first frame update
    void Start()
    {
        ZoomRefEmptyGameObject = Instantiate (new GameObject());  //Zooming is done by setting all scene objects as children to a parent object, then scaling this parent object. 
        ZoomRefEmptyGameObject.name = "ZoomRefEmptyGameObject";
        controlState.zoomFactor = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controlState.mode != ControlState.ControlMode.SelectTool)
        {
            if (controlState.rightGripPress == true && controlState.leftGripPress == true)
            {
                Zoom();
            }
            if (controlState.leftGripPress == true && controlState.leftOptionOnPress == true)  // If these two are pressed together, the zoom level will go back to default. 
            {
                ResetZoom();
            }
        }
    }

    public void Zoom()
    {
        controllerSeparation = Mathf.Abs((controlState.rightHandPosVector - controlState.leftHandPosVector).magnitude);
        controllerSeparationChange = controllerSeparation - previousControllerSeparation;


        if (controlState.rightGripOnPress == true || controlState.leftGripOnPress == true) //On first frame after Zoom is called, calculate a ZoomReferencePoint, and assign the zoomRefEmptyGameObject to have that position.
        {
            ZoomRefEmptyGameObject.transform.DetachChildren();
            GetZoomPositionRotation();  //Sets ZoomReferenceEmptyGameObject position and rotation to halfway between right hand and left hand, facing out. 
            userCreatedObjectParent.transform.SetParent(ZoomRefEmptyGameObject.transform, true); // Set all objects to have parent of ZoomRefEmptyGameObject.  Zooming will now center at that location. 
        }

        else  //This is true every time except the first time this method is called.  Don't change zoom level the first time, otherwise there would be a step change. 
        {
            controlState.zoomFactor *= (1 + controllerSeparationChange);
            ZoomRefEmptyGameObject.transform.localScale = new Vector3(1, 1, 1) * controlState.zoomFactor;
            GetZoomPositionRotation();
        }
        previousControllerSeparation = controllerSeparation;
        previousZoomReferencePoint = zoomReferencePoint;

        controlState.zoomRefPoint = zoomReferencePoint;
 
    }

    private void GetZoomPositionRotation()
    {
        zoomReferencePoint = (controlState.rightHandPosVector + controlState.leftHandPosVector) / 2;
        ZoomRefEmptyGameObject.transform.position = zoomReferencePoint;

        zoomRotateDirection = (controlState.leftHandPosVector - controlState.rightHandPosVector).normalized;
        zoomRotateDirection.y = 0f;
        ZoomRefEmptyGameObject.transform.forward = zoomRotateDirection;
        ZoomRefEmptyGameObject.transform.Rotate(0, 90f, 0);
    }

    private void ResetZoom()
    {
        ZoomRefEmptyGameObject.transform.localScale = new Vector3(1, 1, 1);
        ZoomRefEmptyGameObject.transform.position = new Vector3(0, 0, 0);
        userCreatedObjectParent.transform.localScale = new Vector3(1, 1, 1);
        userCreatedObjectParent.transform.position = new Vector3(0, 0, 0);
       
        controlState.zoomFactor = 1;
    }
}
