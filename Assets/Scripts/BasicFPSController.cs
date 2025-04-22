using UnityEngine;
using UnityEngine.InputSystem;

public class BasicFPSController : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    public Transform cameraTransform; // The camera that handles the look
    public float mouseSensitivity = 1f;
    public float moveSpeed = 5f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private Vector2 moveInput;
    private float verticalLook = 0f;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        lookInput = inputActions.PCPLayer.Look.ReadValue<Vector2>();
        moveInput = inputActions.PCPLayer.Move.ReadValue<Vector2>();

        Debug.Log("Mouse X: " + lookInput.x);

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        verticalLook -= mouseY;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalLook, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        Vector3 move = cameraTransform.right * moveInput.x + cameraTransform.forward * moveInput.y;

        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
