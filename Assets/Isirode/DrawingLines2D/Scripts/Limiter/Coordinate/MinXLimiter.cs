using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinXLimiter : MonoBehaviour
{
    public LegacyInputController legacyInputController;

    public float minX = 0f;

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
        if (newPoint.x <= minX)
        {
            legacyInputController.FinishLine();
        }
    }
}
