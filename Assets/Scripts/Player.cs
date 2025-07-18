using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 1f;
    public float moveSmoothTime = 1f;
    public Transform ship;
    public Rigidbody2D rb;

    Vector2 velocity = Vector2.zero;

    private void Update() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = x * ship.right + y * ship.up;
        if (input.magnitude > 0.01f) {
            velocity = Vector2.Lerp(velocity, input.normalized * moveSpeed, Time.deltaTime * moveSmoothTime);
        } else {
            velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * moveSmoothTime);
        }
    }

    private void FixedUpdate() {
        rb.velocity = velocity;
    }
}
