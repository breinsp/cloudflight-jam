using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Text costText;
    public int cost;

    private bool hovered;
    private RectTransform rect;
    private Vector2 pos;

    public delegate void onClick();
    public event onClick OnClick;

    private bool purchasable;

    void Start()
    {
        costText.text = cost.ToString();
        rect = GetComponent<RectTransform>();
        pos = rect.anchoredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        purchasable = GameManager.instance.MinionCount >= cost;

        if (hovered && purchasable)
        {
            rect.sizeDelta = new Vector2(66, 66);
            rect.anchoredPosition = pos - new Vector2(3, -3);
        }
        else
        {
            rect.sizeDelta = new Vector2(60, 60);
            rect.anchoredPosition = pos;
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
        {
            OnClick.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        BuildUI.hoveringOnPanel = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        BuildUI.hoveringOnPanel = false;
    }
}
