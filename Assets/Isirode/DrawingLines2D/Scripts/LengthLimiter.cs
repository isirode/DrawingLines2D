using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This allow to stop adding points and render the line when the limit is reached
/// This assume that only one line is computed at a time
/// </summary>
public class LengthLimiter : MonoBehaviour
{
    public float limit = 50f;

    public LegacyInputController legacyInputController;

    private Vector2 lastPoint = default(Vector2);
    private float currentLength = 0f;

    // Start is called before the first frame update
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
        if (lastPoint == default(Vector2))
        {
            lastPoint = new Vector2(newPoint.x, newPoint.y);
            return;
        }

        currentLength += Vector2.Distance(lastPoint, newPoint);

        Debug.Log($"Current line length : {currentLength}");

        if (currentLength >= limit)
        {
            // Info : we could use the delegate LineAdded which would reset the state of the LengthLimiter
            legacyInputController.AddLine();
            ResetState();
        }
    }

    private void LineAdded(List<Vector3> points, GameObject gameObject)
    {
        ResetState();
    }

    // Info : Reset method is a Unity method
    private void ResetState()
    {
        // FIXME : this is used as an hidden state control, maybe use an explicit state
        lastPoint = default(Vector2);
        currentLength = 0f;
    }

    // TODO : move it to an extension class
    private void GetCumulatedLength(List<Vector3> points)
    {
        // FIXME : this is not very optimized
        // Could do one iteration
        // Could remember the last length, last point
        var points2D = points.ConvertAll<Vector2>(point => new Vector2(point.x, point.y));

        var length = 0f;
        var lastPoint = default(Vector2);

        // TODO : use something more optimized ? Hard to do using Linq
        points2D.ForEach(vector2 =>
        {
            if (lastPoint == default(Vector2)) 
            {
                lastPoint = vector2;
                return;
            }

            length += Vector2.Distance(lastPoint, vector2);

            lastPoint = vector2;
        });

        // etc

    }
}
