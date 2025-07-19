using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public int level = 0;

    int turretIndex;
    Quaternion initialRotation;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        turretIndex = transform.GetSiblingIndex();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float angle = -30f * turretIndex;
        initialRotation = Quaternion.Euler(Vector3.forward * angle);
        transform.rotation = initialRotation;
    }

    public void Upgrade() {
        level++;
        spriteRenderer.sprite = TurretsManager.GetLevelTurretSprite(level);
    }
}
