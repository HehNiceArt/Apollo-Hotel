using UnityEngine;

public class GravitySphere : MonoBehaviour
{
    [Header("Gravity Settings")]
    [SerializeField] private float gravityStrength = 20f;
    [SerializeField] private float radius = 10f;
    [SerializeField] private bool isPlayerInsideSurfaceGravity = false;

    void FixedUpdate()
    {
        if (!isPlayerInsideSurfaceGravity)
        {
            ApplyGravity();
        }
    }

    void ApplyGravity()
    {
        // Find all objects within the sphere's radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calculate direction to center
                    Vector3 directionToCenter = (transform.position - rb.position).normalized;

                    // Calculate distance for force scaling
                    float distance = Vector3.Distance(transform.position, rb.position);

                    // Apply gravity force
                    rb.AddForce(directionToCenter * gravityStrength, ForceMode.Acceleration);
                }
            }
        }
    }

    public void SetPlayerInsideSurfaceGravity(bool isInside)
    {
        isPlayerInsideSurfaceGravity = isInside;
        Debug.Log($"Player inside surface gravity: {isInside}");
    }

    private void OnDrawGizmos()
    {
        // Visualize the gravity sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}