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
        rb.freezeRotation = false;
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

}