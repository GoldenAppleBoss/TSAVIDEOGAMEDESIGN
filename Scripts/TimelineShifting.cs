using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineShifting : MonoBehaviour
{
    // The endpoint the player will warp to
    public GameObject endPoint;

    [System.Serializable]
    public class SpawnLocation
    {
        [Tooltip("Value for X Offset")]
        public int offsetX;

        [Tooltip("Value for Y Offset")]
        public int offsetY;
    }

    // Fixed spawn location object
    public SpawnLocation spawnConfig = new SpawnLocation();

    void Start()
    {
        ValidateSpawnConfig();
    }

    void Update()
    {

    }

    private void ValidateSpawnConfig()
    {
        if (spawnConfig == null)
        {
            Debug.LogWarning("Spawn Config is null. Initializing defaults.");
            spawnConfig = new SpawnLocation();
        }
    }
}
