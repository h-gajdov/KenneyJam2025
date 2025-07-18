using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public static float moveSpeed = 10f;
    public static float distanceToStop = 25f;
    public static float fireRate = 2f;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public List<Transform> shootingPoints;

    Vector3 targetDestination;
    float nextTimeToFire;
    float totalDistance;
    bool canShoot = false;
    int shootingPointIdx = 0;

    private void Start() {
        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        transform.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void Update() {
        if (!canShoot || Time.time < nextTimeToFire) return;

        Shoot();
        nextTimeToFire = Time.time + (1f / nextTimeToFire);
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move() {
        float distanceFromDestination = Vector3.Distance(transform.position, targetDestination);
        float percentToDestination = 1f - Mathf.Clamp01(distanceFromDestination / totalDistance);
        float speed = moveSpeed * GameMath.ReductionFunctionValueForEnemy(percentToDestination);
        if (speed < 0.1) {
            speed = 0;
            canShoot = true;
        }

        rb.velocity = transform.up * speed;
        if (canShoot) transform.position = targetDestination;
    }

    private void Shoot() {
        Vector3 shootingPosition = shootingPoints[shootingPointIdx].position;
        GameObject bullet = Instantiate(bulletPrefab, shootingPosition, transform.rotation);
        shootingPointIdx = (shootingPointIdx + 1) % shootingPoints.Count;
        Destroy(bullet, 10);
    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(Vector3.zero, distanceToStop);
    //}
}
