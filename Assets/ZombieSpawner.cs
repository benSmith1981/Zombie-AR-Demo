using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Make sure you have the NavMesh components

public class ZombieSpawner : MonoBehaviour
{
    public GameObject zombiePrefab; // The zombie prefab to spawn
    public Transform player;        // Reference to the player or AR camera
    public float spawnHeight = -2f; // Height below the ground where zombies will spawn
    public float riseSpeed = 1f;    // Speed at which the zombie rises from the ground
    public float movementSpeed = 0.2f; // Speed of the zombie's movement
    public float spawnRadius = 10f; // Radius around the player where zombies will spawn

    void Start()
    {
        if (player == null)
        {
            player = Camera.main.transform; // Default to the main camera if no player is assigned
        }
        SpawnZombie();
    }

    public void SpawnZombie()
    {
        // Calculate a random position within a circular area around the player
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = player.position + new Vector3(randomCircle.x, -3f, randomCircle.y);

        GameObject zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();
        /*
        if (zombieMovement != null)
        {
            zombieMovement.SetSpeed(movementSpeed);
        }
        */
        StartCoroutine(TriggerRisingAnimation(zombie));
    }

    /*
    private IEnumerator RiseFromGround(GameObject zombie)
    {
        
        Vector3 startPosition = zombie.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * -spawnHeight;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            zombie.transform.position = Vector3.Lerp(endPosition, startPosition, elapsedTime);
            elapsedTime += Time.deltaTime * riseSpeed;
            yield return null;
        }

        zombie.transform.position = startPosition; // Ensure final position
        
        zombie.GetComponent<NavMeshAgent>().SetDestination(player.position);
    }
    */
    private IEnumerator TriggerRisingAnimation(GameObject zombie)
    {
        Animator animator = zombie.GetComponent<Animator>();
        if (animator != null)
        {
            // Assuming you have a climbing or rising animation state
           //animator.SetTrigger("RiseFromGround");
            animator.SetBool("RiseFromGround", true);
            // Optionally wait for the animation to finish or for a specified duration
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        // Set the zombie's destination to the player's position
        ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();
        if (zombieMovement != null)
        {
            zombieMovement.GetComponent<NavMeshAgent>().SetDestination(player.position);
        }
    }
}
