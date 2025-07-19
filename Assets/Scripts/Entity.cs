using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget {
    public void TakeDamage(float damage);
    public void Die();

    public Transform GetTransform();
}

public class Target : MonoBehaviour, ITarget {
    public float health = 100;

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) Die();
    }

    public virtual void Die() {
        Destroy(gameObject);
    }

    public Transform GetTransform() {
        return transform;
    }
}

public abstract class Entity : Target {
    public float moveSpeed = 1f;
    public float fireRate = 2f;
    public float missleFireRate = 0.5f;
    public float bulletDamage = 10f;
    public float missleDamage = 10f;
    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public GameObject misslePrefab;
    public Transform shipBody;
    public List<Transform> shootingPoints;
    public Motors motors;

    protected float nextTimeToFire;
    protected float nextTimeToFireMissile;
    protected int shootingPointIdx = 0;

    protected abstract void Move();

    protected void Shoot() {
        Vector3 shootingPosition = shootingPoints[shootingPointIdx].position;
        Bullet bullet = Instantiate(bulletPrefab, shootingPosition, shipBody.rotation).GetComponent<Bullet>();
        bullet.damage = bulletDamage;
        bullet.shotFrom = gameObject;
        Destroy(bullet, 10);

        nextTimeToFire = Time.time + (1 / fireRate);

        Animator anim;
        if (shootingPoints[shootingPointIdx].TryGetComponent<Animator>(out anim)) {
            anim.SetTrigger("Shoot");
        }
        shootingPointIdx = (shootingPointIdx + 1) % shootingPoints.Count;
    }

    protected void FireMissileAt(Transform target) {
        Missile missle = Instantiate(misslePrefab, transform.position, shipBody.rotation).GetComponent<Missile>();
        missle.damage = missleDamage;
        missle.shotFrom = gameObject;
        missle.target = target;
        nextTimeToFireMissile = Time.time + (1 / missleFireRate);
        Destroy(missle.gameObject, 5f);
    }
}
