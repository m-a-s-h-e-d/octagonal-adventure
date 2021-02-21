using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTarget : MonoBehaviour
{
    private Grapple grapple;

    private void Awake()
    {
        grapple = transform.parent.GetComponent<Grapple>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (!grapple.Seeking) { return; }
        transform.Translate(grapple.Direction * grapple.Speed);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        switch (collider.gameObject.tag)
        {
            case "Player":
                return;

            case "WorldBlocked":
                Destroy(grapple.gameObject); 
                return;

            default:
                transform.SetParent(collider.transform, true);
                grapple.OnCollision(collider);
                break;
        }
    }
}
