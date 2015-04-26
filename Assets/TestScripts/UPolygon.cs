using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class UPolygon 
{
    public Vector2[] points;

    public bool PointInPolygon(Vector2 point) 
    {
        int i,j;
        bool c = false;
        
        for(i = 0, j = points.Length - 1; i < points.Length; j = i++) 
        {
            if( ( ( (points[i].y) >= point.y ) != (points[j].y >= point.y) ) && (point.x <= (points[j].x - points[i].x) * (point.y - points[i].y) / (points[j].y - points[i].y) + points[i].x))
                c = !c;
        }

        return c;
    }

    public void GizmoDraw()
    {
        if (points.Length <= 1)
            return;

        for(int i = 0 ; i < points.Length - 1 ; i++)
        {
            Gizmos.DrawLine(points[i], points[i + 1]);
        }

        Gizmos.DrawLine(points[points.Length - 1], points[0]);
    }
}
