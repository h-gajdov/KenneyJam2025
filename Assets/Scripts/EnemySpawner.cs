using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public List<GameObject> enemies;
    public float spawnRate = 1f;
    public float spawnRadius = 10f;

    public static EnemySpawner instance;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Start() {
        InvokeRepeating("SpawnEnemy", 0, spawnRate);    
    }

    public void SpawnEnemy() {
        int enemyIdx = Random.Range(0, enemies.Count);
        Vector2 enemyPosition = GameMath.GetRandomPointOnArc(Vector2.zero, spawnRadius, 0, 360);
        GameObject enemy = Instantiate(enemies[enemyIdx], enemyPosition, Quaternion.identity);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Vector3.zero, spawnRadius);
    }
}
