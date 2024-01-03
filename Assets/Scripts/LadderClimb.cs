using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    private bool isClimbing = false;
    private CharacterController characterController;
    private Animator animator;

    public float climbSpeed = 2f;
    public float raycastDistance = 1.5f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isClimbing)
        {
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbDirection = new Vector3(0f, verticalInput, 0f);

            // Disable vertical movement to simulate climbing (ignore gravity)
            climbDirection.y = 0;

            // Move the character
            characterController.Move(climbDirection * climbSpeed * Time.deltaTime);

            // Play climbing animation based on the input
            if (verticalInput != 0)
            {
                animator.SetBool("isClimbing", true);
                animator.SetFloat("ClimbSpeed", verticalInput);
            }
            else
            {
                animator.SetBool("isClimbing", false);
            }
        }
    }

    void FixedUpdate()
    {
        // Raycast to check if a ladder is in front of the player
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        Vector3 raycastDirection = transform.forward + Vector3.up * 0.1f;

        if (Physics.Raycast(raycastOrigin, raycastDirection, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Ladder"))
            {
                isClimbing = true;
                // Reset vertical velocity to avoid interference with climbing
                characterController.Move(Vector3.zero);
                Debug.Log("Player is climbing the ladder");
            }
            else
            {
                isClimbing = false;
                // Reset climbing animation parameters
                animator.SetBool("isClimbing", false);
                Debug.Log("Player is not on a ladder");
            }
        }
        else
        {
            isClimbing = false;
            // Reset climbing animation parameters
            animator.SetBool("isClimbing", false);
            Debug.Log("Player is not detecting a ladder in front");
        }

        // Debug Raycast direction
        Debug.DrawRay(raycastOrigin, raycastDirection * raycastDistance, Color.red);
    }
}
