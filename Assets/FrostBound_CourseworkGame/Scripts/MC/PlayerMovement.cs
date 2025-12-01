using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float acceleration = 10f;
    public float gravity = -20f;
    public float jumpHeight = 1.3f;

    [Header("Camera")]
    public Transform cameraHolder;
    public Camera playerCamera;   // used for FOV effect
    public float mouseSensitivity = 180f;
    public float verticalClamp = 85f;
    public float fovNormal = 70f;
    public float fovSprint = 80f;
    public float fovSpeed = 5f;

    public bool isSprinting { get; private set; }
    public CharacterController controller { get; private set; }

    private Vector3 velocity;
    private float currentSpeed;
    private float xRotation;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera != null)
            playerCamera.fieldOfView = fovNormal;
    }

    void Update()
    {
        HandleMovement();
        HandleCamera();
        HandleFOV();
    }

    void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.right * x + transform.forward * z).normalized;

        isSprinting = Input.GetKey(KeyCode.LeftShift) && z > 0;

        float targetSpeed = isSprinting ? sprintSpeed : walkSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed * direction.magnitude, acceleration * Time.deltaTime);

        controller.Move(direction * currentSpeed * Time.deltaTime);

        if (controller.isGrounded)
        {
            velocity.y = -2f;
            if (Input.GetButtonDown("Jump"))
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -verticalClamp, verticalClamp);
        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    void HandleFOV()
    {
        if (playerCamera == null) return;

        float targetFOV = isSprinting ? fovSprint : fovNormal;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * fovSpeed);
    }
}
