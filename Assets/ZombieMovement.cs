using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Make sure you have the NavMesh components

public class ZombieMovement : MonoBehaviour
{
    public Transform player; // Assign the player (or camera) in the Inspector
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    [SerializeField] private float walkSpeed = 0.2f;  // Speed at which the zombie walks
    [SerializeField] private float attackRange = 1.5f; // Distance within which the zombie will attack

    private bool isAttacking = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = Camera.main.transform; // Default to the main camera if no player is assigned
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component missing from the zombie.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component missing from the zombie.");
        }
    }

    void Update()
    {
        if (navMeshAgent != null && player != null)
        {
            // Calculate the distance between the zombie and the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= attackRange)
            {
                // If the zombie is within the attack range, stop the agent and trigger the attack
                if (!isAttacking)
                {
                    StartAttacking();
                }
            }
            else
            {
                // If the zombie is outside the attack range, move towards the player
                navMeshAgent.SetDestination(player.position);
                SetSpeed(navMeshAgent.velocity.magnitude);
                isAttacking = false; // Reset the attack state if it's moving again
                                     // Trigger the attack animation
                animator.SetBool("Attack", isAttacking);
            }
        }

    }

    public void SetSpeed(float speed)
    {
        // The speed parameter controls the walking animation
        animator.SetFloat("Speed", speed);
    }

    private void StartAttacking()
    {
        isAttacking = true;

        // Stop the NavMeshAgent from moving
        navMeshAgent.isStopped = true;

        // Trigger the attack animation
        animator.SetBool("Attack", isAttacking);

        // Optionally, reset the speed to 0 for the attack
        SetSpeed(0);
    }
}
