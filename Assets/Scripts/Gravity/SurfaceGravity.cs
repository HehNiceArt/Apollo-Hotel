using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum surfaceFloor
{
    Front,
    Back,
    Left,
    Right,
    Top,
    Bottom
}
public class SurfaceGravity : MonoBehaviour
{
    [SerializeField] surfaceFloor surfaceFloor;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] Vector3 surfaceArea = new Vector3(1, 1, 1);
    [SerializeField] Vector3 gravityDirection = Vector3.down;
    BoxCollider boxCollider;
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.center = Vector3.zero;
        if (surfaceFloor == surfaceFloor.Top || surfaceFloor == surfaceFloor.Bottom)
        {
            boxCollider.size = new Vector3(0.15f, 1, 1);
        }
        else
        {
            boxCollider.size = surfaceArea;
        }
        switch (surfaceFloor)
        {
            case surfaceFloor.Front:
                gravityDirection = new Vector3(0, -1, 0);
                break;
            case surfaceFloor.Back:
                gravityDirection = new Vector3(0, 1, 0);
                break;
            case surfaceFloor.Left:
                gravityDirection = new Vector3(0, 0, 1);
                break;
            case surfaceFloor.Right:
                gravityDirection = new Vector3(0, 0, -1);
                break;
            case surfaceFloor.Top:
                gravityDirection = new Vector3(1, 0, 0);
                break;
            case surfaceFloor.Bottom:
                gravityDirection = new Vector3(-1, 0, 0);
                break;
            default:
                gravityDirection = new Vector3(0, -1, 0);
                break;
        }
    }
    public Vector3 GetGravityDirection()
    {
        return gravityDirection;
    }
    public surfaceFloor GetSurfaceFloor()
    {
        return surfaceFloor;
    }
    private void OnDrawGizmos()
    {
        switch (surfaceFloor)
        {
            case surfaceFloor.Front:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            case surfaceFloor.Back:
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            case surfaceFloor.Left:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            case surfaceFloor.Right:
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            case surfaceFloor.Top:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            case surfaceFloor.Bottom:
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(transform.position, surfaceArea);
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGravity playerGravity = other.GetComponentInParent<PlayerGravity>();
            if (playerGravity != null)
            {
                playerGravity.SetCurrentSurface(this, true);
                PlayerController2 playerController = other.GetComponent<PlayerController2>();
                playerController.UpdatePlayerOrientation();
            }
            GravitySphere gravitySphere = FindObjectOfType<GravitySphere>();
            gravitySphere?.SetPlayerInsideSurfaceGravity(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            PlayerController2 playerController = other.GetComponent<PlayerController2>();
            if (rb != null)
            {
                playerController.isInSurfaceArea = true;
                playerController.UpdatePlayerOrientation();
                Vector3 gravityForce = gravity * gravityDirection.normalized;
                rb.AddForce(gravityForce, ForceMode.Acceleration);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerGravity playerGravity = other.GetComponentInParent<PlayerGravity>();
            PlayerController2 playerController = other.GetComponent<PlayerController2>();
            if (playerGravity != null)
            {
                if (playerGravity.GetCurrentSurface() == this)
                {
                    playerGravity.SetCurrentSurface(this, false);
                }
                playerController.isInSurfaceArea = false;
                GravitySphere gravitySphere = FindObjectOfType<GravitySphere>();

                gravitySphere?.SetPlayerInsideSurfaceGravity(false);
                playerController.UpdatePlayerOrientation();
            }
        }
    }
    public SurfaceGravity GetCurrentSurface()
    {
        return this;
    }
}
