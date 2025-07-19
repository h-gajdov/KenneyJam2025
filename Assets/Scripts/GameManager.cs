using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player player;
    public Gradient healthBarGradient;

    public static GameManager instance;

    public static Transform PlayerTransform {
        get {
            return instance.player.transform;
        }
    }

    public static Vector3 PlayerPosition {
        get {
            return PlayerTransform.position;
        }
    }

    public static Color SampleGradient(float value) {
        return instance.healthBarGradient.Evaluate(value);
    }

    public void Awake() {
        Global.Initialize();
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }
}
