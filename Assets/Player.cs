using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float movSpeed = 10f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private InputManagerSO inputManager;

    private CharacterController characterController;
    private Animator characterAnimator;


    private Vector3 movementDirection;
    private Vector3 inputDirection;

    private void OnEnable()
    {
        inputManager.OnMove += OnMove;
        inputManager.OnJump += OnJump;
    }

    private void OnMove(Vector2 context)
    {
        inputDirection = new Vector3(context.x, 0, context.y);
        RotateToDestiny();
    }

    private void OnJump()
    {
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
        print(movementDirection * movSpeed * Time.deltaTime);

        characterController.Move(movementDirection * movSpeed * Time.deltaTime);
        characterAnimator.SetFloat("MovSpeed", characterController.velocity.magnitude);
    }
    
    private void RotateToDestiny()
    {
        Quaternion objectRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = objectRotation;
    }
}
