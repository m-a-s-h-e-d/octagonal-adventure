using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class WorldDeath : MonoBehaviour
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

        StartCoroutine(DeathRoutine(collider.gameObject));
    }

    private IEnumerator DeathRoutine(GameObject gameObject)
    {
        string scenePath = gameObject.scene.path;
        Destroy(gameObject);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(scenePath);
    }
}
