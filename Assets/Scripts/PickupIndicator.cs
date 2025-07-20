using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickupIndicator : MonoBehaviour {
    public Image icon;
    public TextMeshProUGUI text;

    public void SetInfo(Sprite sprite, string info) {
        icon.sprite = sprite;
        text.text = info;
    }
}
