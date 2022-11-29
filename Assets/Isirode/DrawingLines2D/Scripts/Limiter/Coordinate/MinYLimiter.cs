using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinYLimiter : MonoBehaviour
{
    public LegacyInputController legacyInputController;

    public float minY = 0f;
    
    void Start()
    {
        if (legacyInputController == null)
        {
            legacyInputController = GetComponent<LegacyInputController>();
        }
        if (legacyInputController != null)
        {
            legacyInputController.PointAdded += PointAdded;
        }
    }

    private void PointAdded(List<Vector3> currentPoints, Vector3 newPoint)
    {
        if (newPoint.y <= minY)
        {
            legacyInputController.FinishLine();
        }
    }
}
