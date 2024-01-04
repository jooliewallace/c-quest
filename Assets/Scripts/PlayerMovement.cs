using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed;

    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private float jumpButtonGracePeriod;

    [SerializeField]
    private float jumpHorizontalSpeed;

    [SerializeField]
    private float movementSpeed;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float increasedGravity = 10f; // Adjust this value to increase the falling speed

    private Animator animator;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;

    private Vector3 movementDirection; // Declare at the class level

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            animator.SetBool("isGrounded", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                // Calculate the jump direction based on the current movement direction
                Vector3 jumpDirection = movementDirection.normalized;

                // Apply the jump force
                ySpeed = jumpSpeed;
                ySpeed += jumpDirection.y * jumpHorizontalSpeed; // Apply horizontal speed

                animator.SetBool("isJumping", true);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            animator.SetBool("isGrounded", false);
            isGrounded = false;

            if ((isJumping && ySpeed < 0) || ySpeed < -2)
            {
                animator.SetBool("isFalling", true);

                // Apply increased gravity only if not jumping
                if (!isJumping)
                {
                    ySpeed += increasedGravity * Time.deltaTime;
                }
            }
        }

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("isRunning", true);

            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            // Use Quaternion.Slerp for smooth rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Apply movement regardless of the grounded state
        Vector3 velocity = movementDirection * inputMagnitude * movementSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        if (!characterController.isGrounded)
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;

            Vector3 velocity = new Vector3(movementDirection.x, ySpeed, movementDirection.z) * movementSpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}



//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    [SerializeField] private float rotationSpeed = 5f;
//    [SerializeField] private float jumpSpeed = 8f;
//    [SerializeField] private float jumpButtonGracePeriod = 0.1f;
//    [SerializeField] private float jumpHorizontalSpeed = 1f;
//    [SerializeField] private float movementSpeed = 5f;
//    [SerializeField] private float increasedGravity = 10f;

//    private Animator animator;
//    private CharacterController characterController;
//    private float ySpeed;
//    private float originalStepOffset;
//    private float? lastGroundedTime;
//    private float? jumpButtonPressedTime;
//    private bool isJumping;
//    private bool isGrounded;

//    private Vector3 movementDirection;
//    private PlayerInput playerInput;

//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        characterController = GetComponent<CharacterController>();
//        originalStepOffset = characterController.stepOffset;

//        playerInput = new PlayerInput();
//        playerInput.Enable();

//        playerInput.CharacterControls.Move.performed += ctx => UpdateMovement(ctx.ReadValue<Vector2>());
//        playerInput.CharacterControls.Jump.performed += _ => Jump();
//    }


//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false;
//        }
//    }


//    void UpdateMovement(Vector2 input)
//    {
//        float horizontalInput = input.x;
//        float verticalInput = input.y;

//        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
//        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

//        if (Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed)
//        {
//            inputMagnitude /= 2;
//        }

//        animator.SetFloat("InputMagnitude", inputMagnitude, 0.05f, Time.deltaTime);

//        movementDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * movementDirection;
//        movementDirection.Normalize();
//    }

//    void Jump()
//    {
//        if (characterController.isGrounded)
//        {
//            lastGroundedTime = Time.time;
//            characterController.stepOffset = originalStepOffset;
//            ySpeed = jumpSpeed;

//            animator.SetBool("isGrounded", true);
//            isGrounded = true;
//            animator.SetBool("isJumping", true);
//            isJumping = true;
//            jumpButtonPressedTime = Time.time; // Set jumpButtonPressedTime when jump button is pressed
//            lastGroundedTime = null;
//        }
//    }

//    void Update()
//    {
//        isGrounded = characterController.isGrounded;

//        ySpeed += Physics.gravity.y * Time.deltaTime;

//        if (isGrounded && Time.time - lastGroundedTime <= jumpButtonGracePeriod)
//        {
//            characterController.stepOffset = originalStepOffset;
//            ySpeed = -0.5f;
//            animator.SetBool("isGrounded", true);
//            isGrounded = true;
//            animator.SetBool("isJumping", false);
//            isJumping = false;
//            animator.SetBool("isFalling", false);

//            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
//            {
//                ySpeed = jumpSpeed;
//                animator.SetBool("isJumping", true);
//                isJumping = true;
//                jumpButtonPressedTime = null;
//                lastGroundedTime = null;
//            }
//        }
//        else
//        {
//            characterController.stepOffset = 0;
//            animator.SetBool("isGrounded", false);
//            isGrounded = false;

//            if ((isJumping && ySpeed < 0) || ySpeed < -2)
//            {
//                animator.SetBool("isFalling", true);

//                if (!isJumping)
//                {
//                    ySpeed += increasedGravity * Time.deltaTime;
//                }
//            }
//        }

//        if (movementDirection != Vector3.zero)
//        {
//            animator.SetBool("isRunning", true);

//            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

//            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
//        }
//        else
//        {
//            animator.SetBool("isRunning", false);
//        }

//        Vector3 velocity = movementDirection * movementSpeed;
//        velocity.y = ySpeed;

//        characterController.Move(velocity * Time.deltaTime);
//    }

//    private void OnAnimatorMove()
//    {
//        if (!characterController.isGrounded)
//        {
//            Vector3 velocity = new Vector3(movementDirection.x, ySpeed, movementDirection.z) * movementSpeed * Time.deltaTime;

//            characterController.Move(velocity);
//        }
//    }

//    private void OnApplicationFocus(bool focus)
//    {
//        if (focus)
//        {
//            Cursor.lockState = CursorLockMode.Locked;
//        }
//        else
//        {
//            Cursor.lockState = CursorLockMode.None;
//        }
//    }

//    private void OnDestroy()
//    {
//        playerInput.Disable();
//    }
//}