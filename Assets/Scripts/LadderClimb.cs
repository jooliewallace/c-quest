using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    [SerializeField]
    private LayerMask ladderLayer;

    private bool isClimbing;
    private Transform currentLadder;
    private float climbSpeed = 3f;

    void Update()
    {
        if (isClimbing)
        {
            ClimbLadder();
        }
        else
        {
            CheckForLadder();
        }
    }

    void CheckForLadder()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) // Check for ladder above
        {
            StartClimbing(hit.collider.transform);
        }
        else if (Physics.Raycast(transform.position, Vector3.up, out hit, 1.5f)) // Check for ladder below
        {
            StartClimbing(hit.collider.transform);
        }
    }



    void StartClimbing(Transform ladder)
    {
        isClimbing = true;
        currentLadder = ladder;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Animator>().SetBool("isClimbing", true);
    }

    void ClimbLadder()
    {
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 climbDirection = new Vector3(0f, verticalInput, 0f).normalized;
        Vector3 climbVelocity = climbDirection * climbSpeed;

        GetComponent<CharacterController>().Move(climbVelocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            StopClimbing();
        }
    }

    void StopClimbing()
    {
        isClimbing = false;
        currentLadder = null;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<Animator>().SetBool("isClimbing", false);
    }
}
