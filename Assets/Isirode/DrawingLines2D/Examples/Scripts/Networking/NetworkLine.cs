using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkLine : NetworkBehaviour
{
    [SyncVar]
    public int connectionId;

    // WARNING : we to use those
    [SyncVar(hook = nameof(SetPoints))]
    public List<Vector3> points;

    [SyncVar]
    public float thickness;

    /// <summary>
    /// This will control the color of the line
    /// For now, you can modify it inside the Editor, you should run the client+host in Unity, and the client only with the exe
    /// Otherwise, it will not work
    /// </summary>
    [SyncVar]
    public Color lineColor;

    [SyncVar]
    public bool useColor;

    //[SyncVar]
    //public Gradient lineGradient;

    [SyncVar]
    public bool useGradient;

    private LineRenderer lineRenderer;

    /*
     * There is a bug : 
     * If line already added, the next client will show them, but there physics will bug, and they will fall
     * If re-add a line, and collide with the one at the server position : it will be shown at the correct position
     * It need to be a "real" collision, even if the new line it as the correct position, the older line might not be shown
     * By rendering a new line 'inside' the other, it is usually shown
     * It can fail, sometimes, the line will reappear, but the simulation will go wrong, and it will need to be updated again
     * 
     * I think the local simulation of the client does not handle well the physics at the start
     * The server seem to be updating only when there is a collision
     * 
     * I've removed the RigidBody2D from the prefab variant, and disabled the NetworkRigidBody2D
     * 
     * The Target was correctly set
     * Client Authority was not checked
     * Sync Direction was Server To Client
     * Sync Mode was Observers
     * Sync Interval was 0.1
     * 
     */
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log(nameof(OnStartClient));

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            // TODO : make a utility method for this kind of stuff
            Debug.LogWarning($"{nameof(LineRenderer)} is not present in the game object but should be.");
            return;
        }
        var positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        Debug.Log($"GameObject points are : {string.Join(',', positions)}");
        Debug.Log($"Real points are : {string.Join(',', points)}");
        UnityLineRenderer.Setup(gameObject, points, thickness, lineColor, useColor, null, useGradient);
    }

    /// <summary>
    /// WARNING : in client + host, this will be called before <see cref="OnStartClient"></see>
    /// In client only mode, this is called after <see cref="OnStartClient"></see>
    /// But the properties will already be set correctly
    /// </summary>
    public void Start()
    {
        Debug.Log(nameof(Start));
        if (lineRenderer == null)
        {
            Debug.LogWarning($"{nameof(LineRenderer)} is not set but should have been set by {nameof(OnStartClient)}.");
            lineRenderer = GetComponent<LineRenderer>();
            if (lineRenderer == null)
            {
                // TODO : make a utility method for this kind of stuff
                Debug.LogWarning($"{nameof(LineRenderer)} is not present in the game object but should be.");
                return;
            }
        }
        var positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        Debug.Log($"GameObject points are : {string.Join(',', positions)}");
        Debug.Log($"Real points are : {string.Join(',', points)}");
    }

    private void SetPoints(List<Vector3> oldPoints, List<Vector3> newPoints)
    {
        Debug.Log(nameof(SetPoints));

        // Info : at the beginning
        // client + host (drawing) : this is called before Start
        // client + host (client drawing) : this is called before Start
        // client joining : this is called before OnStartClient
        // client drawing : this is called before OnStartClient

        Debug.Log($"{nameof(oldPoints)} : {string.Join(',', oldPoints)}");
        Debug.Log($"{nameof(newPoints)} : {string.Join(',', newPoints)}");
    }

}
