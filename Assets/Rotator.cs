using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private float speed = 0;

    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(
            transform.localEulerAngles.x, 
            transform.localEulerAngles.y, 
            transform.localEulerAngles.z - (speed / 60));
    }
}
