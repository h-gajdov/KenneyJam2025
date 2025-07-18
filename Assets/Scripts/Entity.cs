using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour {
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
        GameObject bullet = Instantiate(bulletPrefab, shootingPosition, rotation);
        shootingPointIdx = (shootingPointIdx + 1) % shootingPoints.Count;
        Destroy(bullet, 10);

        nextTimeToFire = Time.time + (1 / fireRate);
    }
}
