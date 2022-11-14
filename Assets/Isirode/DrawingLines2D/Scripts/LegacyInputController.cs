using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyInputController : MonoBehaviour
{
    // TODO : provide a color picker and other LineRenderer related properties ?

    enum DrawingState
    {
        Waiting,
        AddingPoints
    }

    DrawingState drawingState;
    Vector2Recorder<Vector3> recorder = new Vector2Recorder<Vector3>();

    public Coroutine repeatingCoroutine;
    public float recordLatency = 0.1f;

    public enum PhysicsType
    {
        NoPhysics,
        EdgeCollider2D,
        PolygonComposite2D
    }
    public PhysicsType physicsType = PhysicsType.PolygonComposite2D;

    public GameObject prefab;

    public float thickness = 0.1f;
    public static float COLLIDER_THICKNESS_MULTIPLIER = 0.5f;

    public new Camera camera;

    public Color lineColor = Color.white;
    public bool useColor = true;

    /// <summary>
    /// Click on it to open the editor
    /// Click below it to add a color
    /// Click on a pin to set it's color
    /// Click on a pin and click "del" to remove a pin
    /// Click on top of it to set an alpha value
    /// </summary>
    public Gradient lineGradient;
    public bool useGradient = false;

    // TODO : move it to another common class
    public delegate void LineAddedDelegate(List<Vector3> points, GameObject gameObject);
    public event LineAddedDelegate LineAdded;

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
            drawingState = DrawingState.AddingPoints;
            recorder.Clear();
            repeatingCoroutine = StartCoroutine(AddPoint());
        } 
        else if (Input.GetMouseButtonUp(0) && drawingState == DrawingState.AddingPoints)
        {
            AddLine();
        }
    }

    /// <summary>
    /// Add a line using the currents points recorded, stop the AddPoints coroutine
    /// </summary>
    public void AddLine()
    {
        drawingState = DrawingState.Waiting;
        StopCoroutine(repeatingCoroutine);

        var points = recorder.Get();
        var gameObject = Instantiate(prefab, this.gameObject.transform);

        // WARNING : this is not working, the colliders are not spawned
        // gameObject.SetActive(false);
        var rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
        if (rigidBody2D != null)
        {
            rigidBody2D.isKinematic = true;
        }

        // TODO : provide different line rendering systems
        // Setup the display
        UnityLineRenderer.Setup(gameObject, points, thickness, lineColor, useColor, lineGradient, useGradient);

        // TODO : can probably replace it by a polymorphism system
        // Setup the collision
        switch (physicsType)
        {
            case PhysicsType.NoPhysics:
                // do nothing
                var collider = gameObject.GetComponent<Collider2D>();
                if (collider != null)
                {
                    Debug.LogWarning($"You have picked {nameof(PhysicsType)} {physicsType} but a {nameof(Collider2D)} {collider.GetType().Name} is present in the chosen prefab {prefab.name}.");
                }
                if (gameObject.GetComponent<Rigidbody2D>())
                {
                    Debug.LogWarning($"You have picked {nameof(PhysicsType)} {physicsType} but a {nameof(Rigidbody2D)} is present in the chosen prefab {prefab.name}.");
                }
                break;
            case PhysicsType.EdgeCollider2D:
                EdgeCollider2DLineCollider2D.Setup(gameObject, points, thickness);
                break;
            case PhysicsType.PolygonComposite2D:
                PolygonCompositeLineCollider2D.Setup(gameObject, points, thickness * COLLIDER_THICKNESS_MULTIPLIER);
                break;
            default:
                throw new Exception($"{nameof(PhysicsType)} {physicsType} is not currently handler.");
        }

        // WARNING : awaiting the game object to be ready before spawning it actively
        // gameObject.SetActive(true);
        if (rigidBody2D != null)
        {
            rigidBody2D.isKinematic = false;
        }

        LineAdded?.Invoke(points, gameObject);
    }

    IEnumerator AddPoint()
    {
        while (drawingState == DrawingState.AddingPoints)
        {
            // WARN : using camera near clipping plane is important, without it, the LineRenderer will not be visible
            Vector3 newPoint = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Min(camera.nearClipPlane, 0.5f)));
            recorder.Add(newPoint);
            PointAdded?.Invoke(recorder.Get(), newPoint);
            yield return new WaitForSeconds(recordLatency);
        }
    }
}
