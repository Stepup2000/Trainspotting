using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A basic first-person controller handling movement and mouse look.
/// </summary>
public class BasicFPSController : MonoBehaviour
{
    [SerializeField] private CharacterController controller; // CharacterController for movement

    /// <summary>
    /// The camera transform used for vertical look rotation.
    /// </summary>
    public Transform cameraTransform;

    /// <summary>
    /// Mouse sensitivity multiplier.
    /// </summary>
    public float mouseSensitivity = 1f;

    /// <summary>
    /// Movement speed in units per second.
    /// </summary>
    public float moveSpeed = 5f;

    private PlayerInputActions inputActions; // Input system actions
    private Vector2 lookInput;               // Stores mouse input
    private Vector2 moveInput;               // Stores WASD input
    private float verticalLook = 0f;         // Vertical rotation angle

    /// <summary>
    /// Initializes the input actions.
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    /// <summary>
    /// Enables input when the script becomes active.
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();
    }

    /// <summary>
    /// Disables input when the script becomes inactive.
    /// </summary>
    private void OnDisable()
    {
        inputActions.Disable();
    }

    /// <summary>
    /// Locks the cursor at the start of the game.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Handles player input and movement every frame.
    /// </summary>
    private void Update()
    {
        lookInput = inputActions.PCPLayer.Look.ReadValue<Vector2>();
        moveInput = inputActions.PCPLayer.Move.ReadValue<Vector2>();

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        verticalLook -= mouseY;
        verticalLook = Mathf.Clamp(verticalLook, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalLook, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        Vector3 move = cameraTransform.right * moveInput.x + cameraTransform.forward * moveInput.y;
        move.y = 0f; // Prevent vertical movement
        move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);
    }
}
