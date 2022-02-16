using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        int i = 0;
        Vector3 pos = transform.TransformPoint(vertices[i]);

        
        
            Debug.Log("Vertices: " + vertices.Length + "Triangulos: " + triangles.Length);
        for (int m = 0; m < vertices.Length; m++)
        {
            Debug.Log("vertice" + m + "x: " + vertices[m].x + "y: " + vertices[m].y + "z: " + vertices[m].z);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = new Vector3[mesh.vertexCount];
        
        int i = 0;
        Vector3 pos = new Vector3 (0.0f, 0.0f, 0.0f);
        vertices[i] = transform.InverseTransformPoint(pos);
       
        mesh.vertices = vertices; 
    
    }
}
