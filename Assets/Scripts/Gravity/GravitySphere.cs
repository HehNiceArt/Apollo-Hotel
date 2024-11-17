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
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                Rigidbody rb = collider.GetComponentInChildren<Rigidbody>();
                if (rb != null)
                {
                    Vector3 directionToCenter = (transform.position - rb.position).normalized;

                    float distance = Vector3.Distance(transform.position, rb.position);

                    rb.AddForce(directionToCenter * gravityStrength, ForceMode.Acceleration);
                }
            }
        }
    }

    public void SetPlayerInsideSurfaceGravity(bool isInside)
    {
        isPlayerInsideSurfaceGravity = isInside;
        //Debug.Log($"Player inside surface gravity: {isInside}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}