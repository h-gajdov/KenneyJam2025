using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Scout, Tank
}

public class Enemy : Entity {
    public EnemyType type;
    public static float distanceToStop = 25f;
    public static float aimPlayerDistance = 7f;
    public static float turnSmoothness = 25f;

    Vector3 targetDestination;
    float totalDistance;
    bool canShoot = false;

    private void Start() {
        motors.TurnOnMotors();

        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        shipBody.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void Update() {
        if (!canShoot) return;

        bool canShootMissle = false;
        if (Vector3.Distance(transform.position, GameManager.PlayerPosition) <= aimPlayerDistance) {
            if (type != EnemyType.Tank) LookAt(GameManager.PlayerPosition);
            else canShootMissle = true;
        } else LookAt(Vector3.zero);

        if(canShootMissle && Time.time >= nextTimeToFireMissile) FireMissileAt(GameManager.PlayerTransform);
        if (Time.time < nextTimeToFire) return;
        Shoot();
    }

    private void FixedUpdate() {
        Move();
    }

    private void LookAt(Vector3 position) {
        Quaternion targetRot = GameMath.GetLookAtRotation(transform.position, position, Vector3.forward, 90f);
        shipBody.rotation = Quaternion.Lerp(shipBody.rotation, targetRot, turnSmoothness * Time.deltaTime);
    }

    private void FireMissileAt(Transform target) {
        Missile missle = Instantiate(misslePrefab, transform.position, shipBody.rotation).GetComponent<Missile>();
        missle.shotFrom = gameObject;
        missle.target = target;
        nextTimeToFireMissile = Time.time + (1 / missleFireRate);
        Destroy(missle.gameObject, 5f);
    }

    protected override void Move() {
        float distanceFromDestination = Vector3.Distance(transform.position, targetDestination);
        float percentToDestination = 1f - Mathf.Clamp01(distanceFromDestination / totalDistance);
        float speed = moveSpeed * GameMath.ReductionFunctionValueForEnemy(percentToDestination);
        if (speed < 0.1) {
            speed = 0;
            motors.TurnOffMotors();
            canShoot = true;
        }

        rb.velocity = shipBody.up * speed;
        if (canShoot) transform.position = targetDestination;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, distanceToStop);
    }
}
