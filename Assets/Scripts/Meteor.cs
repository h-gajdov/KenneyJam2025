using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : Bullet, ITarget {
    public float startHealth = 150f;
    public float health = 100;
    public float dropCoinsAmount = 8f;
    public GameObject enemyIndicator;
    public Transform body;
    public Sprite sprite;
    public SpriteRenderer spriteRenderer;

    EnemyIndicator indicator;

    private void Awake() {
        health = startHealth;
    }

    private void Start() {
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
