using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class WorldNextLevel : MonoBehaviour
{
    private new SpriteShapeRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteShapeRenderer>();
    }

    private void Start()
    {
        renderer.color = Color.clear;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag != "Player") { return; }
        SceneManager.LoadScene(collider.gameObject.scene.buildIndex + 1);
    }
}
