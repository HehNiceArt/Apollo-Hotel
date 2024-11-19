using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        RaycastHit hit;
        Vector3 down = -transform.up;
        if (Physics.Raycast(transform.position, down, out hit, 10))
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, Time.deltaTime * 10);
        }
    }
}
