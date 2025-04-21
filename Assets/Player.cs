using UnityEngine;

public class Player : MonoBehaviour
{

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
        if (IsGrounded())
        {
            verticalVelocity.y = Mathf.Sqrt(-2 * gravityMagnitude * jumpHeight);
            //characterAnimator.SetTrigger("Jump");
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        characterAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        movementDirection = cameraTransform.forward * inputDirection.z + cameraTransform.right * inputDirection.x;

        characterController.Move(movementDirection * movSpeed * Time.deltaTime);
        characterAnimator.SetFloat("MovSpeed", characterController.velocity.magnitude);

        if (movementDirection.sqrMagnitude > 0)
            RotateToDestiny();

        if(IsGrounded() && verticalVelocity.y < 0)
            verticalVelocity = Vector3.zero;
        
        ApplyGravity();
    }



    private bool IsGrounded()
    {
        return Physics.CheckSphere(feetTransform.position, groundCheckRadius, groundMask);
    }

    private void RotateToDestiny()
    {
        Quaternion objectRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = objectRotation;
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
}
