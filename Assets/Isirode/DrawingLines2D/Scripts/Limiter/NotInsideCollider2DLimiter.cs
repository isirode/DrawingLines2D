using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotInsideCollider2DLimiter : MonoBehaviour
{
    // WARNING : this will not work with a EdgeCollider2D or a CompositeCollider2D set as edge physics
    // TODO : verify this in the Start method
    public new Collider2D collider2D;

    public LegacyInputController legacyInputController;

    void Start()
    {
        if (collider2D == null)
        {
            collider2D = GetComponent<Collider2D>();
        }
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
        // Info : we could also use the bounds
        if (collider2D.OverlapPoint(new Vector2(newPoint.x, newPoint.y)))
        {
            legacyInputController.FinishLine();
        }
    }
}
