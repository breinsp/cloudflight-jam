using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    public List<BuildingEntity> buildings;

    private BuildingEntity selectedEntity;
    private Camera cam;
    private BuildingPreview previewInstance;
    private Transform buildingsHolder;
    private LayerMask layerMask;

    void Awake()
    {
        instance = this;
        buildingsHolder = new GameObject("buildings").transform;
        buildingsHolder.SetParent(transform);
        cam = Camera.main;
        layerMask = LayerMask.GetMask("Terrain");
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray buildRay = cam.ScreenPointToRay(mousePos);

        bool hit = Physics.Raycast(buildRay, out RaycastHit hitInfo, layerMask);
        if (!hit)
        {
            Debug.LogWarning("not pointing at ground!");
            return;
        }
        Vector3 point = hitInfo.point;

        if (previewInstance != null)
        {
            previewInstance.transform.position = point;
            previewInstance.UpdateVisibility();
        }

        if (Input.GetMouseButtonDown(0) && !BuildUI.hoveringOnPanel && previewInstance != null && !previewInstance.colliding)
        {
            CompleteBuilding(point);
        }
        if (previewInstance != null && (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)))
        {
            CancelBuilding();
        }
    }

    void CompleteBuilding(Vector3 position)
    {
        GameManager.instance.SacrificeMinions(selectedEntity.cost);
        SpawnBuilding(position, selectedEntity);
        Destroy(previewInstance.gameObject);
        previewInstance = null;
    }

    public void SpawnBuilding(Vector3 position, int entityIndex)
    {
        BuildingEntity entity = buildings[entityIndex];
        SpawnBuilding(position, entity);
    }

    public void SpawnBuilding(Vector3 position, BuildingEntity entity)
    {
        GameObject instance = Instantiate(entity.prefab);
        instance.transform.SetParent(buildingsHolder);
        instance.transform.localRotation = Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0);
        instance.transform.position = position;
        instance.name = entity.name;
        instance.GetComponent<Building>().BuildingPlaced();
    }

    void CancelBuilding()
    {
        Destroy(previewInstance.gameObject);
        previewInstance = null;
    }

    internal void Build(BuildingEntity building)
    {
        if (GameManager.instance.MinionCount >= building.cost)
        {
            if (previewInstance != null)
            {
                CancelBuilding();
            }
            GameObject instanceGameObject = Instantiate(building.prefab);
            instanceGameObject.name = "Preview of " + building.name;
            instanceGameObject.transform.SetParent(buildingsHolder);
            previewInstance = instanceGameObject.AddComponent<BuildingPreview>();
            selectedEntity = building;
        }
    }
}

[Serializable]
public struct BuildingEntity
{
    public string name;
    public string description;
    public GameObject prefab;
    public Sprite icon;
    public int cost;
}
