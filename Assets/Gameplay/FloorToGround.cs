using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorToGround : MonoBehaviour
{
    public LayerMask layerMask;
    public bool onUpdate = false;

    void Start()
    {
        layerMask = LayerMask.GetMask("Terrain");
        SetPosition();
    }

    void Update()
    {
        if (onUpdate)
        {
            SetPosition();
        }
    }

    private void SetPosition()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 10, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, layerMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y;
            transform.position = pos;
        }
    }
}
