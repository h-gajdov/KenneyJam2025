using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager instance;

    public static Transform CanvasTransform {
        get {
            return instance.transform;
        }
    }

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }
}
