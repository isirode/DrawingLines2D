using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonCompositeLineCollider2D
{
    /// <summary>
    /// This allow to setup the physics using a composite of polygon segments
    /// Unlike <see cref="EdgeCollider2DLineCollider2D"/>, the lines can interact between each others
    /// WARNING : there are cracks in the produced line (currently), between each segments
    /// </summary>
    /// <param name="gameObject">The prefab GameObject in which the physics setup is happenning</param>
    /// <param name="points">The line's points</param>
    /// <param name="thickness">The thickness of the line</param>
    public static void Setup(GameObject gameObject, List<Vector3> points, float thickness)
    {
        var collider = gameObject.GetComponent<CompositeCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<CompositeCollider2D>();
        }
        collider.generationType = CompositeCollider2D.GenerationType.Manual;

        // WARN : do not remove this
        // See https://forum.unity.com/threads/composite-collider-2d-does-not-collide-with-other-composite-collider-2d.1166960/
        collider.geometryType = CompositeCollider2D.GeometryType.Polygons;

        // Not doing what I thought it would do
        // collider.offsetDistance = 0.3f;

        var points2D = points.ConvertAll<Vector2>(point => new Vector2(point.x, point.y));
        for (int i = 0; i < points2D.Count; i++)
        {
            // Info : we explicitly dont do the last segment since it does not exist
            if (i == points2D.Count - 1) break;

            var first = new Vector2(points2D[i].x, points[i].y);
            var last = new Vector2(points2D[i + 1].x, points2D[i + 1].y);
            var sub = last - first;
            Vector2 normal1 = new Vector2(sub.y, -sub.x);// bottom
            Vector2 normal2 = new Vector2(-sub.y, sub.x);// up
            
            var one = first + (normal2.normalized * thickness);
            var two = first + (normal1.normalized * thickness);
            var three = last + (normal2.normalized * thickness);
            var four = last + (normal1.normalized * thickness);

            PolygonCollider2D polygon = gameObject.AddComponent<PolygonCollider2D>();
            polygon.points = (new List<Vector2>() { one, two, four, three }.ToArray());
            polygon.pathCount = 1;
            polygon.SetPath(0, polygon.points);
            //polygon.offset = 0.1f;
            polygon.usedByComposite = true;
            // polygon.usedByEffector = false;
        }
        collider.GenerateGeometry();
    }
}
