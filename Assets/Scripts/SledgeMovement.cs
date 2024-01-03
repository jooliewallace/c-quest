using System.Collections;
using UnityEngine;

public class SledgeMovement : MonoBehaviour
{
    public Transform[] waypoints; // Define waypoints for the sledge to follow
    public float moveSpeed = 2f;
    public float waitTime = 2f; // Adjust the wait time as needed
    public Transform playerAttachmentPoint; // Create an empty GameObject and assign it here
    private Transform player;
    private Rigidbody sledRigidbody;
    private FixedJoint playerJoint; // Added FixedJoint to connect player to sled

    private int currentWaypointIndex = 0;
    private bool isPlayerAttached = false;

    void Start()
    {
        sledRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(MoveSledgeWithDelay());
    }

    IEnumerator MoveSledgeWithDelay()
    {
        while (true)
        {
            // Wait for the player to attach
            while (!isPlayerAttached)
            {
                yield return null;
            }

            yield return StartCoroutine(MoveToWaypoint());
            yield return new WaitForSeconds(waitTime);
        }
    }

    IEnumerator MoveToWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints defined for the sledge!");
            yield break;
        }

        Vector3 targetPosition = waypoints[currentWaypointIndex].position;

        // Move sled to waypoint
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Set the velocity to move the sled
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            sledRigidbody.velocity = moveDirection * moveSpeed;

            yield return null;
        }

        // Stop the sled when reaching the waypoint
        sledRigidbody.velocity = Vector3.zero;

        // Move to the next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

        // Wait for a short time before moving to the next waypoint
        yield return new WaitForSeconds(0.5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && playerJoint == null)
        {
            // Attach the player to the sled using FixedJoint
            player = other.transform;
            playerJoint = player.gameObject.AddComponent<FixedJoint>();
            playerJoint.connectedBody = sledRigidbody;
            playerJoint.breakForce = Mathf.Infinity; // Adjust breakForce as needed
            isPlayerAttached = true;
        }
    }
}
