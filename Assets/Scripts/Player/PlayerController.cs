using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15;
    public float jumpForce = 10f;
    public float jumpHeight = 10f;
    public bool isGrounded;
    public SurfaceTypes surfaceTypes;
    public GravityAttractor attractor;
    Rigidbody rb;
    float horizontalInput;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        if (horizontalInput != 0 && isGrounded)
        {
            float rotationAmount = horizontalInput * moveSpeed * Time.fixedDeltaTime;
            transform.Rotate(0, rotationAmount, 0);
        }
        else if (horizontalInput == 0)
        {
            transform.Rotate(0, 0, 0);
        }
    }
    void Jump()
    {
        if (surfaceTypes == SurfaceTypes.Sphere)
        {
            isGrounded = false;
            jumpForce = 10;
            jumpHeight = 20;
            horizontalInput = 0;
            Vector3 gravityUp = (transform.position - attractor.transform.position).normalized;
            Vector3 jumpDirection = gravityUp * jumpHeight + transform.forward * jumpForce;
            rb.velocity = jumpDirection;
        }
        else if (surfaceTypes == SurfaceTypes.Plane)
        {
            isGrounded = false;
            jumpForce = 3;
            jumpHeight = 10;
            horizontalInput = 0;
            Vector3 jumpDir = Vector3.up * jumpHeight + transform.forward * jumpHeight;
            rb.AddForce(jumpDir, ForceMode.Impulse);
        }
    }
    public GravityAttractor ReturnGravityAttractor()
    {
        Debug.Log("returning");
        return attractor;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            attractor = other.gameObject.GetComponent<GravityAttractor>();
        }
    }
}