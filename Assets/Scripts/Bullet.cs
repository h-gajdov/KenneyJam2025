using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour {
    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float damage = 10f;

    public GameObject impactEffectPrefab;
    public GameObject shotFrom;

    protected void OnTriggerEnter2D(Collider2D collision) {
        HandleContact(collision.gameObject);
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        HandleContact(collision.gameObject);
    }

    protected void HandleContact(GameObject other) {
        if (other.CompareTag("Bullet")) return;
        if (other.gameObject == shotFrom) return;
        if(shotFrom != null) {
            if (other.gameObject.CompareTag("PowerCore") && shotFrom.CompareTag("Player")) return;
            if (other.tag == shotFrom.tag) {
                if(other.CompareTag("Enemy")) return;
            }
        }

        Debug.Log($"Hit: {other.gameObject.name}");

        Entity entity;
        if (other.gameObject.TryGetComponent<Entity>(out entity)) {
            entity.TakeDamage(damage);
        }

        float randAngle = Random.Range(0, 360f);
        GameObject impactEffect = Instantiate(impactEffectPrefab, transform.position, Quaternion.Euler(Vector3.forward * randAngle));
        int randImpactSprite = Random.Range(0, Global.explosionSprites.Count);
        impactEffect.GetComponentInChildren<SpriteRenderer>().sprite = Global.explosionSprites[randImpactSprite];

        Destroy(impactEffect, 1);
        Destroy(gameObject);
    }

    protected void FixedUpdate() {
        rb.velocity = moveSpeed * transform.up;
    }
}
