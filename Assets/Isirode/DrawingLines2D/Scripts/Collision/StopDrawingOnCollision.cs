using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopDrawingOnCollision : MonoBehaviour
{
    public Camera cameraUsed;
    public LegacyInputController legacyInputController;
    public string targetTag = string.Empty;

    void Start()
    {
        if (cameraUsed == null)
        {
            cameraUsed = Camera.main;
        }
        var rigidBody2D = GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
        {
            Debug.LogWarning($"Unity's collision system require a {nameof(Rigidbody2D)} to be able to detect collisions, {nameof(StopDrawingOnCollision)} will not work without one on game object {this.gameObject.name}");
        }
        else
        {
            // TODO : check the constraints, log a warning
            // Info : we have a RigidBody2D because collision do not work otherwise
            // It has to be a dynamic rigidbody
            // The only way to make it work is to freeze all
            rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        var collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogWarning($"Unity's collision system require a {nameof(Collider2D)} to be able to detect collisions, {nameof(StopDrawingOnCollision)} will not work without one on game object {this.gameObject.name}");
        }

        if (legacyInputController == null)
        {
            throw new Exception($"{nameof(legacyInputController)} is required in {nameof(StopDrawingOnCollision)}, game object {this.gameObject.name}");
        }
    }

    void FixedUpdate()
    {
        // FIXME : Min is not right
        Vector3 newPosition = cameraUsed.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Min(cameraUsed.nearClipPlane, 0.5f)));
        this.gameObject.GetComponent<Collider2D>().transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(nameof(OnTriggerEnter2D));

        StopDrawingIfRequired(collision.tag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(nameof(OnCollisionEnter2D));

        StopDrawingIfRequired(collision.gameObject.tag);
    }

    private void StopDrawingIfRequired(string collisionTag)
    {
        if (this.targetTag == string.Empty || this.targetTag == collisionTag)
        {
            legacyInputController.FinishLine();
        }
    }
}
