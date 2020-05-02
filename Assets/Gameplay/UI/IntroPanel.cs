using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPanel : MonoBehaviour
{
    public Button okButton;

    void Start()
    {
        okButton.onClick.AddListener(() => Destroy(gameObject));
    }
}
