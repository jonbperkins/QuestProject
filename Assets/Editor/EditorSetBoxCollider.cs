
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class EditorSetBoxCollider
{

    private static List<Vector3> vertices;
    private static Vector3 center;
    private static Vector3 scale;
    private static BoxCollider collider;

[MenuItem("Tools/Add Box Collider Matching Mesh")]
private static void AddBoxColliderMeshDimension()
    {
        GameObject[] selections = Selection.gameObjects;
        foreach (GameObject obj in selections)
        {
            vertices = new List<Vector3>();
            if (obj.GetComponent<BoxCollider>() == null) collider = obj.AddComponent<BoxCollider>();
            else collider = obj.GetComponent<BoxCollider>();
            GetMeshVertices(obj.transform);
            CalculateCenterAndScale(obj.transform);
            Debug.Log("Added Gameobject " + obj.gameObject.name);
        }
    }

private static void GetMeshVertices(Transform parent)
    {  
        FindChildrenRecursive(parent);
    }

static void FindChildrenRecursive(Transform parent)
    {
        if (parent.GetComponent<MeshFilter>() != null)
        {
            var verts = parent.GetComponent<MeshFilter>().sharedMesh.vertices;
            for (int i = 0; i < verts.Length; i++)
            {
                Vector3 newVertice = parent.transform.TransformPoint(verts[i]);
                vertices.Add(newVertice);

                //Add dots to vertices to troubleshoot
              //  GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Cube);
               // dot.transform.localScale = new Vector3(.01f, .01f, .01f);
//              dot.transform.SetParent(parent);
            }
        }

            foreach (Transform child in parent)
            {
               FindChildrenRecursive(child);
            }

        Debug.Log("Vertice Length:  " + vertices.Count);
    }


    private static void CalculateCenterAndScale(Transform obj) 
    {

        float minX = 10000f;  
        float maxX = -10000f;
        float minY = 10000f;
        float maxY = -10000f;
        float minZ = 10000f;
        float maxZ = -10000f;

        foreach (Vector3 vert in vertices)
        {
            Vector3 worldVertPos = vert;// obj.TransformPoint(vert);// - obj.transform.position;
            if (worldVertPos.x > maxX) maxX = worldVertPos.x;
            if (worldVertPos.y > maxY) maxY = worldVertPos.y;
            if (worldVertPos.z > maxZ) maxZ = worldVertPos.z;

            if (worldVertPos.x < minX) minX = worldVertPos.x;
            if (worldVertPos.y < minY) minY = worldVertPos.y;
            if (worldVertPos.z < minZ) minZ = worldVertPos.z;
        }
        Debug.Log("Mins: " + minX + " " + minY + " " + minZ);
        Debug.Log("maxs: " + maxX + " " + maxY + " " + maxZ);

        center = new Vector3((maxX + minX)/2, (maxY + minY)/2, (maxZ + minZ)/2);
        collider.center = obj.transform.InverseTransformPoint(center);
        collider.size = new Vector3((maxX - minX) / obj.transform.localScale.x, (maxY - minY) / obj.transform.localScale.y , (maxZ - minZ) / obj.transform.localScale.z);
        //collider.size = new Vector3((maxX - minX) / obj.transform.localScale.x / obj.transform.localScale.x, (maxY - minY) / obj.transform.localScale.y / obj.transform.localScale.y, (maxZ - minZ) / obj.transform.localScale.z / obj.transform.localScale.z);


    }


}
