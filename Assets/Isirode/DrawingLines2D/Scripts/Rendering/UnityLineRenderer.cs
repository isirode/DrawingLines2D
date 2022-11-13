using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityLineRenderer
{
    public static void Setup(GameObject gameObject, List<Vector3> points, float thickness)
    {
        var lineRenderer = gameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
    }
}
