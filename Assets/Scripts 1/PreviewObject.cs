using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New PreviewObject", menuName = "PreviewScriptableObject", order = 52)]
public class PreviewObject : MonoBehaviour
{
    private MeshFilter myMeshFilter;
    private List<Mesh> objectMeshes;
    public Vector3[] vertices;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;
    public float width, height, depth;

    private GameObject obj;
    public GameObject Obj
    {
        get
        {
            return obj;
        }
        set
        {
            if (obj != null)
            {
                Destroy(obj);
                objectMeshes.Clear();
            }
            obj = value;
            CalculateVertices();
        }
    }

    public void CalculateVertices()
    {
        objectMeshes = new List<Mesh>();
        if (obj.GetComponent<MeshFilter>() != null)
        {
            objectMeshes.Add(obj.GetComponent<MeshFilter>().mesh);
             
        }
        foreach(Transform myTransform in obj.transform)
       // if (obj.GetComponentsInChildren<Mesh>() != null)
        {
            Mesh myMesh;
            if (myTransform.GetComponent<MeshFilter>() != null)
            {
                myMesh = myTransform.GetComponent<MeshFilter>().mesh;
            }
            else if (myTransform.GetComponent<SkinnedMeshRenderer>() != null)
            {
                myMesh = myTransform.GetComponent<SkinnedMeshRenderer>().sharedMesh;
            }
            else
            {
                myMesh = null;
            }

            if (myMesh != null)
            {
                objectMeshes.Add(myMesh);
            }

        }

        minX = 0;
        maxX = 0;
        minY = 0;
        maxY = 0;
        minZ = 0;
        maxZ = 0;
       // GetComponentsInChildren<Mesh>();
        foreach (Mesh mesh in objectMeshes)
        {
            vertices = mesh.vertices;
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].x < minX)
                {
                    minX = vertices[i].x;
                }
                if (vertices[i].x > maxX)
                {
                    maxX = vertices[i].x;
                }
                if (vertices[i].y < minY)
                {
                    minY = vertices[i].y;
                }
                if (vertices[i].y > maxY)
                {
                    maxY = vertices[i].y;
                }
                if (vertices[i].z < minZ)
                {
                    minZ = vertices[i].z;
                }
                if (vertices[i].z > maxZ)
                {
                    maxZ = vertices[i].z;
                }
            }
        }

        width = maxX - minX;
        height = maxY - minY;
        depth = maxZ - minZ;
    }
        /*
        if (obj.GetComponent<MeshFilter>() != null)
        {
            myMeshFilter = obj.GetComponent<MeshFilter>();
            myMesh = myMeshFilter.mesh;
            vertices = myMesh.vertices;
            minX = 0;
            maxX = 0;
            minY = 0;
            maxY = 0;
            minZ = 0;
            maxZ = 0;

            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[i].x < minX)
                {
                    minX = vertices[i].x;
                }
                if (vertices[i].x > maxX)
                {
                    maxX = vertices[i].x;
                }
                if (vertices[i].y < minY)
                {
                    minY = vertices[i].y;
                }
                if (vertices[i].y > maxY)
                {
                    maxY = vertices[i].y;
                }
                if (vertices[i].z < minZ)
                {
                    minZ = vertices[i].z;
                }
                if (vertices[i].z > maxZ)
                {
                    maxZ = vertices[i].z;
                }
            }
            
        }
        else  //If it doesn't have a Meshfilter it is a prefab using multiple components.  Put in estimated min/max values.
        {
            minX = -.5f;
            maxX = .5f;
            minY = -.25f;
            maxY = .75f;
            minZ = -.5f;
            maxZ = .5f;
        }

    */
}
