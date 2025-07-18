using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity {
    public float moveSmoothTime = 1f;
    public Transform ship;

    Vector2 velocity = Vector2.zero;

    private void Update() {
        Move();
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire) Shoot(ship.rotation);
    }

    private void FixedUpdate() {
        rb.velocity = velocity;
    }

    protected override void Move() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = x * ship.right + y * ship.up;
        if (input.magnitude > 0.01f) {
            velocity = Vector2.Lerp(velocity, input.normalized * moveSpeed, Time.deltaTime * moveSmoothTime);
        } else {
            velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * moveSmoothTime);
        }
    }

    public override void Die() {
        ship.gameObject.SetActive(false);
    }
}
