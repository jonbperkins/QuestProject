using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

    public List<GameObject> MenuList;  //Different Menu pages are saved as prefabs and loaded in this list
    public GameObject settingsMenu;
    public PreviewObject previewObject;

    public ControlState controlState;  //Has references to mode (Create, Menu, etc.) and all UI from controllers
    public Settings settings;

    public SceneData sceneData;
    public SelectCollider selectCollider;
    private int menuNumber = 0;  
    private bool menuIsOn = false;

    public Vector3 lastMenuPosition;
    public Quaternion lastMenuRotation;

    private GameObject activeMenu;
    public GameObject activeSettingsMenu;

    public Vector3 menuOffset;
    
    public float selectionIgnoreTimer = 0;

    public MusicPlayer musicPlayer;

    

    void Start()
    {
        activeSettingsMenu = settingsMenu;
    }

    void Update()
    {
        CheckForTogglingMenu();  //TODO:  Update to make this event based instead. 

 
        if (controlState.mode == ControlState.ControlMode.Menu)
        {
            CheckForSelectedObjects();
        }


    }

    private void CheckForTogglingMenu()
    {
        if (controlState.rightButtonAOnPress == true)
        {
            if (menuIsOn == false)  //If menu not already active then open first menu
            {
                controlState.mode = ControlState.ControlMode.Menu;
                selectCollider.SelectColliderActive = true;
                selectCollider.flashSelectedObjects = false;
                ShowMenu();
            }

            else  //If menu already active then close menu and go back to create mode
            {
                controlState.mode = ControlState.ControlMode.Create;
                HideMenu();
            }
        }

        if (menuIsOn == true && (controlState.mode != ControlState.ControlMode.Menu))
        {
            HideMenu();
        }
    }


    /*
    private void updateSettingIndicators()
    {
        if (controlState.snapToGridModeOn == true)
        {
            snapToGridActiveIndicator.SetActive(true);
        }
        else
        {
            snapToGridActiveIndicator.SetActive(false);
        }

        if (controlState.repeatModeOn == true)
        {
            repeatToolActiveIndicator.SetActive(true);
        }
        else
        {
            repeatToolActiveIndicator.SetActive(false);
        }

        if (controlState.stretchModeOn == true)
        {
            stretchModeActiveIndicator.SetActive(true);
        }
        else
        {
            stretchModeActiveIndicator.SetActive(false);
        }
    }
    */
    public void HideMenu()
    {
        GameObject.Destroy(activeMenu);
        activeSettingsMenu.SetActive(false);
        menuIsOn = false;

        if (controlState.mode == ControlState.ControlMode.Menu)  //If it was in Menu control mode and the menu is closed switch back to create mode, otherwise keep current mode (Animator, etc..)
        {
            controlState.mode = ControlState.ControlMode.Create;
        }
    }
    public void ShowMenu()
    {
        if (activeMenu != null)  //To switch between different menu screens, destory the existing menu screen before initiating new screen.
        {
            Destroy(activeMenu);
        }

        activeMenu = Instantiate(MenuList[menuNumber], controlState.rightHandPosVector, controlState.rightHandRotationVector);
        activeMenu.transform.rotation = Quaternion.Euler(0, activeMenu.transform.rotation.eulerAngles.y, 0);  //straighten out Menu
        //activeMenu.transform.position += new Vector3 (0f, .25f, 0f);
        activeMenu.transform.position += activeMenu.transform.forward * 0.1f;
        activeMenu.transform.position += activeMenu.transform.right * -0.2f;
        activeMenu.transform.position = new Vector3(activeMenu.transform.position.x, controlState.HMDPosVector.y - .2f, activeMenu.transform.position.z);


        lastMenuPosition = activeMenu.transform.position;
        lastMenuRotation = activeMenu.transform.rotation;


        if (activeSettingsMenu == null)
        {
            //activeSettingsMenu = Instantiate(settingsMenu, activeMenu.transform.position, activeMenu.transform.rotation);
            Debug.Log("No settings menu located");
        }
        else
        {
            activeSettingsMenu.SetActive(true);
            activeSettingsMenu.transform.position = activeMenu.transform.position;
            activeSettingsMenu.transform.rotation = activeMenu.transform.rotation;
        
        }

        activeSettingsMenu.transform.position += (activeSettingsMenu.transform.up * .2f + activeSettingsMenu.transform.forward * -.42f + activeSettingsMenu.transform.right * 0.65f);  //First have settings menu located in same location, then shift to right side of active menu.
        activeSettingsMenu.transform.Rotate(new Vector3(0, 60f, 0));
        menuIsOn = true;
    }

    public void NextMenu()
    {
        if (menuNumber < MenuList.Count - 1)
        {
            menuNumber++;
        }
        else
        {
            menuNumber = 0;
        }
        Vector3 previousMenuPosition = activeMenu.transform.position;
        Quaternion previousMenuRotation = activeMenu.transform.rotation;
        if (activeMenu != null)
        {
            Destroy(activeMenu);
        }

        activeMenu = Instantiate(MenuList[menuNumber], previousMenuPosition, previousMenuRotation);

    }

    public void PreviousMenu()
    {
        if (menuNumber > 0)
        {
            menuNumber--;

        }
        else
        {
            menuNumber = MenuList.Count - 1;
        }

        Vector3 previousMenuPosition = activeMenu.transform.position;
        Quaternion previousMenuRotation = activeMenu.transform.rotation;

        if (activeMenu != null)
        {
            Destroy(activeMenu);
        }  
        activeMenu = Instantiate(MenuList[menuNumber], previousMenuPosition, previousMenuRotation);

    }

    private void CheckForSelectedObjects()
    {
        int selectColliderObjectCount = selectCollider.GetListSize();
        if (selectionIgnoreTimer > 0)
        {
            for (int i = 0; i < selectColliderObjectCount; i++)
            {
                selectCollider.Pop(); //Remove any objects that are selected furing the selectionIgnoreTimer (Makes it so that a new menu doesn't automaticllly trigger to the next/previous selection)
            }
            selectionIgnoreTimer -= Time.deltaTime;
            return;
        }
        else
        {
            for (int i = 0; i < selectColliderObjectCount; i++)
            {
                ProcessSelectedObject(selectCollider.Pop());  //objList[0] is different each time, as the Pop() function removes the previous object. 
            }
        }

    }

    void ProcessSelectedObject(GameObject obj)
    {


        if (obj.CompareTag("MenuObject")  == true)  // The tag "MenuObject" is used for any of the objects the player can draw/create with
        {
            previewObject.Obj = Instantiate(obj);
            previewObject.Obj.tag = "SceneObject";
            controlState.mode = ControlState.ControlMode.Create;
        }

        else if (obj.name == "NextArrow")
        {
            NextMenu();
            selectionIgnoreTimer = .2f;
        }

        else if (obj.name == "PrevArrow")
        {
            PreviousMenu();
            selectionIgnoreTimer = .2f;
        }

        else if (obj.name == "TrashCan")   //Delete all of the created scene and start over
        {
            for (int i = 0; i < sceneData.gameObject.transform.childCount; i++)
            {
                sceneData.GetComponent<SceneData>().Remove(sceneData.gameObject.transform.GetChild(i).gameObject);
            }

            controlState.mode = ControlState.ControlMode.Create;
            HideMenu();
        }

        else if (obj.name == "SelectAnimationMenu")
        {
            controlState.mode = ControlState.ControlMode.AnimateMenu;
            HideMenu();
            //myAnimationMenu.ShowMenu();

        }



        else if (obj.name == "SelectTool") 
        {
            controlState.mode = ControlState.ControlMode.SelectTool;
            HideMenu();

        }

        else if (obj.name == "SelectDeleteTool")
        {
            controlState.mode = ControlState.ControlMode.DeleteTool;
            HideMenu();
        }

        else if (obj.name == "AnimateLockTool")
        {
            controlState.mode = ControlState.ControlMode.AnimateLockTool;
            HideMenu();
        }

        else if (obj.name == "Save")
        {
            sceneData.SaveScene();
            controlState.mode = ControlState.ControlMode.Create;
            HideMenu();
        }

        else if (obj.name == "Load")
        {
            sceneData.LoadScene();
            controlState.mode = ControlState.ControlMode.Create;
            HideMenu();
        }

        else if (obj.name == "RecordGame")
        {
            musicPlayer.GetComponent<AudioSource>().clip = obj.GetComponent<AudioSource>().clip;
            controlState.mode = ControlState.ControlMode.Record;
            
            HideMenu();
        }
        else if (obj.name == "LoadGame")
        {
            controlState.mode = ControlState.ControlMode.Player;
            HideMenu();
        }

        /*

        else if (obj.name == "SelectSnapToGridTool")
        {

            if (controlState.snapToGridModeOn == false)
            {
                //controlState.snapToGridModeOn = true;
                //controlState.repeatModeOn = false;
                //settings.SnapToGridModeTurnOn();
                obj.GetComponent<TogglePB>().Toggle();
            }
            else
            {
                controlState.snapToGridModeOn = false;
            }
        }

        else if (obj.name == "SelectStretchTool")
        {
            if (controlState.stretchModeOn == false)
            {
                controlState.stretchModeOn = true;
            }
            else
            {
                controlState.stretchModeOn = false;
            }
        }

        else if (obj.name == "SelectRepeatTool")
        {
            if (controlState.repeatModeOn == false)
            {
                controlState.repeatModeOn = true;
                controlState.snapToGridModeOn = false;
            }
            else
            {
                controlState.repeatModeOn = false;
            }
        }
        */
        else if (obj.name.Contains("TogglePB"))
        {
            obj.GetComponent<TogglePB>().Toggle();
        }

        else if (obj.name == "AdjustingPBIncrease")
        {
            obj.GetComponentInParent<AdjustingPB>().Increase();
        }
        else if (obj.name == "AdjustingPBDecrease")
        {
            obj.GetComponentInParent<AdjustingPB>().Decrease();
        }
    }

}
