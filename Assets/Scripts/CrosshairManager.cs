using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour {
    public Image crosshairImage;
    public Sprite inGameCrosshair;
    public Sprite defaultCrosshair;

    public static CrosshairManager instance;

    public void SetCrosshair(Sprite sprite) {
        crosshairImage.sprite = sprite;
        crosshairImage.SetNativeSize();
    }

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Start() {
        Cursor.visible = false;
        SetCrosshair(defaultCrosshair);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.visible = true;
        if (Input.GetMouseButtonDown(0)) Cursor.visible = false;
        crosshairImage.transform.position = Input.mousePosition;
    }
}
