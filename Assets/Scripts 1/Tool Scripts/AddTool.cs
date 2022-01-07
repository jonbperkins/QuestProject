using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTool : MonoBehaviour
{
    public ControlState controlState;
    public Settings settings;

    [System.NonSerialized]
    public float scale = 0.2f;  //  Scaling for game objects.  For default value of 0.2, a 1 meter cube is .2 meters wide.  This is changable by user in the "ScaleRotate() function"
    [System.NonSerialized]
    public Vector3 cubeIndex;  // If placing cubes using Snap To Grid, this is the position to create the cube, calculated based on controller position.
    public Vector3 prevCubeIndex;

    public PreviewObject objectPreview;  // Attached to players right hand, ScriptableObject class to have this sharable with other scripts
    public GameObject defaultObject;
    private float previewYRotateDegrees = 0;
    private float previewXRotateDegrees = 0;

    public int createdObjectsIndex = 0;
    public bool snapToGridOn = false;
    //private bool snapToGridOnRisingTrig = false;
    public GameObject snapToGridStartPoint;
    public bool snapToGridHorizontal = false;

    public bool repeatModeOn = false;
    public float repeatDistance = 0.01f;

    public bool startingPointIsSet = false;  //Snap to Grid and Repeat Mode depend require a start point (initial trigger press).  This prevents objects from being drawn when exiting the menu (delete all, etc.)

    public bool StretchModeOn = false;
    public float xStretchModifier = 1;
    public float yStretchModifier = 1;
    public float zStretchModifier = 1;
    
    public SceneData userCreation;
    public GameObject userCreatedObjectParent;
  

    void Start()
    {
        userCreation.objectList.Clear();
        controlState.mode = ControlState.ControlMode.Create;
    }

    void Update()
    {
        if (controlState.mode == ControlState.ControlMode.Create)
        {
            if (objectPreview.Obj == null)
            {
                objectPreview.Obj = Instantiate(defaultObject);
            }
            objectPreview.Obj.SetActive(true);
            ShowObjectPreview();
            userCreatedObjectParent.SetActive(true);
            ScaleRotateObjectPreview();
            CreateNewObjects();  //Method includes functions to create new objects if trigger is pressed
        }

        else if (controlState.mode == ControlState.ControlMode.Menu || controlState.mode == ControlState.ControlMode.AnimateMenu)
        {
            userCreatedObjectParent.SetActive(false);

            startingPointIsSet = false;  

            if (objectPreview.Obj != null)
            {
               objectPreview.Obj.SetActive(false);
            }

        }

        else     //If not in Create or Menu mode, show the created scene but not the object preview
        {
            if (objectPreview.Obj != null)
            {
                objectPreview.Obj.SetActive(false);
            }
            userCreatedObjectParent.SetActive(true);
        }

        UpdateControlStateModes();
    }

    private void UpdateControlStateModes()
    {
        snapToGridOn = settings.snapToGridModeOn;
        repeatModeOn = settings.repeatModeOn;
        StretchModeOn = settings.stretchModeOn;

        if (StretchModeOn == false)
        {
            settings.xStretchModifier = 1;
            settings.yStretchModifier = 1;
            settings.zStretchModifier = 1;
        }

        if (snapToGridOn == false)
        {
            snapToGridStartPoint = null;
        }

        xStretchModifier = settings.xStretchModifier;
        yStretchModifier = settings.yStretchModifier;
        zStretchModifier = settings.zStretchModifier;
    }

    private void CreateNewObjects()
    {
        if (controlState.rightTriggerOnPress == true)
        {
            startingPointIsSet = true;
        }


        if  (controlState.rightTriggerOnPress == true && snapToGridOn == true && snapToGridStartPoint == null)
        {
            SetSnapToGridStartingPoint(); 
        }

        if (snapToGridOn == false && snapToGridStartPoint != null)
        {
            snapToGridStartPoint = null;
        }


        if (startingPointIsSet == true && controlState.rightTriggerPress == true && snapToGridOn == true)
        {
            objectPreview.Obj.transform.position = GetSnapToGridVector();
            //objectPreview.Obj.transform.rotation = Quaternion.identity;
            objectPreview.Obj.transform.rotation = snapToGridStartPoint.transform.rotation;

            foreach (GameObject obj in userCreation.objectList)
            {
                if (cubeIndex == obj.transform.position)
                {
                    return;
                }
            }

            PinObject();
            prevCubeIndex = cubeIndex;

            if (cubeIndex != prevCubeIndex)
            {

            }
        }


        else if (startingPointIsSet == true && controlState.rightTriggerPress == true && repeatModeOn == true)  // As long as trigger is held add new object after dist moved is > repeatDistance
        {
            if (float.IsNaN(prevCubeIndex.x)) //Was Getting NaN some times when switching between modes. 
            {
                prevCubeIndex = new Vector3(0, 0, 0);
            }
            if (Mathf.Abs((controlState.rightHandPosVector - prevCubeIndex).magnitude) > repeatDistance )  
            {
                PinObject();
                prevCubeIndex = controlState.rightHandPosVector;
            }
        }

        else if (controlState.rightTriggerOnPress == true)  //If not in snapToGrid or repeat mode, pin object in current objectPreview location.
        {
            PinObject();  
        }

    }

    private void SetSnapToGridStartingPoint()
    {
        //if (controlState.rightTriggerPress == true && snapToGridOn == true && snapToGridOnRisingTrig == false)  //Only do this once during a snapToGrid sequence.  Every time trigger is pressed find a reference point
        {
          //  snapToGridOnRisingTrig = true;

            if (snapToGridStartPoint == null)
            {
                snapToGridStartPoint = new GameObject();
            }

            snapToGridStartPoint.transform.position = objectPreview.Obj.transform.position;
            snapToGridStartPoint.transform.rotation = objectPreview.Obj.transform.rotation;

            if (snapToGridHorizontal == true)  //Set object to be pointed up (flat on horizontal grid)
            {
                Vector3 vec = objectPreview.Obj.transform.eulerAngles;
                vec.x = Mathf.Round(vec.x / 90) * 90;
                vec.y = objectPreview.Obj.transform.rotation.eulerAngles.y;
                vec.z = Mathf.Round(vec.z / 90) * 90;
               
                snapToGridStartPoint.transform.rotation = Quaternion.Euler(vec);
            }

        }
       // if (controlState.rightTriggerPress == false)
        //{
          //  snapToGridOnRisingTrig = false;
        //}
    }

    private void ScaleRotateObjectPreview()
    {
        if (controlState.rightJoystickPosition.x > .1)
        {
            previewYRotateDegrees += (Time.deltaTime * (controlState.rightJoystickPosition.x - .1f) * 180);
            if (previewYRotateDegrees > 360)
            {
                previewYRotateDegrees -= 360;
            }
        }

        if (controlState.rightJoystickPosition.x < -0.1)
        {
            previewYRotateDegrees += (Time.deltaTime * (controlState.rightJoystickPosition.x - .1f) * 180);
            if (previewYRotateDegrees < 0)
            {
                previewYRotateDegrees += 360;
            }
        }

        if (controlState.rightJoystickPosition.y > .3)
        {
            if (scale < 20)
            {
                scale *= (1 + Time.deltaTime);
                ShowObjectPreview();
            }
        }

        else if (controlState.rightJoystickPosition.y < -0.3)
        {
            if (scale > .001)
            {
                scale /= (1+Time.deltaTime);
                //yStretchModifier /= (1 + Time.deltaTime);
                ShowObjectPreview();
            }
        }

    }

        
    private void ShowObjectPreview()  //Created a preview object that appears to be attached to hand.  Position is then constantly update in Update() function to correspond to hand location. 
    {

        if (objectPreview.Obj != null)
        {
            objectPreview.Obj.transform.localScale = new Vector3(1, 1, 1);
            objectPreview.Obj.transform.localScale *= (scale * controlState.zoomFactor);
            objectPreview.Obj.transform.position = controlState.pointerPosition;
            objectPreview.Obj.transform.rotation = controlState.pointerRotation;
            objectPreview.Obj.transform.Rotate(new Vector3(previewXRotateDegrees, previewYRotateDegrees, 0));

            if (StretchModeOn == true)
            {
                float xStretchAmount = controlState.zoomFactor * 5f * scale * xStretchModifier;  //At default, zoomFactor=1, scale=.2, and stretchModifier = 1
                float yStretchAmount = controlState.zoomFactor * 5f * scale * yStretchModifier;
                float zStretchAmount = controlState.zoomFactor * 5f * scale * zStretchModifier;
                objectPreview.Obj.transform.position -= new Vector3(0, 0, -.05f);   //Shift down as many objects started slightly above y = 0
                objectPreview.Obj.transform.localScale = Vector3.Scale(objectPreview.Obj.transform.localScale, new Vector3(xStretchAmount, yStretchAmount, zStretchAmount)); //Multiply each component wise
                //float shiftYDown = controlState.zoomFactor * scale * (yStretchPct * .01f) * 0.5f * objectPreview.height;

            }
        }
        else
        {
           
        }

    }

     private Vector3 GetSnapToGridVector()
     {
        float xgridSize = objectPreview.width * scale * controlState.zoomFactor; 
        float ygridSize = objectPreview.height * scale * controlState.zoomFactor;  
        float zgridSize = objectPreview.depth * scale * controlState.zoomFactor; 

        if (StretchModeOn == true)   
        {
            xgridSize *= (xStretchModifier * scale * 5f * controlState.zoomFactor);
            ygridSize *= (yStretchModifier * scale * 5f * controlState.zoomFactor);
            zgridSize *= (zStretchModifier * scale * 5f * controlState.zoomFactor);
        }

        Vector3 distanceVector = snapToGridStartPoint.transform.InverseTransformPoint(controlState.pointerPosition);  //Get position of pointer relative to start point local position

        float xSnapToGridCoordinate = Mathf.Round(distanceVector.x / xgridSize);

        float ySnapToGridCoordinate;
        if (objectPreview.Obj.name.Contains("Plane") != true)
        {
            float ySnapToGridOffset = ygridSize * .1f; //Offset by 10% of object size. Helps to not draw 2 objects initially (intended object and one lower)
            ySnapToGridCoordinate = Mathf.Floor((distanceVector.y + ySnapToGridOffset) / ygridSize);  
        }
        else
        {
            ySnapToGridCoordinate = 0; //IF it is a plane, don't allow Y snap to grid changes as it is a flat object. 
        }

        float zSnapToGridCoordinate = Mathf.Round(distanceVector.z / zgridSize);
        cubeIndex = snapToGridStartPoint.transform.right * xSnapToGridCoordinate * xgridSize
                  + snapToGridStartPoint.transform.up * ySnapToGridCoordinate * ygridSize
                  + snapToGridStartPoint.transform.forward * zSnapToGridCoordinate * zgridSize;
        cubeIndex += snapToGridStartPoint.transform.position;
      
        return cubeIndex;
    }

    private void PinObject()
    {
        GameObject newObject = Instantiate(objectPreview.Obj, objectPreview.Obj.transform.position, objectPreview.Obj.transform.rotation, userCreatedObjectParent.transform);  //TODO: Add instantiate to UserCreation Script Add() function

        newObject.name = newObject.name.Replace("(Clone)", "");  //Get rid of text as object name is used for SAVE & LOAD

        newObject.transform.localScale /= controlState.zoomFactor;  //Corrects for fact that the parent object is scaled down

        userCreation.Add(newObject);

        createdObjectsIndex++;
    }

}
