using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed = 1f;
    public float moveSmoothTime = 1f;
    public float fireRate = 2f;
    public GameObject bulletPrefab;
    public Transform ship;
    public List<Transform> shootingPoints;
    public Rigidbody2D rb;

    Vector2 velocity = Vector2.zero;
    float nextFireTime;
    int shootingPointIdx = 0;

    private void Update() {
        Move();
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime) {
            Shoot();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    private void FixedUpdate() {
        rb.velocity = velocity;
    }

    private void Move() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = x * ship.right + y * ship.up;
        if (input.magnitude > 0.01f) {
            velocity = Vector2.Lerp(velocity, input.normalized * moveSpeed, Time.deltaTime * moveSmoothTime);
        } else {
            velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * moveSmoothTime);
        }
    }

    private void Shoot() {
        Vector3 shootingPosition = shootingPoints[shootingPointIdx].position;
        GameObject bullet = Instantiate(bulletPrefab, shootingPosition, ship.rotation);
        shootingPointIdx = (shootingPointIdx + 1) % shootingPoints.Count;
        Destroy(bullet, 10);
    }
}
