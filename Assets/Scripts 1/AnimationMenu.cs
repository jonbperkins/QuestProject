using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimationMenu : MonoBehaviour
{

    public ControlState controlState;  //Has references to mode (Create, Menu, etc.) and all UI from controllers

    public GameObject animationMenuPrefab;
    public GameObject activeAnimationMenu;
    public GameObject menuButton;

    public AnimateTool animateTool;

    public bool menuIsOn = false;
    public Menu mainMenu;  //Initially used as a way to pass it a menu starting position/rotation

    public SelectCollider selectCollider;  //Contains list of gameobjects that are selected (collide w/ selector sphere)

    public List<animationMenuPage> animationMenuPages;

    public float menuXSpacing = .02f;
    public float menuYSpacing = .04f;
    public float topMargin = .2f;
    public float minSideMargin = .1f;

    public float selectionIgnoreTimer = 0;

    int menuNum = 0;

    void Start()
    {
       
    }

    void Update()
    {
        CheckMenuToggling();
        if (controlState.mode == ControlState.ControlMode.AnimateMenu)
        {
            CheckForSelectedObjects();
        }

    }

    private void CheckMenuToggling()
    {
        if (controlState.mode == ControlState.ControlMode.AnimateMenu && menuIsOn == false)
        {
            ShowMenu();
        }

        if (menuIsOn == true && (controlState.mode != ControlState.ControlMode.AnimateMenu))
        {
            HideMenu();
        }
    }

    public void ShowMenu()
    {

        activeAnimationMenu = Instantiate(animationMenuPrefab, mainMenu.lastMenuPosition, mainMenu.lastMenuRotation);
        float menuWidth = activeAnimationMenu.transform.localScale.x;
        float menuHeight = activeAnimationMenu.transform.localScale.y;
        float menuBoxWidth = menuButton.transform.localScale.x;
        float menuBoxHeight = menuButton.transform.localScale.y;
        int gridWidth = (int)Mathf.Floor((menuWidth - minSideMargin*2) / (menuBoxWidth + menuXSpacing));
        int gridHeight = (int)Mathf.Floor((menuHeight - topMargin) / (menuBoxHeight + menuYSpacing));
        float finalSideMargin = (activeAnimationMenu.transform.localScale.x - gridWidth*menuButton.transform.localScale.x - menuXSpacing * (gridWidth - 1)) * 0.5f;

        Vector3 firstBlockLocation = new Vector3 (menuWidth * -0.5f + finalSideMargin + menuBoxWidth * 0.5f
                                     ,menuHeight * 0.5f - topMargin - menuBoxHeight * 0.5f
                                     ,menuButton.transform.localPosition.z);
        float colSpacing = menuBoxWidth + menuXSpacing;
        float rowSpacing = menuBoxHeight + menuYSpacing;
        activeAnimationMenu.GetComponentInChildren<TextMeshPro>().text = animationMenuPages[menuNum].name;

        for (int i = 0; i < animationMenuPages[menuNum].animationSegments.Count; i++)
        {
            GameObject newMenuBox = Instantiate(menuButton);
            newMenuBox.transform.SetParent(activeAnimationMenu.transform);
            int rowOffset = 0;
            int colOffset = i;
            while (colOffset >= gridWidth)
            {
                rowOffset++;
                colOffset -= gridWidth;
            }

            newMenuBox.transform.localPosition = firstBlockLocation + new Vector3 (colOffset * colSpacing, - rowOffset * rowSpacing, 0);
            newMenuBox.transform.localRotation = Quaternion.identity;
            newMenuBox.SetActive(true);
            
            newMenuBox.name = ("MenuBox " + i);
            newMenuBox.GetComponentInChildren<TextMeshPro>().text = animationMenuPages[menuNum].animationSegments[i].name;


        }

        menuIsOn = true;
    }

    public void HideMenu()
    {
        GameObject.Destroy(activeAnimationMenu);
        menuIsOn = false;

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
        if (obj.name.Contains("MenuBox "))
        {
            int selectNumber;
            string numberFromMenuObjectName = obj.name.Replace("MenuBox ", "");
            int.TryParse(numberFromMenuObjectName, out selectNumber);
            animateTool.activeAnimationSegment = animationMenuPages[menuNum].animationSegments[selectNumber];
            controlState.mode = ControlState.ControlMode.AnimateTool;
            HideMenu();
        }

        if (obj.name == "NextMenuArrow")
        {
            if (menuNum < animationMenuPages.Count - 1)
            {
                menuNum++;
            }
            else
            {
                menuNum = 0;
            }
            HideMenu();
            ShowMenu();
            selectionIgnoreTimer = .2f;
        }

        else if (obj.name == "PrevMenuArrow")
        {
            if (menuNum > 0)
            {
                menuNum--;
            }
            else
            {
                menuNum = animationMenuPages.Count -1;
            }
            HideMenu();
            ShowMenu();
            selectionIgnoreTimer = .2f;
        }


        if (obj.name == "BackToMainMenu")
        {
            HideMenu();
            controlState.mode = ControlState.ControlMode.Menu;
        }


    }


}
[System.Serializable]
public struct animationMenuPage
{
    public string name;
    public List<AnimationSegment> animationSegments;
    //Can add other menu-specific toggles, selections, sliders, etc.
}
