using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    [SerializeField]
    private Vector3 offset = Vector3.zero;

    private Vector3 defaultPosition;

    private bool reverse = false;

    private Vector3 targetPosition => defaultPosition + offset;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        if (!reverse)
        {
            transform.localPosition = Vector2.MoveTowards(
                transform.localPosition, 
                defaultPosition + offset, 
                speed / 60);
        } 
        else
        {
            transform.localPosition = Vector2.MoveTowards(
                transform.localPosition, 
                defaultPosition, 
                speed / 60);
        }

        // Checks if in starting or ending position and reverses direction if true
        if (transform.localPosition == targetPosition || 
            transform.localPosition == defaultPosition)
        {
            reverse = !reverse;
        }
    }
}
