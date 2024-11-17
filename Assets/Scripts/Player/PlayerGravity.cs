using System;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] float surfaceDetectionBuffer = 0.1f;
    public SurfaceGravity currentSurface;
    bool isInSurfaceArea;
    float lastSurfaceChangeTime;
    [HideInInspector] public Rigidbody rb;
    Vector3 currentUp;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
        lastSurfaceChangeTime = -surfaceDetectionBuffer;
        currentUp = Vector3.up;
    }
    public void SetCurrentSurface(SurfaceGravity surface, bool isInside)
    {
        if (Time.time - lastSurfaceChangeTime >= surfaceDetectionBuffer)
        {
            if (isInside && surface != null)
            {
                if (isInSurfaceArea && currentSurface != null && currentSurface != surface)
                {
                    return;
                }
                currentSurface = surface;
                isInSurfaceArea = true;
            }
            else
            {
                currentSurface = null;
                isInSurfaceArea = false;
            }
            lastSurfaceChangeTime = Time.time;
        }
    }
    public SurfaceGravity GetCurrentSurface()
    {
        return currentSurface;
    }
    void UpdateOrientation()
    {
        if (currentSurface != null)
        {
            Vector3 gravityDirection = currentSurface.GetGravityDirection();
            transform.rotation = Quaternion.FromToRotation(-gravityDirection, gravityDirection) * Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }
}