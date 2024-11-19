using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = 9.81f;

    private SphereCollider gravitySphere;
    [SerializeField] float gravitySphereRadius = 20f;

    private void Awake()
    {
        gravitySphere = gameObject.AddComponent<SphereCollider>();
        gravitySphere.isTrigger = true;
    }
    private void Update()
    {
        gravitySphere.radius = gravitySphereRadius;
    }

    public void Attract(Transform body, Rigidbody rb, bool isJumping = false)
    {
        Vector3 up = (transform.position - rb.position).normalized;
        float attractionStrength = isJumping ? gravity * 0.5f : gravity;
        rb.AddForce(up * attractionStrength, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            Attract(other.transform, rb);
        }
    }
}
