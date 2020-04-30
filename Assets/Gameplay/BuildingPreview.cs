using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    public bool colliding;

    private MeshRenderer meshRenderer;
    private Rigidbody rb;

    private List<Collider> collidingWith;

    private void Awake()
    {
        collidingWith = new List<Collider>();
        meshRenderer = GetComponent<MeshRenderer>();
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        GetComponent<FloorToGround>().onUpdate = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain") return;
        collidingWith.Add(collision.collider);
        colliding = collidingWith.Count > 0;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collidingWith.Contains(collision.collider)) return;
        collidingWith.Remove(collision.collider);
        colliding = collidingWith.Count > 0;
    }

    public void UpdateVisibility()
    {
        meshRenderer.enabled = !colliding;
    }
}
