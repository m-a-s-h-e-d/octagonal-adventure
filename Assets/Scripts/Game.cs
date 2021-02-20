using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    /// <summary>
    /// Gets prefab.
    /// </summary>
    /// <param name="name">Prefab name</param>
    /// <returns>Prefab</returns>
    public static GameObject LoadPrefab(string name)
    {
        return Resources.Load<GameObject>($"Prefabs/{name}"); 
    }

    /// <summary>
    /// Gets prefab and clones it into the scene.
    /// </summary>
    /// <param name="name">Prefab name</param>
    /// <returns>Clone</returns>
    public static GameObject InstantiatePrefab(string name)
    {
        return Object.Instantiate(LoadPrefab(name)); 
    }
}
