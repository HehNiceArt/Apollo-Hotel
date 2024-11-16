using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.Timeline;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Constants
    private const float GROUNDED_THRESHOLD = 0.1f;
    #endregion

    #region Movement Settings
    [Header("Movement")]
    [SerializeField, Range(0f, 360f)] private float playerSpeed = 180f;
    [SerializeField, Range(0f, 20f)] private float jumpForce = 5f;
    [SerializeField, Range(0f, 20f)] private float jumpHeight = 5f;
    [SerializeField, Range(0f, 50f)] private float rotationSpeed = 20f;
    #endregion

    #region Private Fields
    private Rigidbody rb;
    private Vector3 currentNormal = Vector3.up;
    private Vector3 gravityUp = Vector3.zero;
    private float rotationInput;

    [SerializeField] private bool isGrounded;
    [SerializeField] private SurfaceDetector surfaceDetector;
    private string currentSurface;
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        HandleRotation();
        HandleMovement();
    }
    #endregion

    #region Private Methods
    private void InitializeComponents()
    {
        if (TryGetComponent(out Rigidbody rigidbody))
        {
            rb = rigidbody;
            rb.useGravity = false;
            rb.freezeRotation = true;
        }
        else
        {
            Debug.LogError("Rigidbody component not found on PlayerController!");
        }
    }

    private void HandleInput()
    {
        rotationInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        isGrounded = false;
        Vector3 jumpForward = transform.forward * jumpForce;
        jumpForward += new Vector3(jumpForward.x, jumpForward.y + jumpHeight, jumpForward.z);
        rb.AddForce(jumpForward * jumpForce, ForceMode.Impulse);
    }

    private void ApplyGravity()
    {
        Vector3 gravity = CustomGravity.GetGravity(rb.position);
        gravityUp = -gravity.normalized;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    private void HandleRotation()
    {
        Vector3 targetUp = isGrounded ?
            Vector3.Lerp(gravityUp, currentNormal, 0.5f) :
            gravityUp;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, targetUp);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * transform.rotation,
            rotationSpeed * Time.fixedDeltaTime);

        if (isGrounded)
        {
            float surfaceAngle = Vector3.Angle(currentNormal, gravityUp);
            float stickForce = Mathf.Lerp(20f, 50f, surfaceAngle / 90f);
            rb.AddForce(-currentNormal * stickForce, ForceMode.Acceleration);
        }
    }

    private void HandleMovement()
    {
        if (Mathf.Abs(rotationInput) > 0.1f)
        {
            Vector3 rotationAxis;
            float adjustedInput = rotationInput;

            if (isGrounded)
            {
                // Get the world up vector (opposite of gravity)
                Vector3 worldUp = -gravityUp;

                // Calculate the forward direction along the surface
                Vector3 surfaceForward = Vector3.Cross(currentNormal, transform.right).normalized;

                // Calculate the right direction along the surface
                Vector3 surfaceRight = Vector3.Cross(currentNormal, surfaceForward).normalized;

                // Use the surface normal as rotation axis
                rotationAxis = currentNormal;

                // Calculate the angle between current normal and world up
                float angle = Vector3.Angle(currentNormal, worldUp);

                // Determine if we're on a vertical surface (walls)
                bool isVerticalSurface = angle > 45f && angle < 135f;

                // Adjust input based on surface orientation
                if (isVerticalSurface)
                {
                    // For walls, check if we need to invert based on which way we're facing
                    float rightDot = Vector3.Dot(surfaceRight, worldUp);
                    if (rightDot > 0)
                    {
                        adjustedInput *= -1;
                    }
                }
                else
                {
                    // For floor/ceiling, check if we're inverted
                    float upDot = Vector3.Dot(currentNormal, worldUp);
                    if (upDot < 0)
                    {
                        adjustedInput *= -1;
                    }
                }
            }
            else
            {
                rotationAxis = transform.up;
            }

            Quaternion rotation = Quaternion.AngleAxis(
                adjustedInput * playerSpeed * Time.fixedDeltaTime,
                rotationAxis
            );
            rb.MoveRotation(rb.rotation * rotation);
        }
    }
    #endregion

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = false;
        foreach (ContactPoint contact in collision.contacts)
        {
            float dot = Vector3.Dot(contact.normal, transform.up);
            if (dot > GROUNDED_THRESHOLD)
            {
                isGrounded = true;
                currentNormal = contact.normal;
                currentSurface = surfaceDetector.WhichSurface(collision);
                break;
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, currentNormal * 2f);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, gravityUp * 2f);
        }
    }
}
