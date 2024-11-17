using System;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float rotationSpeed = 30f;
    [SerializeField] float horizontalRotationSpeed = 100f;
    [SerializeField] SurfaceGravity currentSurface;
    PlayerGravity playerGravity;
    [SerializeField] float groundCheckDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    public bool isGrounded = true;
    Rigidbody rb;
    public bool isInSurfaceArea;
    public GameObject parent;
    Quaternion targetRotation;
    Quaternion currentRotation;
    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        playerGravity = GetComponentInParent<PlayerGravity>();
        targetRotation = transform.rotation;
    }

    private void Update()
    {
        CheckGrounded();
        currentSurface = playerGravity.GetCurrentSurface();
        if (isInSurfaceArea && currentSurface != null)
        {
            HandleRotationInput();
            //UpdatePlayerOrientation();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            HandleJumpInput();
        }
    }

    void HandleRotationInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            Vector3 rotationAxis = -currentSurface.GetGravityDirection();
            transform.Rotate(rotationAxis, -horizontalInput * horizontalRotationSpeed * Time.deltaTime);
        }
    }
    void HandleJumpInput()
    {
        isGrounded = false;
        Vector3 jumpDirection = -currentSurface.GetGravityDirection() * jumpHeight;
        jumpDirection += transform.forward * jumpForce;
        rb.velocity = jumpDirection;
    }
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            try
            {
                Vector3 gravityUp = -currentSurface.GetGravityDirection();
                Vector3 horizontalVel = Vector3.ProjectOnPlane(rb.velocity, gravityUp);
                if (horizontalVel.magnitude > 0.1f)
                {
                    rb.velocity = Vector3.zero;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
    }
    public void UpdatePlayerOrientation()
    {
        if (currentSurface != null)
        {
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
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
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
}