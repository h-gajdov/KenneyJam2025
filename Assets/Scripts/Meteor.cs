using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Bullet, ITarget {
    public float health = 100;
    public GameObject enemyIndicator;
    public Sprite sprite;

    SpriteRenderer spriteRenderer;
    EnemyIndicator indicator;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();

        int randIdx = Random.Range(0, Global.meteorSprites.Count);
        Sprite meteorSprite = Global.meteorSprites[randIdx];
        sprite = meteorSprite;
        spriteRenderer.sprite = sprite;

        indicator = Instantiate(enemyIndicator, UIManager.CanvasTransform, true).GetComponent<EnemyIndicator>();
        indicator.Initialize(this);
        indicator.StartBlinking();
    }

    public void Die() {
        Destroy(gameObject);
    }

    public void TakeDamage(float damage) {
        health -= damage;

        if (health <= 0) Die();
    }

    private new void FixedUpdate() {
        Move();
    }

    private void Move() {
        Vector3 direction = -transform.position.normalized;
        rb.velocity = direction * moveSpeed;
    }

    public Transform GetTransform() {
        return transform;
    }
}
