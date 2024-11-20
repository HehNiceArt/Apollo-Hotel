using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeCollide : MonoBehaviour
{
    [SerializeField] float colliderRadius;
    SphereCollider sphereCollider;
    Level level;
    private void Start()
    {
        sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = colliderRadius;
        sphereCollider.isTrigger = true;
        level = GetComponent<Level>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("adww");
            level.LoadScene();
        }
    }
}
