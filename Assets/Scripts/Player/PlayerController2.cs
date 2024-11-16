using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] SurfaceGravity[] surfaceGravity;
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }
    private void Update()
    {
    }
}