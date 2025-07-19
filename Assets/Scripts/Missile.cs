using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet {
    public Transform target;

    private void Update() {
        if (target == null) return;
        transform.rotation = GameMath.GetLookAtRotation(transform.position, target.position, Vector3.forward, 90f);
    }
}
