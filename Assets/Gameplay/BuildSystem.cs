using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    public static BuildSystem instance;

    public List<BuildingEntity> buildings;
    public AudioClip buildSound;
    public Material invalidMaterial;
    public LayerMask layerMask;

    private BuildingEntity selectedEntity;
    private Camera cam;
    private BuildingPreview previewInstance;
    public Transform buildingsHolder;
    private AudioSource audioSource;


    void Awake()
    {
        instance = this;
        audioSource = gameObject.AddComponent<AudioSource>();
        buildingsHolder = new GameObject("buildings").transform;
        buildingsHolder.SetParent(transform);
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray buildRay = cam.ScreenPointToRay(mousePos);

        bool hit = Physics.Raycast(buildRay, out RaycastHit hitInfo, 1000, layerMask);
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
        GetKeyInput();
    }

    void GetKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Build(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Build(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Build(2);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            Build(3);
        if (Input.GetKeyDown(KeyCode.Alpha5))
            Build(4);
        if (Input.GetKeyDown(KeyCode.Alpha6))
            Build(5);
        if (Input.GetKeyDown(KeyCode.Alpha7))
            Build(6);
        if (Input.GetKeyDown(KeyCode.Alpha8))
            Build(7);
        if (Input.GetKeyDown(KeyCode.Alpha9))
            Build(8);
    }

    void CompleteBuilding(Vector3 position)
    {
        audioSource.PlayOneShot(buildSound);
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
        if(instance.name == "Enemy Spawner")
        {
            instance.transform.parent = GameManager.instance.enemySpawnerHolder;
        }
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
    internal void Build(int index)
    {
        if (index >= buildings.Count) return;
        Build(buildings[index]);
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
