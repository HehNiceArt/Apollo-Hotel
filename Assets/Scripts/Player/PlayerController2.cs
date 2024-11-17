using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float horizontalRotationSpeed = 100f;
    [SerializeField] float surfaceDetectionBuffer = 0.1f;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;

    SurfaceGravity currentSurface;
    bool isInSurfaceArea;
    public bool isGrounded = true;
    float lastSurfaceChangeTime;
    [HideInInspector] public Rigidbody rb;
    Vector3 currentUp;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = false;
        lastSurfaceChangeTime = -surfaceDetectionBuffer;
        currentUp = Vector3.up;
    }
    private void Update()
    {
        CheckGrounded();
        if (isInSurfaceArea && currentSurface != null)
        {
            UpdatePlayerOrientation();
            HandleRotationInput();
        }
        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            HandleJumpInput();
        }
    }
    public SurfaceGravity GetCurrentSurface()
    {
        return currentSurface;
    }
    public void SetCurrentSurface(SurfaceGravity surface, bool isInside)
    {
        if (Time.time - lastSurfaceChangeTime >= surfaceDetectionBuffer)
        {
            if (isInside && surface != null)
            {
                if (isInSurfaceArea && currentSurface != null && currentSurface != surface)
                {
                    return;
                }
                currentSurface = surface;
                isInSurfaceArea = true;
            }
            else
            {
                currentSurface = null;
                isInSurfaceArea = false;
            }
            lastSurfaceChangeTime = Time.time;
        }
    }
    void CheckGrounded()
    {
        Vector3 rayDir = currentSurface != null ? currentSurface.GetGravityDirection() : Vector3.down;
        RaycastHit hit;

        Vector3 rayStart = transform.position;
        Debug.DrawRay(rayStart, rayDir * groundCheckDistance, Color.red);
        isGrounded = Physics.Raycast(rayStart, rayDir, out hit, groundCheckDistance, groundLayer);
    }
    void UpdatePlayerOrientation()
    {
        if (currentSurface == null) return;

        Transform surfaceTransform = currentSurface.transform;
        Vector3 gravityDir = currentSurface.GetGravityDirection();
        Vector3 forwardDir = surfaceTransform.forward;
        switch (currentSurface.GetSurfaceFloor())
        {
            case surfaceFloor.Left:
            case surfaceFloor.Right:
                forwardDir = -Vector3.left;
                break;
            case surfaceFloor.Front:
            case surfaceFloor.Back:
                forwardDir = Vector3.forward;
                break;
            case surfaceFloor.Top:
            case surfaceFloor.Bottom:
                forwardDir = Vector3.forward;
                break;
        }
        Quaternion targetRotation = Quaternion.LookRotation(forwardDir, -gravityDir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void HandleRotationInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            Vector3 rotationAxis = -currentSurface.GetGravityDirection();
            Quaternion deltaRotation = Quaternion.AngleAxis(horizontalInput * horizontalRotationSpeed * Time.deltaTime, rotationAxis);

        }
    }

    void HandleJumpInput()
    {
        isGrounded = false;
        //Vector3 jumpDirection = transform.forward * jumpForce;
        //jumpDirection += new Vector3(jumpDirection.x, jumpDirection.y + jumpHeight, jumpDirection.z);
        Vector3 jumpDirection = -currentSurface.GetGravityDirection() * jumpHeight;
        jumpDirection += transform.forward * jumpForce;
        rb.velocity = jumpDirection;
    }
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            Vector3 gravityUp = -currentSurface.GetGravityDirection();
            Vector3 horizontalVel = Vector3.ProjectOnPlane(rb.velocity, gravityUp);
            if (horizontalVel.magnitude > 0.1f)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }
}