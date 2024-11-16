using UnityEngine;

public class GravitySphere : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public float radius = 10f;
    public float height = 10f;
    [SerializeField] bool isPlayerInsideSurfaceGravity = false;

    void FixedUpdate()
    {
        if (!isPlayerInsideSurfaceGravity)
        {
            ApplyGravity();
            Debug.Log("Applying gravity");
        }
    }

    void ApplyGravity()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        Vector3 left = transform.position + Vector3.left * (height / 2);
        Vector3 right = transform.position + Vector3.right * (height / 2);
        foreach (var collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 closestPoint = collider.ClosestPointOnBounds(transform.position);
                Vector3 direction = (transform.position - closestPoint).normalized;
                float distance = Vector3.Distance(transform.position, rb.position);
                float force = -gravityStrength / (distance * distance);
                rb.AddForce(direction * force);
            }
        }
    }

    public void SetPlayerInsideSurfaceGravity(bool isInside)
    {
        isPlayerInsideSurfaceGravity = isInside;
    }
    private void OnDrawGizmos()
    {
        Vector3 left = transform.position + Vector3.left * (height / 2);
        Vector3 right = transform.position + Vector3.right * (height / 2);

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(left, radius);
        Gizmos.DrawWireSphere(right, radius);

        Gizmos.DrawLine(left + Vector3.up * radius, right + Vector3.up * radius);
        Gizmos.DrawLine(left - Vector3.up * radius, right - Vector3.up * radius);
        Gizmos.DrawLine(left + Vector3.forward * radius, right + Vector3.forward * radius);
        Gizmos.DrawLine(left - Vector3.forward * radius, right - Vector3.forward * radius);

    }
}