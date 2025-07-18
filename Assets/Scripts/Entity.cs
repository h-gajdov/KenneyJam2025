using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
    public float health = 100;
    public float moveSpeed = 1f;
    public float fireRate = 2f;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public List<Transform> shootingPoints;

    protected float nextTimeToFire;
    protected int shootingPointIdx = 0;

    protected abstract void Move();

    protected void Shoot() {
        Shoot(transform.rotation);
    }

    protected void Shoot(Quaternion rotation) {
        Vector3 shootingPosition = shootingPoints[shootingPointIdx].position;
        Bullet bullet = Instantiate(bulletPrefab, shootingPosition, rotation).GetComponent<Bullet>();
        bullet.shotFrom = gameObject;
        Destroy(bullet, 10);

        nextTimeToFire = Time.time + (1 / fireRate);

        Animator anim;
        if (shootingPoints[shootingPointIdx].TryGetComponent<Animator>(out anim)) {
            anim.SetTrigger("Shoot");
        }
        shootingPointIdx = (shootingPointIdx + 1) % shootingPoints.Count;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) Die(); 
    }

    public virtual void Die() {
        Destroy(gameObject);
    }
}
