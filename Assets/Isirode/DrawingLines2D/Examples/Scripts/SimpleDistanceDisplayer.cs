using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// FIXME : could be included in the library
/// <summary>
/// Allow to render the distance allowed
/// </summary>
public class SimpleDistanceDisplayer : MonoBehaviour
{
    public LegacyInputController legacyInputController;
    public DrawRing drawRing;
    public LineRenderer lineRenderer;
    public DistanceLimiter distanceLimiter;

    private bool isDrawing = false;

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
            legacyInputController.LineAdded += LineAdded;
        }
        else
        {
            Debug.LogWarning($"{nameof(legacyInputController)} is null.");
        }
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();

            lineRenderer?.gameObject.SetActive(false);
        }
        if (drawRing == null)
        {
            drawRing = GetComponent<DrawRing>();
        }
        if (distanceLimiter == null)
        {
            distanceLimiter = GetComponent<DistanceLimiter>();
        }
    }

    private void PointAdded(List<Vector3> currentPoints, Vector3 newPoint)
    {
        if (lineRenderer == null || drawRing == null || distanceLimiter == null)
        {
            // TODO : log a warning
            return;
        }
        if (!isDrawing)
        {
            isDrawing = true;

            drawRing.gameObject.transform.position = newPoint;

            drawRing.radius = distanceLimiter.limit;

            drawRing.Draw();

            lineRenderer?.gameObject.SetActive(true);
        }
    }


    private void LineAdded(List<Vector3> points, GameObject gameObject)
    {
        isDrawing = false;

        lineRenderer?.gameObject.SetActive(false);
    }
}
