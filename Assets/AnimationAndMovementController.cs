using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    CharacterController characterController;
    Animator animator;

    int isWalkingHash;
    int isRunningHash;
    int isJumpingHash;

    // Variables to store player input values
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    Vector3 currentRunMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isJumpPressed;
    float rotationFactorPerFrame = 15.0f;
    [SerializeField] public float walkMultiplier = 2.0f; // Adjust this value to make the character walk faster
    [SerializeField] public float runMultiplier = 50.0f;
    [SerializeField] public float jumpForce = 5.0f;
    [SerializeField] public float gravityMultiplier = 2.0f;
    private float ySpeed;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.performed += onJump;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void onJump(InputAction.CallbackContext context)
    {
        if (context.performed && characterController.isGrounded)
        {
            isJumpPressed = true;
        }
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
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * walkMultiplier;
        currentMovement.z = currentMovementInput.y * walkMultiplier;
        currentRunMovement.x = currentMovementInput.x * runMultiplier;
        currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void handleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        // Start walking if movement pressed is true and not already walking
        if (isMovementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }
        // Stop walking if isMovementPressed is false and not already walking
        else if (!isMovementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((isMovementPressed && isRunPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }
        else if ((!isMovementPressed || !isRunPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        // Trigger jump animation
        animator.SetBool(isJumpingHash, isJumpPressed && characterController.isGrounded);
    }

    void handleGravity()
    {
        if (characterController.isGrounded)
        {
            ySpeed = -0.5f;
            isJumpPressed = false;
        }
        else
        {
            ySpeed += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            ySpeed = Mathf.Max(ySpeed, -jumpForce);
        }

        currentMovement.y = ySpeed;
        currentRunMovement.y = ySpeed;
    }

    void Update()
    {
        handleRotation();
        handleAnimation();
        handleGravity();

        // Apply jump force
        if (isJumpPressed && characterController.isGrounded)
        {
            characterController.Move(Vector3.up * jumpForce * Time.deltaTime);
        }

        // Move the character
        if (isRunPressed)
        {
            characterController.Move(currentRunMovement * Time.deltaTime);
        }
        else
        {
            characterController.Move(currentMovement * Time.deltaTime);
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
