using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;  // Make sure you have the NavMesh components

public class ZombieMovement : MonoBehaviour
{
    public Transform player; // Assign the player (or camera) in the Inspector
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    [SerializeField] private float walkSpeed = 0.2f; // Speed at which the zombie walks

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
            navMeshAgent.SetDestination(player.position);

            // Update the animator's speed parameter based on the agent's velocity
            // The speed parameter controls the walking animation
            SetSpeed(navMeshAgent.velocity.magnitude);
        }
    }

    public void SetSpeed(float speed)
    {
        // The speed parameter controls the walking animation
        animator.SetFloat("Speed", speed);
    }

}
