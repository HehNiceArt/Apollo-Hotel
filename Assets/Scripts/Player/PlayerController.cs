using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15;
    public float jumpForce = 10f;
    public float jumpHeight = 10f;
    public bool isGrounded;
    [SerializeField] GravityAttractor attractor;
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
        isGrounded = false;
        horizontalInput = 0;
        Vector3 gravityUp = (transform.position - attractor.transform.position).normalized;
        Vector3 jumpDirection = gravityUp * jumpHeight + transform.forward * jumpForce;
        rb.velocity = jumpDirection;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}