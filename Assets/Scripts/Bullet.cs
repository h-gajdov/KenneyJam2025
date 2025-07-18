using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public Rigidbody2D rb;
    public float moveSpeed = 10f;

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
        Debug.Log($"Hit: {collision.gameObject.name}");
    }

    private void FixedUpdate() {
        rb.velocity = moveSpeed * transform.up;
    }
}
