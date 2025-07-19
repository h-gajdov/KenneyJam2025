using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
    public int level = 0;
    public float enemyInRangeRadius = 10f;
    public float damage = 25f;
    public float fireRate = 5f;
    public GameObject bulletPrefab;

    int turretIndex;
    float nextFireTime;
    Enemy target;
    Quaternion initialRotation;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        turretIndex = transform.GetSiblingIndex();
        spriteRenderer = GetComponent<SpriteRenderer>();

        float angle = -30f * turretIndex;
        initialRotation = Quaternion.Euler(Vector3.forward * angle);
        transform.rotation = initialRotation;
    }

    private void Update() {
        if (level == 0) return;

        if(target == null) target = GetEnemyInRange();
        else if(Time.time >= nextFireTime) {
            transform.rotation = GameMath.GetLookAtRotation(transform.position, target.transform.position, Vector3.forward, 90f);
            Shoot();
        }
    }

    public void Shoot() {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation).GetComponent<Bullet>();
        bullet.shotFrom = gameObject;
        bullet.damage = damage * level;
        Destroy(bullet, 10f);
        nextFireTime = Time.time + (1f / fireRate);
    }

    public Enemy GetEnemyInRange() {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, enemyInRangeRadius);
        return hit.GetComponent<Enemy>();
    }

    public void Upgrade() {
        level++;
        spriteRenderer.sprite = TurretsManager.GetLevelTurretSprite(level);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyInRangeRadius);
        if (target != null)
            Gizmos.DrawWireSphere(target.transform.position, 1f);
    }
}
