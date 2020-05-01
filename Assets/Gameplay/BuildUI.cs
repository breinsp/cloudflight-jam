using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildSystem))]
public class BuildUI : MonoBehaviour
{
    public static bool hoveringOnPanel;

    public GameObject buildButtonPrefab;
    public GameObject buildPanel;

    private BuildSystem buildSystem;

    void Start()
    {
        buildSystem = GetComponent<BuildSystem>();
        SetupBuildPanel();
    }

    void SetupBuildPanel()
    {
        int buttonSize = Mathf.RoundToInt(buildButtonPrefab.GetComponent<RectTransform>().sizeDelta.x);
        int padding = 10;
        int buttonOuterSize = buttonSize + 2 * padding;

        int count = buildSystem.buildings.Count;
        int width = buttonOuterSize * count;
        int height = buttonOuterSize;

        buildPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);

        for (int i = 0; i < count; i++)
        {
            BuildingEntity building = buildSystem.buildings[i];
            GameObject instance = Instantiate(buildButtonPrefab);
            instance.transform.SetParent(buildPanel.transform);
            BuildButton buildButton = instance.GetComponent<BuildButton>();
            buildButton.cost = building.cost;
            buildButton.OnClick += () => buildSystem.Build(building);
            RectTransform rectTransform = instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector3(i * buttonOuterSize + padding, -padding, 0);
        }
    }
}
