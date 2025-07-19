using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        Bullet bullet;
        if(collision.TryGetComponent<Bullet>(out bullet)) {
            if (bullet.shotFrom == Player.instance.gameObject) return;
            Destroy(gameObject);
        }
    }
}
