using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity {
    public float moveSmoothTime = 1f;
    public float enemyInRangeRadius = 10f;
    public float timeOfLastKilledEnemy;
    public float powerMeterValue = 0;
    public float powerMeterDecaySpeed = 4f;
    public float ultimateBulletDamage = 100f;
    public float ultimateFireRate = 5f;
    public GameObject ultimateBulletPrefab;
    public MissleCrosshair missleCrosshair;
    public Slider powerMeterSlider;

    Vector2 velocity = Vector2.zero;
    Animator powerMeterAnim;
    float nextTimeToSeekForTargets;
    float nextTimeToFireUltimateBullet;
    bool ultimateIsOn = false;

    public static Player instance;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Start() {
        powerMeterAnim = powerMeterSlider.GetComponent<Animator>();
        InvokeRepeating("TargetEnemiesInRange", 0f, 1f);
    }

    private void Update() {
        Move();
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire) Shoot();

        if(missleCrosshair.target == null) {
            if(Time.time >= nextTimeToSeekForTargets) {
                TargetEnemiesInRange();
                if(missleCrosshair.target != null) nextTimeToSeekForTargets = Time.time + 20f;
            }
        }
        else if (missleCrosshair.image.enabled && Input.GetMouseButtonDown(1) && Time.time >= nextTimeToFireMissile) FireMissileAt(missleCrosshair.target);
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

        if (powerMeterValue < 1) powerMeterValue -= powerMeterDecaySpeed * Time.deltaTime;
        else {
            powerMeterAnim.SetBool("isBlinking", true);
            if(Input.GetMouseButtonDown(1)) {
                StopAllCoroutines();
                StartCoroutine(UltimateAttack());
                powerMeterAnim.SetBool("isBlinking", false);
                powerMeterValue = 0;
            }
        }

        powerMeterValue = (powerMeterValue < 0) ? 0 : powerMeterValue;
        powerMeterSlider.value = powerMeterValue;
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

    public void AddToPowerMeter() {
        if (ultimateIsOn) return;
        powerMeterValue += 0.2f;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyInRangeRadius);
    }

    private IEnumerator UltimateAttack() {
        float started = Time.time;
        float elapsed = 0;
        ultimateIsOn = true;
        while(elapsed < 5f) {
            if(Time.time >= nextTimeToFireUltimateBullet) {
                Bullet bullet = Instantiate(ultimateBulletPrefab, transform.position, shipBody.rotation).GetComponent<Bullet>();
                bullet.shotFrom = gameObject;
                bullet.damage = ultimateBulletDamage;
                Destroy(bullet.gameObject, 10f);
                nextTimeToFireUltimateBullet = Time.time + (1 / ultimateFireRate);
            }
            elapsed = Time.time - started;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        ultimateIsOn = false;
    }
}
