using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    // Array of different zombie prefabs
    public GameObject[] zombiePrefabs; // Three zombie prefabs to spawn randomly
    public Transform player;           // Reference to the player or AR camera
    public float spawnHeight = -1f;    // Height below the ground where zombies will spawn
    public float riseSpeed = 1f;       // Speed at which the zombie rises from the ground
    public float movementSpeed = 0.2f; // Speed of the zombie's movement
    public float spawnRadius = 10f;    // Radius around the player where zombies will spawn
    public GameObject mistPrefab;      // The mist particle prefab
    public float mistDuration = 5f;    // Duration of the mist effect
    public Transform[] graveSpawnPoints; // Array to hold predefined grave spawn points

    void Start()
    {
        if (player == null)
        {
            player = Camera.main.transform; // Default to the main camera if no player is assigned
        }

        // Ensure there are graves defined before trying to spawn
        if (graveSpawnPoints.Length > 0 && zombiePrefabs.Length > 0)
        {
            SpawnRandomZombies(); // Spawn multiple zombies randomly at start
        }
        else
        {
            Debug.LogError("No grave spawn points or zombie prefabs assigned in ZombieSpawner!");
        }
    }

    // Method to spawn random zombies at random grave spawn points
    public void SpawnRandomZombies()
    {
       //for (int i = 0; i < 2; i++)
        {
            // Pick a random zombie prefab from the array
            int zombieIndex = Random.Range(0, zombiePrefabs.Length);
            GameObject selectedZombiePrefab = zombiePrefabs[zombieIndex];

  
            // Pick a random grave from the array
            int graveIndex = Random.Range(0, graveSpawnPoints.Length);
            Vector3 spawnPosition = graveSpawnPoints[graveIndex].position;
            spawnPosition.y += spawnHeight; // Adjust height to the configured spawn height

            // Instantiate the zombie at the selected grave spawn point
            GameObject zombie = Instantiate(selectedZombiePrefab, spawnPosition, Quaternion.identity);

            // Optionally, spawn the mist particle effect at the zombie's spawn location
            if (mistPrefab != null)
            {
                //GameObject mist = Instantiate(mistPrefab, spawnPosition, Quaternion.identity);
                //Destroy(mist, mistDuration); // Destroy mist after duration
            }

            // Configure the zombie's BoxCollider (if it exists)
            BoxCollider zombieCollider = zombie.GetComponent<BoxCollider>();

            if (zombieCollider != null)
            {
                Debug.Log("BoxCollider found on the zombie prefab!");

                // Set the collider as a trigger to avoid physical collision but still detect overlaps
                zombieCollider.isTrigger = true;

                // Adjust the collider size and position if needed
                float zombieFullHeight = 3f; // Assuming 3 meters as a placeholder for zombie height

                // Adjust the size of the collider
                zombieCollider.size = new Vector3(zombieCollider.size.x, zombieFullHeight, zombieCollider.size.z);

                // Adjust the center of the collider based on height
                zombieCollider.center = new Vector3(zombieCollider.center.x, zombieFullHeight / 2, zombieCollider.center.z);
            }
            else
            {
                Debug.LogError("No BoxCollider found on the zombie prefab!");
            }

            // Optionally, link the ZombieHealth to the spawner for future spawns
            ZombieHealth zombieHealth = zombie.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.zombieSpawner = this.gameObject;
            }
        }
    }
}
