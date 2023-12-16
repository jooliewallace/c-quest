using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationAndMovementController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int WalkHash;
    int RunHash;
    int JumpHash;

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    float rotationFactorPerFrame = 10.0f;
    float runMultiplier = 3.0f;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        WalkHash = Animator.StringToHash("Walk");
        RunHash = Animator.StringToHash("Run");
        JumpHash = Animator.StringToHash("Jump");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();

    }

    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();

    }

    void handleRotation()
    {
        Vector3 positionToLookAt;

        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            //creates a new rotation based on where the player is currently pressing
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = -currentMovementInput.x; // Invert x-axis input
        currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = -currentMovementInput.x * runMultiplier; // Invert x-axis input for running
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }


    void handleAnimation()
    {
        bool Walk = animator.GetBool("Walk");


        Debug.Log("isRunPressed: " + isRunPressed);
        Debug.Log("isMovementPressed: " + isMovementPressed);

        animator.SetBool("Walk", isMovementPressed);
        animator.SetBool("Run", isRunPressed && isMovementPressed);
        animator.SetBool("Jump", isJumpPressed && isMovementPressed);

    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            float groundedGravity = -.05f;
            currentMovement.y = groundedGravity;
            currentRunMovement.y = groundedGravity;
        }
        else
        {
            float gravity = -9.8f;
            currentMovement.y += gravity;
            currentRunMovement.y += gravity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleGravity();
        handleRotation();
        handleAnimation();

        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
        }

        if (isJumpPressed && characterController.isGrounded)
            Debug.Log("Jump pressed: " + isJumpPressed);

        {
            // Apply a vertical force to make the character jump
            Debug.Log("Jumping!");
            float jumpForce = 8.0f;
            currentMovement.y = jumpForce;
            isJumpPressed = false; // Reset the jump flag
        }
    }


    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }
}
