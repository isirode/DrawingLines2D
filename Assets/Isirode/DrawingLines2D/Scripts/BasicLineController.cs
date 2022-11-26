using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLineController : MonoBehaviour
{

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

    public Color lineColor = Color.white;
    public bool useColor = true;

    /// <summary>
    /// If set to true, the line will be shown live, the collider will only be added when the drawing is done
    /// </summary>
    public bool livePreview = false;
    private GameObject currentLineGameObject;

    /// <summary>
    /// Click on it to open the editor
    /// Click below it to add a color
    /// Click on a pin to set it's color
    /// Click on a pin and click "del" to remove a pin
    /// Click on top of it to set an alpha value
    /// </summary>
    public Gradient lineGradient;
    public bool useGradient = false;

    public LegacyInputController legacyInputController;

    // TODO : move it to another common class
    public delegate void LineAddedDelegate(List<Vector3> points, GameObject gameObject);
    public event LineAddedDelegate LineAdded;

    private void Start()
    {
        if (legacyInputController == null)
        {
            legacyInputController = GetComponent<LegacyInputController>();
        }
        if (legacyInputController == null)
        {
            Debug.LogWarning($"{nameof(legacyInputController)} is null, this component cannot work without it");
            return;
        }
        legacyInputController.LineBeginned += LegacyInputController_LineBeginned;
        legacyInputController.LineFinished += LegacyInputController_LineFinished;
        legacyInputController.PointAdded += LegacyInputController_PointAdded; ;
    }

    private void LegacyInputController_LineBeginned()
    {
        currentLineGameObject = null;
    }

    private void LegacyInputController_PointAdded(List<Vector3> currentPoints, Vector3 newPoint)
    {
        AddPoint(currentPoints, newPoint);
    }

    private void LegacyInputController_LineFinished(List<Vector3> points)
    {
        AddLine(points);
    }

    /// <summary>
    /// Add a line using the currents points recorded, stop the AddPoints coroutine
    /// </summary>
    public void AddLine(List<Vector3> points)
    {
        Debug.Log(nameof(AddLine));
        // Stop();

        Rigidbody2D rigidbody2D;

        if (livePreview)
        {
            if (currentLineGameObject == null)
            {
                Debug.LogWarning($"{nameof(currentLineGameObject)} is null, but {nameof(livePreview)} is on, it should be null.");

                Debug.Log("Instantiating");
                currentLineGameObject = Instantiate(prefab, this.gameObject.transform);
            }
        }
        else
        {
            Debug.Log("Instantiating");
            currentLineGameObject = Instantiate(prefab, this.gameObject.transform);
        }

        // WARNING : this is not working, the colliders are not spawned
        // gameObject.SetActive(false);
        rigidbody2D = currentLineGameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            rigidbody2D.isKinematic = true;
            rigidbody2D.simulated = false;
        }

        // TODO : provide different line rendering systems
        // Setup the display
        // FIXME : could skip it if livePreview is true ?
        UnityLineRenderer.Setup(currentLineGameObject, points, thickness, lineColor, useColor, lineGradient, useGradient);

        // TODO : can probably replace it by a polymorphism system
        // Setup the collision
        SetupLinePhysics(points, currentLineGameObject);

        // WARNING : awaiting the game object to be ready before spawning it actively
        // gameObject.SetActive(true);
        if (rigidbody2D != null)
        {
            rigidbody2D.isKinematic = false;
            rigidbody2D.simulated = true;
            // rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        LineAdded?.Invoke(points, currentLineGameObject);

        currentLineGameObject = null;
    }

    void AddPoint(List<Vector3> currentPoints, Vector3 newPoint)
    {
        // FIXME : maybe call the delegate PointAdded before rendering it
        // Info : could make another class for the live preview, but it would need to reset the points anyway it seems
        // There is a possility that increasing the count and using SetPosition() would work without the reset
        if (livePreview)
        {
            if (currentLineGameObject == null)
            {
                Debug.Log("Instantiating");
                currentLineGameObject = Instantiate(prefab, this.gameObject.transform); 

                var rigidBody2D = currentLineGameObject.GetComponent<Rigidbody2D>();

                if (rigidBody2D != null)
                {
                    // TODO : make it an extension ?
                    // FIXME : use Sleep instead ?
                    //   If yes, modify the other parts of the code using this
                    rigidBody2D.isKinematic = true;
                    rigidBody2D.simulated = false;
                    // Debug.Log("Freezing all");
                    // rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
            UnityLineRenderer.Setup(currentLineGameObject, currentPoints, thickness, lineColor, useColor, lineGradient, useGradient);
            // SetupLinePhysics(currentPoints, currentLineGameObject);
        }
    }

    void SetupLinePhysics(List<Vector3> points, GameObject lineGameObject)
    {
        // TODO : can probably replace it by a polymorphism system
        // Setup the collision
        switch (physicsType)
        {
            case PhysicsType.NoPhysics:
                // do nothing
                var collider = lineGameObject.GetComponent<Collider2D>();
                if (collider != null)
                {
                    Debug.LogWarning($"You have picked {nameof(PhysicsType)} {physicsType} but a {nameof(Collider2D)} {collider.GetType().Name} is present in the chosen prefab {prefab.name}.");
                }
                if (lineGameObject.GetComponent<Rigidbody2D>())
                {
                    Debug.LogWarning($"You have picked {nameof(PhysicsType)} {physicsType} but a {nameof(Rigidbody2D)} is present in the chosen prefab {prefab.name}.");
                }
                break;
            case PhysicsType.EdgeCollider2D:
                EdgeCollider2DLineCollider2D.Setup(lineGameObject, points, thickness);
                break;
            case PhysicsType.PolygonComposite2D:
                PolygonCompositeLineCollider2D.Setup(lineGameObject, points, thickness * COLLIDER_THICKNESS_MULTIPLIER);
                break;
            default:
                throw new Exception($"{nameof(PhysicsType)} {physicsType} is not currently handler.");
        }
    }
}
