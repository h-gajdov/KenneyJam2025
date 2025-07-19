using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Entity {
    public float moveSmoothTime = 1f;
    public float enemyInRangeRadius = 10f;
    public MissleCrosshair missleCrosshair;

    Vector2 velocity = Vector2.zero;
    float nextTimeToSeekForTargets;

    private void Start() {
        InvokeRepeating("TargetEnemiesInRange", 0f, 1f);
    }

    private void Update() {
        Move();
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire) Shoot();

        if(missleCrosshair.target == null && Time.time >= nextTimeToSeekForTargets) {
            TargetEnemiesInRange();
            if(missleCrosshair.target != null) nextTimeToSeekForTargets = Time.time + 20f;
        }
        else if (Input.GetMouseButtonDown(1) && Time.time >= nextTimeToFireMissile) FireMissileAt(missleCrosshair.target);
    }

    private void FixedUpdate() {
        rb.velocity = velocity;
    }

    protected override void Move() {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 input = x * transform.right + y * transform.up;
        if (input.magnitude > 0.01f) {
            velocity = Vector2.Lerp(velocity, input.normalized * moveSpeed, Time.deltaTime * moveSmoothTime);
            motors.TurnOnMotors();
        } else {
            velocity = Vector2.Lerp(velocity, Vector2.zero, Time.deltaTime * moveSmoothTime);
            motors.TurnOffMotors();
        }
    }

    public override void Die() {
        shipBody.gameObject.SetActive(false);
    }

    public Enemy[] GetEnemiesInRange() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, enemyInRangeRadius);
        List<Enemy> enemies = new List<Enemy>();
        foreach(var hit in hits) {
            Enemy enemy;
            if (hit.TryGetComponent<Enemy>(out enemy)) enemies.Add(enemy);
        }
        return enemies.ToArray();
    }

    private void TargetEnemiesInRange() {
        Enemy[] enemiesInRange = GetEnemiesInRange();
        if (enemiesInRange.Length == 0) return;
        missleCrosshair.SetTarget(enemiesInRange[0].transform);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyInRangeRadius);
    }
}
