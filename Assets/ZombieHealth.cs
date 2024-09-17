using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public GameObject zombieSpawner; // Reference to the ZombieSpawner
    private AudioSource audioSource; // Reference to the zombie's AudioSource

    public float health = 100f;
    private Animator animator;
    private bool isDead = false;

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
            spawner.SpawnZombie();
        }

    }
}
