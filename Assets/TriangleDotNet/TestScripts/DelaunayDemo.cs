using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TriangleNet.Geometry;

public class DelaunayDemo : MonoBehaviour 
{
    TriangleNet.Mesh meshRepresentation;
    InputGeometry geometry;

    public float distance = 5;
    public float verticalDistance = 2;
    public float boxDistance = 1f;
    public float circleDistance = 0.7f;

    public UPolygon logoOutline;
    public UPolygon[] holes; 

    public float boxWidth = 1f;
    public float zOffset = 0.5f;

    Mesh mesh;

	// Use this for initialization
	void Start () 
    {
        geometry = new InputGeometry();

        //it is necessary to put a border around all the points in order to get triangulation to work correctly when holes are used
        List<Point> border = new List<Point>();
        border.Add(new Point(distance, verticalDistance));
        border.Add(new Point(distance, -verticalDistance));
        border.Add(new Point(-distance, -verticalDistance));
        border.Add(new Point(-distance, verticalDistance));
        geometry.AddRing(border);

        List<Point> outlinePoints = new List<Point>(logoOutline.points.Length);
        foreach (Vector2 coordinates in logoOutline.points)
        {
            outlinePoints.Add(new Point(coordinates));
        }

        geometry.AddRingAsHole(outlinePoints, 0);
        

        foreach(UPolygon hole in holes)
        {
            List<Point> holePoints = new List<Point>(hole.points.Length);

            foreach (Vector2 coordinates in hole.points)
                holePoints.Add(new Point(coordinates));

            geometry.AddRing(holePoints, 0);
        }


        List<Point> points = new List<Point>();
        for (float offsetX = -distance; offsetX < distance; offsetX += boxDistance)
        {
            for (float offsetY = -verticalDistance; offsetY < verticalDistance; offsetY += boxDistance)
            {
                Vector2 offset = new Vector2(offsetX, offsetY) + Vector2.one * boxDistance * 0.5f;

                float radians = Random.RandomRange(0, 2 * Mathf.PI);
                float length = Random.RandomRange(0, circleDistance);

                Vector2 pos = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * length;
                pos += offset;

                bool insideOutline = logoOutline.PointInPolygon(pos);

                bool stillAlloved = false;
                for (int i = 0; i < holes.Length; i++ )
                {
                    if (holes[i].PointInPolygon(pos))
                        stillAlloved = true;
                }

                if (!insideOutline || stillAlloved)
                    geometry.AddPoint((float)pos.x, (float)pos.y, 0);
            }
        }

        meshRepresentation = new TriangleNet.Mesh();
        meshRepresentation.Triangulate(geometry);

        //generate mesh based on triangulation

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

            Vector3 p0 = new Vector3( vertex0.x, vertex0.y, zOffsets[vertex0.id]);
            Vector3 p1 = new Vector3( vertex1.x, vertex1.y, zOffsets[vertex1.id]);
            Vector3 p2 = new Vector3( vertex2.x, vertex2.y, zOffsets[vertex2.id]);

            vertices.Add(p0);
            vertices.Add(p1);
            vertices.Add(p2);

            triangleIndices.Add(triangleIndex + 2);
            triangleIndices.Add(triangleIndex + 1);
            triangleIndices.Add(triangleIndex);

            triangleIndex += 3;
        }
        
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

        Gizmos.color = Color.black;
        logoOutline.GizmoDraw();

        Gizmos.color = Color.white;
        foreach(UPolygon hole in holes)
        {
            hole.GizmoDraw();
        }

        if (meshRepresentation == null)
            return;

        Gizmos.color = Color.cyan;

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
}
