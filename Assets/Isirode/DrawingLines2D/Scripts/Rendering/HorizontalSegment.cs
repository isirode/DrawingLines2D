using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSegment : MonoBehaviour
{
    public float yPos = 0f;
    public float xPosCenter = 0f;
    public float length = 10f;
    public float thickness = 0.2f;

    public LineRenderer lineRenderer;
    
    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        Setup();
    }

    
    public void Setup()
    {
        var zPos = this.transform.position.z;
        var halfLength = length / 2f;
        var startPoint = new Vector3(xPosCenter - halfLength, yPos, zPos);
        var endPoint = new Vector3(xPosCenter + halfLength, yPos, zPos);
        var points = new List<Vector3>() { startPoint, endPoint };
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.startWidth = thickness;
        lineRenderer.endWidth = thickness;
        
        // TODO : setup color or gradient ?
    }
}
