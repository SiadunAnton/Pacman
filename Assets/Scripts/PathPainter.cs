using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PathPainter 
{
    public static void DrawRoute(List<Node> nodes, Color color)
    {
        if (nodes == null || nodes.Count < 2)
            return;

        var points = new List<Vector3Int>(nodes.Select(x => x.Position));
        for(int i = 0; i < points.Count()-1; i++)
        {
            Debug.DrawLine(points[i],points[i+1], color, 0.5f,true);
        }
    }
}
