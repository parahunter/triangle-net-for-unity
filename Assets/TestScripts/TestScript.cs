using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class TestScript : MonoBehaviour 
{
    TriangleNet.Mesh meshRepresentation;
    InputGeometry geometry;

    public float distance = 5;
    public float verticalDistance = 2;
    public float boxDistance = 1f;
    public float circleDistance = 0.7f;

    public UPolygon[] holes;

    public Color gizmoColor;
    public float boxWidth = 1f;
    public float zOffset = 0.5f;

    Mesh mesh;

	// Use this for initialization
	void Start () 
    {
        geometry = new InputGeometry();
        List<Point> points = new List<Point>();

        points.Add(new Point((float)distance, (float)verticalDistance));
        points.Add(new Point((float)distance, (float)-verticalDistance));
        points.Add(new Point((float)-distance, (float)-verticalDistance));
        points.Add(new Point((float)-distance, (float)verticalDistance));

        geometry.AddRing(points, 0);
        int counter = 0;
        {
            UPolygon hole = holes[0];
            List<Point> holePoints = new List<Point>(hole.points.Length);

            foreach (Vector2 coordinates in hole.points)
                holePoints.Add(new Point(coordinates));

            geometry.AddRingAsHole(holePoints, counter);
            counter++;
        }


        {
            UPolygon hole = holes[1];
            List<Point> holePoints = new List<Point>(hole.points.Length);

            foreach (Vector2 coordinates in hole.points)
                holePoints.Add(new Point(coordinates));

            geometry.AddRing(holePoints);
            counter++;
        }

        {
            UPolygon hole = holes[2];
            List<Point> holePoints = new List<Point>(hole.points.Length);

            foreach (Vector2 coordinates in hole.points)
                holePoints.Add(new Point(coordinates));

            geometry.AddRing(holePoints);
            counter++;
        }


        {
            UPolygon hole = holes[3];
            List<Point> holePoints = new List<Point>(hole.points.Length);

            foreach (Vector2 coordinates in hole.points)
                holePoints.Add(new Point(coordinates));

            geometry.AddRing(holePoints);
            counter++;
        }

        //List<Point> hole = new List<Point>();
        //hole.Add(new Point((float)boxWidth, (float)boxWidth));
        //hole.Add(new Point((float)boxWidth, (float)0));
        //hole.Add(new Point((float)boxWidth, (float)-boxWidth));
        //hole.Add(new Point((float)-boxWidth, (float)-boxWidth));
        //hole.Add(new Point((float)-boxWidth, (float)boxWidth));
        //geometry.AddRingAsHole(hole, 1);

        for (float offsetX = -distance; offsetX < distance; offsetX += boxDistance)
        {
            // float offsetY = -distance;
            for (float offsetY = -verticalDistance; offsetY < verticalDistance; offsetY += boxDistance)
            {
                Vector2 offset = new Vector2(offsetX, offsetY) + Vector2.one * boxDistance * 0.5f;

                float radians = Random.RandomRange(0, 2 * Mathf.PI);
                float length = Random.RandomRange(0, circleDistance);

                Vector2 pos = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * length;
                pos += offset;

                bool inHole = holes[0].PointInPolygon(pos);

                bool stillAlloved = false;
                for (int i = 1; i < holes.Length; i++ )
                {
                    if (holes[i].PointInPolygon(pos))
                        stillAlloved = true;
                }

                if (!inHole || stillAlloved)
                    geometry.AddPoint((float)pos.x, (float)pos.y, 0);
            }
        }

        meshRepresentation = new TriangleNet.Mesh();
        meshRepresentation.Triangulate(geometry);

        Dictionary<int, float> zOffsets = new Dictionary<int, float>();
        
        foreach(KeyValuePair<int, TriangleNet.Data.Vertex> pair in meshRepresentation.vertices)
        {
            zOffsets.Add(pair.Key, Random.RandomRange(-zOffset, zOffset));
        }
        
        int triangleIndex = 0;
        List<Vector3> vertices = new List<Vector3>(meshRepresentation.triangles.Count * 3);
        List<int> triangleIndices = new List<int>(meshRepresentation.triangles.Count * 3);

        foreach(KeyValuePair<int, TriangleNet.Data.Triangle> pair in meshRepresentation.triangles)
        {
            TriangleNet.Data.Triangle triangle = pair.Value;

            TriangleNet.Data.Vertex vertex0 = triangle.GetVertex(0);
            TriangleNet.Data.Vertex vertex1 = triangle.GetVertex(1);
            TriangleNet.Data.Vertex vertex2 = triangle.GetVertex(2);

            Vector3 p0 = new Vector3((float) vertex0.x, (float) vertex0.y, zOffsets[vertex0.id]);
            Vector3 p1 = new Vector3((float)vertex1.x, (float)vertex1.y, zOffsets[vertex1.id]);
            Vector3 p2 = new Vector3((float)vertex2.x, (float)vertex2.y, zOffsets[vertex2.id]);

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);

            triangleIndices.Add(triangleIndex + 2);
            triangleIndices.Add(triangleIndex + 1);
            triangleIndices.Add(triangleIndex);

            triangleIndex += 3;
        }

        print("vertices: " + vertices.Count);


        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangleIndices.ToArray();
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
	}
	
    void OnDrawGizmos()
    {
        if (holes.Length <= 0)
            return;

        Gizmos.color = Color.red;
        foreach(UPolygon hole in holes)
        {
            hole.GizmoDraw();
        }

        if (meshRepresentation == null)
            return;

        Gizmos.color = gizmoColor;

        foreach(KeyValuePair<int, TriangleNet.Data.Triangle> pair in meshRepresentation.triangles)
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
