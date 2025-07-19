using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public Player player;

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

    public void Awake() {
        Global.Initialize();
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }
}
