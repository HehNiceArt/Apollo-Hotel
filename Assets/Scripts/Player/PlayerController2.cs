using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    SurfaceGravity currentSurface;
    bool isInSurfaceArea;
    [HideInInspector] public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }
    private void Update()
    {
        if (isInSurfaceArea && currentSurface != null)
        {
            UpdatePlayerOrientation();
        }
    }
    public void SetCurrentSurface(SurfaceGravity surface, bool isInside)
    {
        currentSurface = isInside ? surface : null;
        isInSurfaceArea = isInside;
    }
    void UpdatePlayerOrientation()
    {
        Transform surfaceTransform = currentSurface.transform;
        Quaternion targetRotation = Quaternion.LookRotation(surfaceTransform.forward, -currentSurface.GetGravityDirection());
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        Debug.Log(targetRotation);
    }
    public float HorizontalInput()
    {
        return Input.GetAxis("Horizontal");
    }
}