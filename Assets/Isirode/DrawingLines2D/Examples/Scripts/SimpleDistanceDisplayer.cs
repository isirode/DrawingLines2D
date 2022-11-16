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
            // FIXME : listen to the callbacks of BasicLineControlle ?
            // If so, will not be usable with another controller
            legacyInputController.PointAdded += PointAdded;
            legacyInputController.LineFinished += OnLineFinished;
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
        // Debug.Log($"{nameof(SimpleDistanceDisplayer)}:{nameof(PointAdded)}");

        if (lineRenderer == null || drawRing == null || distanceLimiter == null)
        {
            // TODO : log a warning
            return;
        }
        // WARNING : checking the state of the legacyInputController is very important
        // Otherwise the delegate will call DistanceLimiter (it will disable the circle), and later on this method (it will enable the circle)
        // Causing a bug
        if (!isDrawing && legacyInputController.drawingState == LegacyInputController.DrawingState.AddingPoints)
        {
            // Debug.Log("Enabling the circle");

            isDrawing = true;

            drawRing.gameObject.transform.position = newPoint;

            drawRing.radius = distanceLimiter.limit;

            drawRing.Draw();

            lineRenderer?.gameObject.SetActive(true);
        }
    }


    private void OnLineFinished(List<Vector3> points)
    {
        // Debug.Log("Stop rendering the circle");

        isDrawing = false;

        lineRenderer?.gameObject.SetActive(false);
    }
}
