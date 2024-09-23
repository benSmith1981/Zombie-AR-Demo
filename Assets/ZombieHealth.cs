using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // Import AR Foundation for handling AR planes

public class ZombieHealth : MonoBehaviour
{
    public GameObject zombieSpawner; // Reference to the ZombieSpawner
    private AudioSource audioSource; // Reference to the zombie's AudioSource

    public float health = 100f;
    private Animator animator;
    private bool isDead = false;

        // Material to apply to the AR plane when the zombie hits it
    [SerializeField] private Material cutoutHoleMaterial;


    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        {
            TriggerDeath();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(50f); // Take damage when hit by a bullet (you can adjust this value)
        }
        else if (other.CompareTag("ARPlane"))
        {
            // When zombie collides with an AR wall, create a hole in the wall
            //CreateHoleInWall(other);
        }
    }

        // Create a hole in the AR wall when the zombie collides with it
    void CreateHoleInWall(Collider wallCollider)
    {
        // Get the ARPlaneMeshVisualizer to modify the wall's material
        ARPlaneMeshVisualizer meshVisualizer = wallCollider.GetComponent<ARPlaneMeshVisualizer>();
        if (meshVisualizer != null)
        {
            // Apply the cutout material (or transparent material) to create the hole
            MeshRenderer meshRenderer = meshVisualizer.GetComponent<MeshRenderer>();
            if (meshRenderer != null && cutoutHoleMaterial != null)
            {
                meshRenderer.material = cutoutHoleMaterial;

                // Optional: Display debug message
                Debug.Log("Zombie hit the AR wall and created a hole.");
            }
        }
    }


    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("death", true); // Trigger the death animation

            // Disable movement or set speed to zero
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
            // Stop the zombie's audio
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // Optionally, disable other zombie functionality here (attacks, etc.)

            // Call the respawn method in the ZombieSpawner
            StartCoroutine(RespawnZombie());
        }
    }

    // Respawn the zombie after death
    IEnumerator RespawnZombie()
    {
        // Wait for a short delay (for example, wait for death animation to complete)
        yield return new WaitForSeconds(2.0f);

        // Call the SpawnZombie method in ZombieSpawner to spawn a new zombie
        ZombieSpawner spawner = zombieSpawner.GetComponent<ZombieSpawner>();
        if (spawner != null)
        {
            spawner.SpawnRandomZombies();
        }

    }
}
