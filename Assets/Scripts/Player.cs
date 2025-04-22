using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public bool canMove = false;

    [SerializeField] private float movSpeed = 10f;
    [SerializeField] private float gravityMagnitude = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputManagerSO inputManager;

    private CharacterController characterController;
    private Animator characterAnimator;

    [Header("Ground Check")]
    [SerializeField] private Transform feetTransform;
    [SerializeField] private float groundCheckRadius = 10f;
    [SerializeField] private LayerMask groundMask;

    private Vector3 movementDirection;
    private Vector3 inputDirection;
    private Vector3 verticalVelocity;

    private bool isGrounded = true;

    private void OnEnable()
    {
        inputManager.OnMove += Move;
        inputManager.OnJump += Jump;
    }

    private void Move(Vector2 context)
    {
        inputDirection = new Vector3(context.x, 0, context.y);
    }

    private void Jump()
    {
        if (isGrounded && canMove)
        {
            verticalVelocity.y = Mathf.Sqrt(-2 * gravityMagnitude * jumpHeight);
            characterAnimator.SetTrigger("Jump");
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
        canMove = false;
    }

    void Update()
    {
        if (!canMove)
        {
            characterController.velocity.Set(0f,0f,0f);
            characterAnimator.SetFloat("MovSpeed", characterController.velocity.magnitude);
            return;
        }


        movementDirection = cameraTransform.forward * inputDirection.z + cameraTransform.right * inputDirection.x;
        movementDirection = new Vector3 (movementDirection.x, 0, movementDirection.z);

        
        characterController.Move(movementDirection * movSpeed * Time.deltaTime);
        characterAnimator.SetFloat("MovSpeed", characterController.velocity.magnitude);


        if (movementDirection != Vector3.zero)
            RotateToDestiny();

        ApplyGravity();
        IsGrounded();

        characterAnimator.SetBool("isGrounded", isGrounded);

        if (isGrounded && verticalVelocity.y < 0)
            verticalVelocity = Vector3.zero;
    }



    private void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(feetTransform.position, groundCheckRadius, groundMask);
    }

    private void RotateToDestiny()
    {
        Quaternion objectRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = new Quaternion(0, objectRotation.y, 0, objectRotation.w);
    }

    private void ApplyGravity()
    {
        verticalVelocity.y += gravityMagnitude * Time.deltaTime;
        characterController.Move(verticalVelocity * Time.deltaTime);
        print(verticalVelocity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(feetTransform.position, groundCheckRadius);
    }

    private void OnDisable()
    {
        inputManager.OnMove -= Move;
        inputManager.OnJump -= Jump;
    }
}
