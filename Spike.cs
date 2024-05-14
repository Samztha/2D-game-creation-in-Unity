using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Cone : MonoBehaviour
{
    //Mesh
    Mesh mesh;
    MeshRenderer meshRenderer;

    //Vertices/corner triangles
    List<Vector3> vertices;
    List<int> triangles;

    public Material material;

    //Cone parametres
    public float height = 1.5f;
    public float radius = 0.5f;
    public int segments = 100;

    Vector3 pos;

    float angle = 0.0f;
    float angleAmount = 0.0f;

    void Start()
    {
        //Initialize mesh and material
        gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        //Initialize vertices/corner angles
        vertices = new List<Vector3>();
        pos = new Vector3();

        angleAmount = 2 * Mathf.PI / segments;
        angle = 0.0f;

        //High center of the cone
        pos.x = 0.0f;
        pos.y = height;
        pos.z = 0.0f;
        vertices.Add(new Vector3(pos.x, pos.y, pos.z));

        //Base center of the cone
        pos.y = 0.0f;
        vertices.Add(new Vector3(pos.x, pos.y, pos.z));

        for (int i = 0; i < segments; i++)
        {
            //Add corners around
            pos.x = radius * Mathf.Sin(angle);
            pos.z = radius * Mathf.Cos(angle);

            vertices.Add(new Vector3(pos.x, pos.y, pos.z));

            angle -= angleAmount;
        }

        //Assign corners to the mesh
        mesh.vertices = vertices.ToArray();

        //Initialize the list of triangles
        triangles = new List<int>();

        for (int i = 2; i < segments + 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }

        //Close bottom
        triangles.Add(0);
        triangles.Add(2);
        triangles.Add(segments + 1);

        //Top
        for (int i = 2; i < segments + 1; i++)
        {
            triangles.Add(1);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        //Close the top
        triangles.Add(1);
        triangles.Add(segments + 1);
        triangles.Add(2);

        mesh.triangles = triangles.ToArray();
    }
}