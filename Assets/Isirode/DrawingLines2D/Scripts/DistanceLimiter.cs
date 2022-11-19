using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class allow to limit the maximum distance between the points added and the first point of the line
/// WARNING : the class allow to have a line longer to the limit since the limit will be applied after the new point is added
/// </summary>
public class DistanceLimiter : MonoBehaviour
{
    public float limit = 15f;

    public LegacyInputController legacyInputController;

    private Vector2 firstPoint = default(Vector2);

    void Start()
    {
        if (legacyInputController == null)
        {
            // attempt to load it from the current game object
            legacyInputController = GetComponent<LegacyInputController>();
        }
        if (legacyInputController != null)
        {
            legacyInputController.PointAdded += PointAdded;
        }
        else
        {
            Debug.LogWarning($"{nameof(legacyInputController)} is null.");
        }
    }

    private void PointAdded(List<Vector3> currentPoints, Vector3 newPoint)
    {
        Debug.Log(nameof(PointAdded));

        if (firstPoint == default(Vector2))
        {
            firstPoint = new Vector2(newPoint.x, newPoint.y);
            return;
        }

        float currentDistance = Vector2.Distance(firstPoint, newPoint);

        // Debug.Log($"Current distance to first point : {currentDistance}");

        if (currentDistance >= limit)
        {
            Debug.Log("current distance >= limit");

            // Info : we could use the delegate LineAdded which would reset the state
            legacyInputController.FinishLine();
            ResetState();
        }
    }

    // FIXME : could use the delegate here
    private void LineAdded(List<Vector3> points, GameObject gameObject)
    {
        ResetState();
    }

    // Info : Reset method is a Unity method
    private void ResetState()
    {
        // Debug.Log(nameof(ResetState));
        // FIXME : this is used as an hidden state control, maybe use an explicit state
        firstPoint = default(Vector2);
    }
}
