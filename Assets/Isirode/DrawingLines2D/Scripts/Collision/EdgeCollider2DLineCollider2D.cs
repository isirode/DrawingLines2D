using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : make it a MonoBehaviour or an abstract class ?
public class EdgeCollider2DLineCollider2D
{
    /// <summary>
    /// This allow to setup the physics using a a <see cref="EdgeCollider2D"/>
    /// WARNING : Unlike <see cref="PolygonCompositeLineCollider2D"/>, the lines can't interact between each others but they will interact with other colliders
    /// </summary>
    /// <param name="gameObject">The prefab GameObject in which the physics setup is happenning</param>
    /// <param name="points">The line's points</param>
    /// <param name="thickness">The thickness of the line</param>
    public static void Setup(GameObject gameObject, List<Vector3> points, float thickness)
    {
        var collider = gameObject.GetComponent<EdgeCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<EdgeCollider2D>();
        }
        collider.SetPoints(points.ConvertAll<Vector2>(point => new Vector2(point.x, point.y)));
        collider.edgeRadius = thickness;
    }
}
