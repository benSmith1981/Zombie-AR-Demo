using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Damage value

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision Entered with {collision.gameObject.name} at position {collision.contacts[0].point}");

        // Check if the bullet hit a Zombie
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log($"{collision.gameObject.name} collided with");

            // Get the Zombie component and apply damage
            ZombieHealth zombieHealth = collision.gameObject.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                zombieHealth.TakeDamage(damage);
            }

            // Optionally destroy the bullet
            Destroy(gameObject);
        }
    }



}

