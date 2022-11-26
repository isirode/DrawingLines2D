using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputController : MonoBehaviour
{
    // TODO : provide a color picker and other LineRenderer related properties ?

    public new Camera camera;

    public enum DrawingState
    {
        Waiting,
        AddingPoints
    }

    [NonSerialized]
    public DrawingState drawingState;
    Vector2Recorder<Vector3> recorder = new Vector2Recorder<Vector3>();

    public Coroutine repeatingCoroutine;
    public float recordLatency = 0.1f;

    public delegate void LineBeginnedDelegate();
    public event LineBeginnedDelegate LineBeginned;

    public delegate void LineFinishedDelegate(List<Vector3> points);
    public event LineFinishedDelegate LineFinished;

    /// <summary>
    /// Callback when a point is added
    /// </summary>
    /// <param name="currentPoints">Current points, include the new point</param>
    /// <param name="newPoint">The new point which was added</param>
    public delegate void PointAddedDelegate(List<Vector3> currentPoints, Vector3 newPoint);
    public event PointAddedDelegate PointAdded;

    private void Start()
    {
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && drawingState == DrawingState.Waiting)
        {
            // Debug.Log("Start drawing");

            drawingState = DrawingState.AddingPoints;
            recorder.Clear();

            LineBeginned?.Invoke();

            repeatingCoroutine = StartCoroutine(AddPoint());
        } 
        else if (Input.GetMouseButtonUp(0) && drawingState == DrawingState.AddingPoints)
        {
            LineFinished?.Invoke(recorder.Get());
            Stop();
        }
    }

    IEnumerator AddPoint()
    {
        while (drawingState == DrawingState.AddingPoints)
        {
            // FIXME : Min is not right
            // WARN : using camera near clipping plane is important, without it, the LineRenderer will not be visible
            Vector3 newPoint = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Min(camera.nearClipPlane, 0.5f)));
            recorder.Add(newPoint);

            PointAdded?.Invoke(recorder.Get(), newPoint);

            yield return new WaitForSeconds(recordLatency);
        }
    }

    public void FinishLine()
    {
        Debug.Log(nameof(FinishLine));
        Stop();
        LineFinished?.Invoke(recorder.Get());
        recorder.Clear();
    }

    public void Stop()
    {
        // Debug.Log(nameof(Stop));
        if (repeatingCoroutine != null)
        {
            StopCoroutine(repeatingCoroutine);
        }
        drawingState = DrawingState.Waiting;
    }
}
