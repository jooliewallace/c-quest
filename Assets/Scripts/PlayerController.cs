using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 1;

    [SerializeField]
    private float lookSensitivity = 500;

    [SerializeField]
    private float jumpHeight = 1;

    [SerializeField]
    private float gravity = 9.81f;

    private Vector2 moveVector;
    private Vector2 lookVector;
    private Vector3 rotation;
    private float verticalVelocity;

    private CharacterController characterController;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    public void onMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
        if (moveVector.magnitude > 0)
        {
            animator.SetBool("Run", true);
        }
        else
        {
            animator.SetBool("Run", false);
        }

    }

    private void Move()
    {

        verticalVelocity += -gravity * Time.deltaTime;
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0;
        }

        //Vector3 move = transform.right * moveVector.x + transform.forward * moveVector.y + transform.up * verticalVelocity;
        //characterController.Move(move * Time.deltaTime);

        Vector3 move = (transform.right * moveVector.x + transform.forward * moveVector.y + transform.up * verticalVelocity) * moveSpeed;
        characterController.Move(move * Time.deltaTime);

    }

    public void onLook(InputAction.CallbackContext context)
    {
        lookVector = context.ReadValue<Vector2>();
    }

    private void Rotate()
    {
        rotation.y += lookVector.x * lookSensitivity * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (characterController.isGrounded && context.performed)
        {
            Debug.Log("Jump button pressed and player is grounded.");

            Debug.Log("Starting Jump Animation");
            Jump();
        }
    }

    private void Jump()
    {
        Debug.Log("Jumping!");

        // Set the trigger to start or restart the jump animation
        animator.SetTrigger("JumpTrigger");

        verticalVelocity = Mathf.Sqrt(jumpHeight * gravity);
    }


}