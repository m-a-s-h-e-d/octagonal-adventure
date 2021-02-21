using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(
            transform.eulerAngles.x, 
            transform.eulerAngles.y, 
            transform.eulerAngles.z - speed);
    }
}
