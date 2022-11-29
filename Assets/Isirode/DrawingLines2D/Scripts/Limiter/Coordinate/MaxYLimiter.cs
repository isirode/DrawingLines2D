using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxYLimiter : MonoBehaviour
{
    public LegacyInputController legacyInputController;

    public float maxY = 0f;

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
        if (newPoint.y >= maxY)
        {
            legacyInputController.FinishLine();
        }
    }
}
