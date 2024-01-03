using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    private bool isClimbing = false;
    private CharacterController characterController;
    public float climbSpeed = 2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            StartClimbing();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            StopClimbing();
        }
    }

    void StartClimbing()
    {
        isClimbing = true;
    }

    void StopClimbing()
    {
        isClimbing = false;
    }

    void Update()
    {
        if (isClimbing)
        {
            ClimbLadder();
        }
    }

    void ClimbLadder()
    {
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 climbDirection = new Vector3(0f, verticalInput, 0f);
        characterController.Move(climbDirection * climbSpeed * Time.deltaTime);
    }
}
