using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStaticOnCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Collision with : {collision.gameObject.name}");

        var rigidbody2D = collision.gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody2D == null)
        {
            Debug.Log($"{collision.gameObject.name} does not have a {nameof(Rigidbody2D)}");
            return;
        }
        rigidbody2D.bodyType = RigidbodyType2D.Static;
    }
}
