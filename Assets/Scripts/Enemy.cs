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
        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        transform.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void Update() {
        if (!canShoot) return;

        if (Vector3.Distance(transform.position, GameManager.PlayerPosition) <= aimPlayerDistance) {
            if (type != EnemyType.Tank) LookAt(GameManager.PlayerPosition);
            else FireMissleAt(GameManager.PlayerTransform);
        } else LookAt(Vector3.zero);

        if (Time.time < nextTimeToFire) return;
        Shoot();
    }

    private void FixedUpdate() {
        Move();
    }

    private void LookAt(Vector3 position) {
        Quaternion targetRot = GameMath.GetLookAtRotation(transform.position, position, Vector3.forward, 90f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSmoothness * Time.deltaTime);
    }

    private void FireMissleAt(Transform transform) { 
        
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
