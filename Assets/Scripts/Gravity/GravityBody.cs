using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    [HideInInspector] public Rigidbody rb;
    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;
    }
    private void Update()
    {
        attractor.Attract(transform, rb);
    }
}