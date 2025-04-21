using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float movSpeed = 10f;
    [SerializeField] private Transform cameraTransform;

    private float inputV;
    private float inputH;

    private CharacterController characterController;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        Vector3 movementDirection = (cameraTransform.forward * inputV + cameraTransform.right * inputH).normalized;

        characterController.Move(movementDirection * movSpeed * Time.deltaTime);
        
        if(inputH != 0 || inputV != 0)
            RotateToDestiny(movementDirection);
    }
    
    private void RotateToDestiny(Vector3 movementDirection)
    {
        Quaternion objectRotation = Quaternion.LookRotation(movementDirection);
        transform.rotation = objectRotation;
    }
}
