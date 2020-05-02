using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class BuildingPreview : MonoBehaviour
{
    public bool colliding;

    private Rigidbody rb;

    private List<Collider> collidingWith;
    private Building building;

    private void Awake()
    {
        building = GetComponent<Building>();
        collidingWith = new List<Collider>();
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
        building.SetVisible(!colliding);
    }
}
