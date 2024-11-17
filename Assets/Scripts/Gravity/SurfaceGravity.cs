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
    PlayerController2 playerController2;
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

        //orientation
        if (surfaceFloor == surfaceFloor.Front)
        {
            gravityDirection = new Vector3(0, -1, 0);
        }
        else if (surfaceFloor == surfaceFloor.Back)
        {
            gravityDirection = new Vector3(0, 1, 0);
        }
        else if (surfaceFloor == surfaceFloor.Left)
        {
            gravityDirection = new Vector3(0, 0, 1);
        }
        else if (surfaceFloor == surfaceFloor.Right)
        {
            gravityDirection = new Vector3(0, 0, -1);
        }
        else if (surfaceFloor == surfaceFloor.Top)
        {
            gravityDirection = new Vector3(1, 0, 0);
        }
        else if (surfaceFloor == surfaceFloor.Bottom)
        {
            gravityDirection = new Vector3(-1, 0, 0);
        }

    }
    public Vector3 GetGravityDirection()
    {
        return gravityDirection;
    }

    private void OnDrawGizmos()
    {
        if (surfaceFloor == surfaceFloor.Front)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
        else if (surfaceFloor == surfaceFloor.Back)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
        else if (surfaceFloor == surfaceFloor.Left)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
        else if (surfaceFloor == surfaceFloor.Right)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
        else if (surfaceFloor == surfaceFloor.Top)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
        else if (surfaceFloor == surfaceFloor.Bottom)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, surfaceArea);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController2 playerController = other.GetComponent<PlayerController2>();
            if (playerController != null)
            {
                playerController.SetCurrentSurface(this, true);
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
            if (rb != null)
            {
                Vector3 gravityForce = gravity * gravityDirection.normalized;
                rb.AddForce(gravityForce, ForceMode.Acceleration);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController2 playerController = other.GetComponent<PlayerController2>();
            if (playerController != null)
            {
                playerController.SetCurrentSurface(this, false);
            }
            GravitySphere gravitySphere = FindObjectOfType<GravitySphere>();
            gravitySphere?.SetPlayerInsideSurfaceGravity(false);
            Debug.Log($"Player exited {surfaceFloor} area");
        }
    }
}
