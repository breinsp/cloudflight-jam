using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public GameObject prefab;

    private Plane groundPlane;
    private Camera cam;
    private BuildingPreview previewInstance;
    private Transform buildingsHolder;

    // Start is called before the first frame update
    void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
        buildingsHolder = new GameObject("buildings").transform;
        buildingsHolder.SetParent(transform);
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray buildRay = cam.ScreenPointToRay(mousePos);

        bool hit = groundPlane.Raycast(buildRay, out float enterDistance);
        if (!hit)
        {
            Debug.LogWarning("not pointing at ground!");
            return;
        }
        Vector3 point = buildRay.GetPoint(enterDistance);

        if (previewInstance != null)
        {
            previewInstance.transform.position = point;
            previewInstance.UpdateVisibility();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (previewInstance != null && !previewInstance.colliding)
            {
                CompleteBuilding(point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (previewInstance == null)
            {
                StartBuilding();
            }
        }
        if (previewInstance != null && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)))
        {
            CancelBuilding();
        }

    }

    void StartBuilding()
    {
        GameObject instanceGameObject = Instantiate(prefab);
        instanceGameObject.name = "Preview";
        instanceGameObject.transform.SetParent(buildingsHolder);
        previewInstance = instanceGameObject.AddComponent<BuildingPreview>();
    }

    void CompleteBuilding(Vector3 position)
    {
        GameObject instance = Instantiate(prefab);
        instance.transform.SetParent(buildingsHolder);
        instance.transform.position = position;
        instance.name = prefab.name;
        Destroy(previewInstance.gameObject);
        previewInstance = null;
    }

    void CancelBuilding()
    {
        Destroy(previewInstance.gameObject);
        previewInstance = null;
    }
}
