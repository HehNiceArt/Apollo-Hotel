using Unity.VisualScripting;
using UnityEngine;

public class SurfaceDetector : MonoBehaviour
{
    [SerializeField] private Collider[] surfaceColliders;
    private string currentSurfaceName;

    public string WhichSurface(Collision collision)
    {
        if (surfaceColliders != null && collision != null)
        {
            foreach (var contact in collision.contacts)
            {
                foreach (var surfaceCollider in surfaceColliders)
                {
                    if (contact.otherCollider == surfaceCollider)
                    {
                        currentSurfaceName = surfaceCollider.gameObject.name;
                        return currentSurfaceName;
                    }
                }
            }
        }
        return string.Empty;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (!string.IsNullOrEmpty(currentSurfaceName))
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.yellow;
            UnityEditor.Handles.Label(transform.position + Vector3.up * 2f, $"Surface: {currentSurfaceName}", style);
        }
    }
}
