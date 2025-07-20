using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType {
    Scout, Tank
}

public class Enemy : Entity {
    public EnemyType type;
    public GameObject enemyIndicator;
    public float dropCoinsAmount;
    public static float distanceToStop = 25f;
    public static float aimPlayerDistance = 7f;
    public static float turnSmoothness = 25f;
    public static int succession = 0;
    public GameObject coin;

    EnemyIndicator indicator;
    Vector3 targetDestination;
    float totalDistance;
    bool canShoot = false;

    private void Awake() {
        health = startHealth;
        indicator = Instantiate(enemyIndicator, UIManager.CanvasTransform, true).GetComponent<EnemyIndicator>();
        indicator.Initialize(this);
    }

    private void Start() {
        motors.TurnOnMotors();

        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        shipBody.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void Update() {
        if (!canShoot) return;
        if (!indicator.isBlinking) indicator.StartBlinking();

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

    public override void Die() {
        base.Die();
        if (Player.instance.timeOfLastKilledEnemy > Time.time - 5f) {
            succession++;
        } else succession = 1;
        Player.instance.timeOfLastKilledEnemy = Time.time;
        Player.instance.AddToPowerMeter();

        int numberOfCoins = (int)dropCoinsAmount / 5;
        while (numberOfCoins > 0) {
            float value = (dropCoinsAmount <= 5) ? dropCoinsAmount : 5;
            Coin cn = Instantiate(coin, transform.position, Quaternion.identity).GetComponent<Coin>();
            cn.value = value;
            numberOfCoins--;
        }

        //TODO: Refactor this so that every enemy has a specific add amount to the score
        int addScore = (type == EnemyType.Scout) ? 10 : 30;
        Player.instance.AddScore(addScore);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Vector3.zero, distanceToStop);
    }
}
