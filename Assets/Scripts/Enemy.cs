using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity {
    public static float distanceToStop = 25f;

    Vector3 targetDestination;
    float totalDistance;
    bool canShoot = false;

    private void Start() {
        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        transform.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void Update() {
        if (!canShoot || Time.time < nextTimeToFire) return;

        Shoot();
    }

    private void FixedUpdate() {
        Move();
    }

    protected override void Move() {
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

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, distanceToStop);
    }
}
