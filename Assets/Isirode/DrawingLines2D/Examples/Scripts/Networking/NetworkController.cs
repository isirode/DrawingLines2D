using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : make it compatible with the other scripts
/// <summary>
/// This is a copy of <see cref="BasicLineController"/>, adapted to work with connectivity
/// </summary>
public class NetworkController : NetworkBehaviour
{
    #region "Duplicated from BasicLineController"

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
    private GameObject currentLinePreviewGameObject;
    public bool previewing = false;

    // FIXME : it seem to be working without SyncVar, but it might be need later on
    String playerGuid = Guid.NewGuid().ToString();

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

    #endregion


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
        // currentLineGameObject = null;
    }

    private void LegacyInputController_PointAdded(List<Vector3> currentPoints, Vector3 newPoint)
    {
        AddPoint(currentPoints, newPoint);
    }

    private void LegacyInputController_LineFinished(List<Vector3> points)
    {
        if (!isLocalPlayer)
        {
            Debug.LogWarning($"Calling {nameof(LegacyInputController_LineFinished)} but not {nameof(isLocalPlayer)}.");
            return;
        }
        // TODO : check if livePreview is enabled
        if (currentLinePreviewGameObject == null)
        {
            CmdAddLine(points, false, playerGuid);
        }
        else
        {
            CmdValidatePreview(points, currentLinePreviewGameObject);
            currentLinePreviewGameObject = null;
            previewing = false;
        }
    }

    /// <summary>
    /// Add a line using the currents points recorded, stop the AddPoints coroutine
    /// </summary>
    [Command]
    public void CmdAddLine(List<Vector3> points, bool isPreview, String playerGuid)
    {
        Debug.Log(nameof(CmdAddLine));

        Rigidbody2D rigidbody2D;

        // FIXME : if we use currentLineGameObject = Instantiate(prefab, this.gameObject.transform);
        // This will add the game object to the client's GameObject
        // When the client leave : the lines will disappear
        // But we want a good hierarchy and the lines to be inside another GameObject
        // And since this is a spawned script, we cannot access easily another GameObject inside the scene
        // And since Mirror will throw errors, we cannot move the script elsewhere (we would need to access another GameObject of the scene)
        // currentLineGameObject = Instantiate(prefab, this.gameObject.transform);
        GameObject currentLineGameObject = Instantiate(prefab);

        // WARNING : this is not working, the colliders are not spawned
        // gameObject.SetActive(false);
        rigidbody2D = currentLineGameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D != null)
        {
            Debug.Log("Setting kinematic");
            rigidbody2D.isKinematic = true;
            rigidbody2D.simulated = false;
        }

        // Info : this is where we the line rendering was setup in the offline class
        // But this would not work here since the LineRenderer's data is not synced

        // TODO : can probably replace it by a polymorphism system
        // Setup the collision
        // Info : we do it this way because NetworkRigidBody2D did not seem to be working very well
        // The collider is added when the line is validated
        if (!isPreview)
        {
            AddCollider(currentLineGameObject, points);
        }

        // FIXME : is this useful in the network controller ?
        // WARNING : awaiting the game object to be ready before spawning it actively
        // gameObject.SetActive(true);
        if (rigidbody2D != null && !isPreview)
        {
            // never entering here
            Debug.Log("Removing kinematic");
            rigidbody2D.isKinematic = false;
            rigidbody2D.simulated = true;
        }

        // TODO : find a way to put the callbacks back
        // LineAdded?.Invoke(points, currentLineGameObject);

        // connectionToClient
        var networkLine = currentLineGameObject.GetComponent<NetworkLine>();
        if (networkLine != null)
        {
            networkLine.connectionId = connectionToClient.connectionId;

            networkLine.points = points;
            networkLine.thickness = thickness;
            networkLine.lineColor = lineColor;
            networkLine.useColor = useColor;
            // networkLine.lineGradient = lineGradient;
            networkLine.useGradient = useGradient;

            networkLine.isPreview = isPreview;
            networkLine.ownerPlayerGuid = playerGuid;
        }
        else
        {
            Debug.LogWarning($"A {nameof(NetworkLine)} component is necessary for the networked line system to work.");
        }

        var rigidBody2D = currentLineGameObject.GetComponent<Rigidbody2D>();
        if (rigidBody2D == null)
        {
            // not present
            Debug.LogWarning($"{nameof(Rigidbody2D)} is not present in the line.");
        }
        else
        {
            Debug.Log($"{nameof(Rigidbody2D)} is present in the line.");
        }

        // Info : this spawn the GameObject with server authority if connectionToClient is omitted
        NetworkServer.Spawn(currentLineGameObject, connectionToClient);

        RpcHasSpawned(currentLineGameObject, points);

        currentLineGameObject = null;
    }

    [Command]
    public void CmdValidatePreview(List<Vector3> points, GameObject currentLineGameObject)
    {
        Debug.Log(nameof(CmdValidatePreview));

        Rigidbody2D rigidbody2D;

        AddCollider(currentLineGameObject, points);

        // FIXME : currently, we do not add the physics in preview
        // It could be useful in some cases
        rigidbody2D = currentLineGameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log($"{nameof(Rigidbody2D)} is not present in the game object.");
        }
        else
        {
            Debug.Log($"{nameof(Rigidbody2D)} is present in the game object.");

            rigidbody2D.isKinematic = false;
            rigidbody2D.simulated = true;
        }
    }

    [ClientRpc]
    public void RpcHasSpawned(GameObject lineGameObject, List<Vector3> points)
    {
        Debug.Log($"Has spawned: {lineGameObject.name}");

        // WARNING : LineRenderer points are not sync
        var lineRenderer = lineGameObject.GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogWarning($"{nameof(LineRenderer)} is not present in the game object but should be.");
            return;
        }

        // Info : we could update the LineRenderer's positions here
        // But this would work if a client join an existing instance : we would not enter this method then
        // And the LineRenderer's would be invisible

        var connectionId = -1;
        var networkLine = lineGameObject.GetComponent<NetworkLine>();
        if (networkLine != null)
        {
            connectionId = networkLine.connectionId;
        }
        else
        {
            Debug.LogWarning($"A {nameof(NetworkLine)} component is necessary for the networked line system to work.");
            return;
        }
        if (connectionToServer == null)
        {
            Debug.LogWarning($"{nameof(connectionToServer)} is null but should not be.");
            return;
        }
        if (connectionId != connectionToServer.connectionId)
        {
            // Client will log : You are not the object owner, you are 0, owner is -1410334697.
            Debug.Log($"You are not the object owner according to connectionId, you are {connectionToServer.connectionId}, owner is {connectionId}.");
        }
        if (networkLine.isOwned)
        {
            Debug.Log($"You are object owner according to {nameof(networkLine.isOwned)}.");
        }
        // TODO : inline this condition
        if (playerGuid.Equals(networkLine.ownerPlayerGuid) && livePreview && networkLine.isPreview)
        {
            Debug.Log($"Setting the  {nameof(currentLinePreviewGameObject)}.");
            currentLinePreviewGameObject = lineGameObject;
        }
        else
        {
            Debug.Log($"{nameof(networkLine.isPreview)} is {networkLine.isPreview}, GUID : {networkLine.ownerPlayerGuid}");
        }
    }

    void AddPoint(List<Vector3> currentPoints, Vector3 newPoint)
    {
        // TODO : livePreview : we need to update the points of the NetworkLine
        // Local script should have it's target correctly setted
        if (livePreview)
        {
            if (currentLinePreviewGameObject == null)
            {
                Debug.Log("Instantiating preview");
               
                if (!previewing)
                {
                    Debug.Log("Send command to server");
                    previewing = true;
                    CmdAddLine(currentPoints, true, playerGuid);
                }
                else
                {
                    Debug.Log("Is already previewing");
                }
            }
            else
            {
                // FIXME : should currentLinePreviewGameObject be a NetworkLine ?
                var networkLine = currentLinePreviewGameObject.GetComponent<NetworkLine>();
                if (networkLine == null)
                {
                    Debug.LogWarning($"{nameof(networkLine)} is null but should not be.");
                    return;
                }
                Debug.Log($"Updating {nameof(networkLine.points)}");
                // WARNING : this is not even updating the points using the callback in host+client mode
                //networkLine.points = currentPoints;

                CmdUpdatePoints(networkLine, currentPoints);
            }
        }
    }

    [Command]
    void CmdUpdatePoints(NetworkLine networkLine, List<Vector3> currentPoints)
    {
        networkLine.points = currentPoints;
    }

    private void AddCollider(GameObject lineGameObject, List<Vector3> points)
    {
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
