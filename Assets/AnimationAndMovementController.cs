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

    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;
    bool isRunPressed;
    bool isGrounded;

    float rotationFactorPerFrame = 15.0f;
    [SerializeField] float runMultiplier = 3.0f;
    [SerializeField] float jumpHeight = 1.0f;
    float verticalVelocity;
    private float jumpVelocity;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        jumpVelocity = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics.gravity.y));

        playerInput.CharacterControls.Move.started += OnMovementInput;
        playerInput.CharacterControls.Move.canceled += OnMovementInput;
        playerInput.CharacterControls.Move.performed += OnMovementInput;
        playerInput.CharacterControls.Run.started += OnRun;
        playerInput.CharacterControls.Run.canceled += OnRun;
        playerInput.CharacterControls.Jump.performed += OnJump;

        playerInput.Enable();
    }


    void OnRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            Jump();
        }
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt = new Vector3(currentMovement.x, 0.0f, currentMovement.z);
        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x;
        currentMovement.z = currentMovementInput.y;
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }

    void HandleAnimation()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool isJumping = animator.GetBool(isJumpingHash);

        if (isGrounded)
        {
            if (isMovementPressed && !isWalking)
            {
                animator.SetBool(isWalkingHash, true);
            }
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

            if (isJumping)
            {
                // Check if the normalized time of the animation is greater than or equal to 1
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
                {
                    animator.SetBool(isJumpingHash, false);
                    verticalVelocity = 0f; // Reset the vertical velocity to zero after the jump animation ends
                }
            }
            else
            {
                animator.SetBool(isJumpingHash, false);
            }
        }
        else
        {
            animator.SetBool(isJumpingHash, false);
        }
    }


    void Jump()
    {
        if (isGrounded)
        {
            jumpVelocity = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics.gravity.y));
            verticalVelocity = 0f;
            animator.SetBool(isJumpingHash, true);
            currentMovement.y = jumpVelocity;
        }
    }



    void HandleGravity()
    {
        isGrounded = characterController.isGrounded;

        if (!isGrounded)
        {
            // Apply gravity every frame
            verticalVelocity -= Physics.gravity.y * Time.deltaTime;

            // Limit the jump height
            if (verticalVelocity < -jumpHeight)
            {
                verticalVelocity = -jumpHeight;
            }

            currentMovement.y = verticalVelocity;
        }
        else
        {
            // Reset vertical velocity to 0 if grounded
            verticalVelocity = 0f;
        }
    }


    void Update()
    {
        HandleRotation();
        HandleAnimation();
        HandleGravity();

        Vector3 move = isRunPressed ? currentMovement * runMultiplier : currentMovement;
        characterController.Move(move * Time.deltaTime);
    }

    void OnDisable()
    {
        playerInput.Disable();
    }
}
