using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCollider : MonoBehaviour    //Creates a List of GameObjects that are selected (by colliding with the select sphere).  Each tool function uses the Pop() function to decide what to do with selected objects.  
{

    public bool SelectColliderActive;

    public List<GameObject> objList = new List<GameObject>();

    public ControlState controlState;
    public ControlState.ControlMode prevControlMode; //If control mode changes, reset all functions


    private float SelectColliderFlashTimer = .5f;
    private MeshRenderer myMesh;
    private SphereCollider myCollider;

    public bool useTrigger = true;
    private float scaleMultiplier = 1;

    public bool flashSelectedObjects;

    public GameObject tempObject;


    void Start()
    {

        myMesh = GetComponent<MeshRenderer>();
        myCollider = GetComponent<SphereCollider>();

    }

    void Update()
    {
        CheckIfSelectColliderOff();
        if (SelectColliderActive == true)
        {
            ShowSelectCollider();
            //  myCollider.enabled = true;

        }
        else
        {
            myMesh.enabled = false;
            //   myCollider.enabled = false;
        }

        if (SelectColliderActive == true && useTrigger == true && controlState.rightTriggerPress == false)  //If useTrigger is ON, only enable sphere collider when trigger is pressed.
        {
            myCollider.enabled = false;
        }

        else
        {
            myCollider.enabled = true;
        }


        CheckForNewSelectMode();



    }

    private void CheckForNewSelectMode()
    {
        if (controlState.mode != prevControlMode)
        {
            foreach (GameObject obj in objList)
            {
                if (obj != null)
                {
                    obj.SetActive(true);  //Set all to active.  Necessary as selected objects might have been inactive in order to flash selected objects.  
                }

            }
            scaleMultiplier = 1;
            objList.Clear();
        }
        prevControlMode = controlState.mode;
    }

    private void CheckIfSelectColliderOff() //Once select tool is complete, make sure all objects are active (Objects are turned on/off to indicate which are in current selection)
    {
        if (controlState.mode == ControlState.ControlMode.AnimateMenu ||
            controlState.mode == ControlState.ControlMode.AnimateTool ||
            controlState.mode == ControlState.ControlMode.Menu ||
            controlState.mode == ControlState.ControlMode.DeleteTool ||
            controlState.mode == ControlState.ControlMode.SelectTool ||
            controlState.mode == ControlState.ControlMode.AnimationGroupSelect ||
            controlState.mode == ControlState.ControlMode.AnimateLockTool)  //For any ControlState mode besides these, the select tool should be inactive 
        {
            SelectColliderActive = true;
        }

        else
        {
            SelectColliderActive = false;
        }
    }

    private void ShowSelectCollider()
    {
        this.transform.position = controlState.pointerPosition;    //This position is set in "GetControllerInput" and will position the tool sphere just in front of right controller
        this.transform.localScale = new Vector3(.02f, .02f, .02f) * scaleMultiplier;
        SelectColliderFlashTimer -= Time.deltaTime;
        if (SelectColliderFlashTimer < .45f)
        {
            myMesh.enabled = true; //Make selector tool sphere flash when adding animations to objects
        }
        if (SelectColliderFlashTimer < 0)
        {
            myMesh.enabled = false;
            SelectColliderFlashTimer = 0.5f;
        }
    }

    public void ChangeSphereSize(float multiplier)  //Any script that uses SelectCollider can send in a size multiplier for the sphere, will be reset when control mode changes. 
    {
        scaleMultiplier = multiplier;
    }

    public int GetListSize()
    {
        return objList.Count;
    }


    public void Push(GameObject obj)
    {
        objList.Add(obj);
    }

    public GameObject Pop()  //Class that uses Select Tool will use Pop() to see what objects have been selected. 
    {
        if (objList.Count > 0)
        {
            if (objList[0] == null)  //Make sure it's returning a valid object;
            {
                objList.RemoveAt(0);
                return null;
            }
            else
            {
                GameObject tempObj = objList[0];
                tempObj.SetActive(true);   //Ensure the returend object is active, as it could be non-Active if flashing selected objects.  
                objList.RemoveAt(0);
                return tempObj;
            }

        }
        else
        {
            return null;
        }
    }


    void OnTriggerEnter(Collider colliderInfo)
    {
        tempObject = colliderInfo.gameObject;
        Push(tempObject);
        return;
    }
}