using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BananaAnimation : MonoBehaviour
{
    Animator animator;
    Rigidbody rb; // Assuming you have a Rigidbody component for movement detection
    public float movementThreshold = 0.1f; // Minimum speed to consider as "moving"
    FieldOfView fieldOfView;
    NavMeshAgent navMeshAgent;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
    }

    private void FixedUpdate()
    {
        // Check if the object is moving based on its velocity
        // Check if the object is moving based on its velocity
        bool isMoving = navMeshAgent.velocity.magnitude > movementThreshold;

        // Check if the agent has reached its destination or is within stopping distance
        bool hasReachedDestination = navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance;

        if (isMoving && !hasReachedDestination)
        {
            // Object is moving and hasn't reached the stopping distance
            if (IsRunning())
            {
                Run();
            }
            else
            {
                Walk();
            }
        }
        else
        {
            // Object is not moving or has reached the stopping distance
            Idle();
        }
    }

    void Idle()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Run", false);
    }

    public void Walk()
    {
        animator.SetBool("Move", true);
        animator.SetBool("Run", false); // Ensure "Run" is false when "Walk" is true
    }

    public void Run()
    {
        animator.SetBool("Move", false);
        animator.SetBool("Run", true);
    }

    private bool IsRunning()
    {
        return fieldOfView.canSeePlayer;
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
