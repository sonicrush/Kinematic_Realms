using System;
using System.Collections.Generic;
using UnityEngine;

public class vectorArrowObject : MonoBehaviour
{
    public float stemHeight;
    public float stemLength;
    public float tipLength;
    public float tipHeight;
    public float stemLengthBonus;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateArrow();
    }
    void GenerateArrow()
    {
        //setup
        vertices = new Vector3[9];
        triangles = new int[15];

        //stem setup
        Vector3 stemOrigin = Vector3.zero;
        //Stem points
        //bottomLeftStem = 0;
        //topLeftStem = 1;
        //topRightStem = 2;
        //bottomRightStem = 3;

        vertices[0] = (stemOrigin + (stemHeight / 2f * Vector3.down));
        vertices[1] = (vertices[0] + (stemHeight * Vector3.up));
        vertices[2] = (vertices[1] + ((stemLength + stemLengthBonus) * Vector3.right));
        vertices[3] = (vertices[2] + (stemHeight * Vector3.down));

        //Stem triangles
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;
        //tip setup
        Vector3 tipOrigin = (stemLength + stemLengthBonus) * Vector3.right;

        //tip points
        //topMiddleTip = 4;
        //bottomMiddleTip = 5;
        //centerTip = 6;
        //topLeftTip = 7;
        //bottomRightTip = 8;

        float tipHalfHeight = tipHeight / 2f;
        vertices[4] = (tipOrigin + (tipHalfHeight * Vector3.up));
        vertices[5] = (vertices[4] + (tipHeight * Vector3.down));
        vertices[6] = (tipOrigin + (tipLength * Vector3.right));
        vertices[7] = ((vertices[4] + ((tipLength / 2f) * Vector3.left)) + (tipHalfHeight * 0.33f * Vector3.up));
        vertices[8] = ((vertices[5] + ((tipLength / 2f) * Vector3.left)) + (tipHalfHeight * 0.33f * Vector3.down));
 
        //vertices[9] = vertices[2] + (0.1f * Vecto3.left)
        //vertices[10] = vertices[3] + (0.1f * Vecto3.left)
        //To move the outside tips to end a little to the left of their slope, would replace within the second and third triangle.

        //tip triangle
        triangles[6] = 4;
        triangles[7] = 6;
        triangles[8] = 5;

        triangles[9] = 7;
        triangles[10] = 4;
        triangles[11] = 2;

        triangles[12] = 8;
        triangles[13] = 3;
        triangles[14] = 5;

        //assign lists to mesh.
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
