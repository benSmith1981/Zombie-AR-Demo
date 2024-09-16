using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health = 100f;
    private Animator animator;
    private bool isDead = false;
    private void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
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
            TriggerDeath();
        }
    }
    public void TriggerDeath()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("death", true); // Trigger the death animation
                                             // Optionally, you can disable the zombie's movement or interactions here
                                             // Optionally, disable the Zombie or set it inactive
            
            //GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false; // Stop movement
            
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 0;
        }
    }
}
