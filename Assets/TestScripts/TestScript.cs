using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class TestScript : MonoBehaviour 
{
    TriangleNet.Mesh mesh;
    InputGeometry geometry;

    public int pointCount = 50;
    public float distance = 5;
    public Color gizmoColor;

	// Use this for initialization
	void Start () 
    {
        geometry = new InputGeometry();

        for (int i = 0; i < pointCount; i++ )
        {
            

            geometry.AddPoint((double)Random.RandomRange(-distance, distance), (double)Random.RandomRange(-distance, distance));
        }

        mesh = new TriangleNet.Mesh();
        mesh.Triangulate(geometry);
	    

        
	}
	
    void OnDrawGizmos()
    {
        if (mesh == null)
            return;

        Gizmos.color = gizmoColor;

        foreach(KeyValuePair<int, TriangleNet.Data.Triangle> pair in mesh.triangles)
        {
            TriangleNet.Data.Triangle triangle = pair.Value;

            TriangleNet.Data.Vertex vertex0 = triangle.GetVertex(0);
            TriangleNet.Data.Vertex vertex1 = triangle.GetVertex(1);
            TriangleNet.Data.Vertex vertex2 = triangle.GetVertex(2);

            Vector2 p0 = new Vector2((float) vertex0.x, (float) vertex0.y);
            Vector2 p1 = new Vector2((float)vertex1.x, (float)vertex1.y);
            Vector2 p2 = new Vector2((float)vertex2.x, (float)vertex2.y);

            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p1, p2);
            Gizmos.DrawLine(p2, p0);
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
