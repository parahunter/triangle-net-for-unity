using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class TestScript : MonoBehaviour 
{
    TriangleNet.Mesh mesh;
    InputGeometry geometry;

    public float distance = 5;
    public float boxDistance = 1f;
    public float circleDistance = 0.7f;

    public Color gizmoColor;
    public float boxWidth = 1f;

	// Use this for initialization
	void Start () 
    {
        geometry = new InputGeometry();
        List<Point> points = new List<Point>();

        points.Add(new Point((double)distance, (double)distance));
        points.Add(new Point((double)distance, (double)-distance));
        points.Add(new Point((double)-distance, (double)-distance));
        points.Add(new Point((double)-distance, (double)distance));

        geometry.AddRing(points, 0);

        List<Point> hole = new List<Point>();
        hole.Add(new Point((double)boxWidth, (double)boxWidth));
        hole.Add(new Point((double)boxWidth, (double)0));
        hole.Add(new Point((double)boxWidth, (double)-boxWidth));
        hole.Add(new Point((double)-boxWidth, (double)-boxWidth));
        hole.Add(new Point((double)-boxWidth, (double)boxWidth));
        geometry.AddRingAsHole(hole, 1);

        for (float offsetX = -distance; offsetX < distance; offsetX += boxDistance)
        {
            // float offsetY = -distance;
            for (float offsetY = -distance; offsetY < distance; offsetY += boxDistance)
            {
                Vector2 offset = new Vector2(offsetX, offsetY) + Vector2.one * boxDistance * 0.5f;

                float radians = Random.RandomRange(0, 2 * Mathf.PI);
                float length = Random.RandomRange(0, circleDistance);

                Vector2 pos = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * length;
                pos += offset;

                if (Mathf.Abs(pos.x) < boxWidth && Mathf.Abs(pos.y) < boxWidth)
                    continue;

                //points.Add(new Point((double)pos.x, (double)pos.y));
                geometry.AddPoint( (double)pos.x, (double)pos.y);
            }
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
