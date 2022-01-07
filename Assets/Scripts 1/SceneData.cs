using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;



public class SceneData:MonoBehaviour
{
    public List<GameObject> objectList;   //Reference to in-scene instanteated object. 
    public List<ObjectSaveData> objectSaveData; //Includes all of the saved info about objects.  Defined at bottom of script. 

    public GameObject userCreationParentObject;
    public ControlState controlState;

    public string fileName = "SaveData.txt";
    private string SavedTextsCompleteFilePath;

    public Menu menu;

    private void Start()
    {

#if PLATFORM_ANDROID && !UNITY_EDITOR
        SavedTextsCompleteFilePath = Application.persistentDataPath;

        // just for debugging and playing in the editor
#elif UNITY_EDITOR
        SavedTextsCompleteFilePath = "Assets/Resources";
#endif
        // set the base file path, then add the directory if it's not there yet
        SavedTextsCompleteFilePath = MakeFolder(SavedTextsCompleteFilePath, "MyGameSaveFolder");
    }
    private string MakeFolder(string path, string savedTextsFolder)
    {
        string saveDirectory = path + savedTextsFolder;
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log("directory created! at: " + path);
        }
        return saveDirectory;
    }

    public void Add(GameObject obj)  //Doesn't instatiate as the AddTool applies the initial position and rotation. TODO - Look at adding overloaded Add function with these as parameters. 
    {
       int index = objectList.Count;
       objectList.Insert(index, obj);
       UpdateSaveData(index, obj);
    }

    public void UpdateSaveData(int index, GameObject obj)
    {
        int saveIndex = objectList.IndexOf(obj);
        ObjectSaveData saveData = new ObjectSaveData();
        saveData.objSaveName = obj.name;
        saveData.startPos = obj.transform.localPosition;
        saveData.startRot = obj.transform.localRotation;
        saveData.startScale = obj.transform.localScale;
        saveData.objTag = obj.tag;
        objectSaveData.Insert(index, saveData);
    }
    

    public void Remove(GameObject obj)
    {
        int index = objectList.IndexOf(obj);
        objectList.RemoveAt(index);
        objectSaveData.RemoveAt(index);
        Destroy(obj);
    }

    public void SaveScene()
    {
        objectList.Clear();
        objectSaveData.Clear();
        foreach(Transform objTransform in userCreationParentObject.transform)
        {
            Add(objTransform.gameObject);
        }
        string json = JsonUtility.ToJson(this);

        File.WriteAllText(Path.Combine(SavedTextsCompleteFilePath, fileName), json, System.Text.Encoding.ASCII);
       
        //startSave = false;
        //objectList.Clear();
    }

    public void LoadScene()
    {
        //objectList.Clear();
        for (int i = objectList.Count; i > 0; i-- )
        {
            Remove(objectList[i - 1]);
        }

#if PLATFORM_ANDROID && !UNITY_EDITOR
        string textAsset = new TextAsset(System.IO.File.ReadAllText(Path.Combine(SavedTextsCompleteFilePath, fileName))).ToString();
#elif UNITY_EDITOR
        string textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(Path.Combine(SavedTextsCompleteFilePath, fileName)).ToString();
#endif

        JsonUtility.FromJsonOverwrite(textAsset, this);

        foreach (ObjectSaveData objLoad in objectSaveData)  
        {
            foreach (GameObject menuObj in menu.MenuList)  //TODO:  Make finding object more efficient
            {
                foreach (Transform menuGameObj in menuObj.transform)
                {
                    if (menuGameObj.name == objLoad.objSaveName)
                    {
                        GameObject loadedObject = Instantiate(menuGameObj.gameObject, userCreationParentObject.transform);
                        loadedObject.name = loadedObject.name.Replace("(Clone)", "");  //Get rid of text as object name is used for SAVE & LOAD
                        loadedObject.gameObject.transform.localPosition = objLoad.startPos;
                        loadedObject.gameObject.transform.localRotation = objLoad.startRot;
                        loadedObject.gameObject.transform.localScale = objLoad.startScale;
                        loadedObject.gameObject.tag = objLoad.objTag;
                        int index = objectSaveData.IndexOf(objLoad);
                        objectList[index] = loadedObject;
                    }


                }
            }

        }

    }

   // private void WriteToFile(string fileName, string json)
   // {
       // string path = GetFilePath(fileName);
      //  File.WriteAllText(path, json);
      //  controlState.mode = ControlState.ControlMode.Create;
    //}



    ///private string GetFilePath(string fileName)
    ///{
   ///     return (Application.persistentDataPath + "./Data/" + fileName);
    ///}

}

[System.Serializable]
public struct ActionSaveData
{
    public int actionNumber;

    public Vector3 rotateDirection;
    public float rotateCyclesPerSecond;
    public float targetRotateCycles;

    public Vector3 moveDirection;
    public float moveDistancel;
    public float moveCyclesPerSecond;
    public float targetMoveCycles;

    public Vector3 scaleDirection;
    public float scaleDistancel;
    public float scaleCyclesPerSecond;
    public float targetScaleCycles;

    public Material recolorMaterial;
    public float reColorIntensity;
    public float recolorCyclesPerSec;
    public float targetRecolorCycles;
}

[System.Serializable]
public struct ObjectSaveData
{
    public string objSaveName;
    public int objTypeIndex;
    public Vector3 startPos;
    public Quaternion startRot;
    public Vector3 startScale;
    public string objTag;
    public List<int> triggers;
    public List<int> actions;
}
