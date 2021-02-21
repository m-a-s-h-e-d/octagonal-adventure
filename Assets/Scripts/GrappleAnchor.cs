using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleAnchor : MonoBehaviour
{
    private DistanceJoint2D joint;

    private Player player;

    private void Awake()
    {
        joint = GetComponent<DistanceJoint2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
    }
}
