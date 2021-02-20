using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private const float FollowSpeed = 50;

    private new Camera camera;

    private void Awake()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        Cursor.visible = false;

        Vector3 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 targetPosition = new Vector3(
            mousePosition.x, 
            mousePosition.y, 
            transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * FollowSpeed);
    }
}
