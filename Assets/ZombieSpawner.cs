using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // The zombie prefab to spawn
    public Transform player;        // Reference to the player or AR camera
    public float spawnHeight = -1f; // Height below the ground where zombies will spawn
    public float riseSpeed = 1f;    // Speed at which the zombie rises from the ground
    public float movementSpeed = 0.2f; // Speed of the zombie's movement
    public float spawnRadius = 10f; // Radius around the player where zombies will spawn
    public GameObject mistPrefab;    // The mist particle prefab
    public float mistDuration = 5f;  // Duration of the mist effect
                                     // Array to hold predefined grave spawn points
    public Transform[] graveSpawnPoints;

    void Start()
    {
        if (player == null)
        {
            player = Camera.main.transform; // Default to the main camera if no player is assigned
        }
        // Ensure there are graves defined before trying to spawn
        if (graveSpawnPoints.Length > 0)
        {
            SpawnZombie(); // Initial spawn
        }
        else
        {
            Debug.LogError("No grave spawn points assigned in ZombieSpawner!");
        }
    }

    public void SpawnZombie()
    {
        // Pick a random grave from the array
        int randomIndex = Random.Range(0, graveSpawnPoints.Length);
        Vector3 spawnPosition = graveSpawnPoints[randomIndex].position;
        spawnPosition.y += spawnHeight; // Adjust height to the configured spawn height

        // Instantiate the zombie at the selected grave spawn point
        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

        // Spawn the mist particle effect at the zombie's spawn location
        GameObject mist = Instantiate(mistPrefab, spawnPosition, Quaternion.identity);

        // Destroy the mist after a certain duration
        Destroy(mist, mistDuration);

        // Optionally, you can link the ZombieHealth to the spawner for future spawns
        ZombieHealth zombieHealth = zombie.GetComponent<ZombieHealth>();
        if (zombieHealth != null)
        {
            zombieHealth.zombieSpawner = this.gameObject;
        }
    }
}
