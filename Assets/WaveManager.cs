using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class Wave {
    public int numberOfScouts;  
    public int numberOfTanks;
    public int numberOfMeteors;

    public float scoutSpawnRate;
    public float tankSpawnRate;
    public float meteorSpawnRate;

    public float waitAfter;

    public Wave(int numberOfScouts, int numberOfTanks, int numberOfMeteors, float scoutSpawnRate, float tankSpawnRate, float meteorSpawnRate, float waitAfter) {
        this.numberOfScouts = numberOfScouts;
        this.numberOfTanks = numberOfTanks;
        this.numberOfMeteors = numberOfMeteors;

        this.scoutSpawnRate = scoutSpawnRate;
        this.tankSpawnRate = tankSpawnRate;
        this.meteorSpawnRate = meteorSpawnRate;

        this.waitAfter = waitAfter;
    }
}

public class WaveManager : MonoBehaviour {
    private Wave[] waves = {
        new Wave(3, 0, 1, 2.5f, 0f, 6f, 5),
        new Wave(5, 1, 2, 2f, 6f, 5f, 6),
        new Wave(7, 2, 3, 1.5f, 5.5f, 4f, 7),
        new Wave(8, 3, 4, 1.2f, 5f, 3.5f, 8),
        new Wave(10, 5, 5, 1f, 4.5f, 3f, 9)
    };

    public float spawnRadius;
    public GameObject scoutPrefab, tankPrefab, meteorPrefab;
    public GameObject shop;
    public TextMeshProUGUI timeText;

    public List<GameObject> aliveEnemies = new List<GameObject>();

    Wave currentWave;
    bool waveInProgress = false;
    float targetTime;
    int numberOfWave = 0;
    int targetNumberOfEnemies = 0, currentNumberOfSpawnedEnemies = 0;

    private void Start() {
        StartCoroutine(EndWave(5f));
    }

    private void Update() {
        float elapsed = targetTime - Time.time;
        if (elapsed > 0) {
            timeText.text = $"Next wave in: {Mathf.CeilToInt(elapsed)}...";
        }

        if (!waveInProgress) return;

        List<GameObject> toDelete = new List<GameObject>();
        foreach(var enemy in aliveEnemies) {
            if (enemy == null) toDelete.Add(enemy);
        }

        aliveEnemies.RemoveAll(enemy => toDelete.Contains(enemy));

        if(aliveEnemies.Count == 0 && currentNumberOfSpawnedEnemies == targetNumberOfEnemies) {
            Debug.Log(currentWave.waitAfter);
            StartCoroutine(EndWave(currentWave.waitAfter));
        }
    }

    private void StartWave(int index) {
        CameraManager.SetZoomedOut(false);
        shop.SetActive(false);

        currentWave = waves[index];
        targetNumberOfEnemies = currentWave.numberOfScouts + currentWave.numberOfTanks + currentWave.numberOfMeteors;
        currentNumberOfSpawnedEnemies = 0;

        if(currentWave.numberOfScouts != 0) StartCoroutine(SpawnEnemy(scoutPrefab, currentWave.scoutSpawnRate, currentWave.numberOfScouts));
        if(currentWave.numberOfTanks != 0) StartCoroutine(SpawnEnemy(tankPrefab, currentWave.tankSpawnRate, currentWave.numberOfTanks));
        if(currentWave.numberOfMeteors != 0) StartCoroutine(SpawnEnemy(meteorPrefab, currentWave.meteorSpawnRate, currentWave.numberOfMeteors));
    }

    private IEnumerator SpawnEnemy(GameObject prefab, float waitTime, int count) {
        while(count > 0) {
            float offset = Random.Range(-0.3f, 0.3f);
            yield return new WaitForSeconds(waitTime + offset);
            waveInProgress = true;

            Vector2 enemyPosition = GameMath.GetRandomPointOnArc(Vector2.zero, spawnRadius, 0, 360);
            GameObject enemy = Instantiate(prefab, enemyPosition, Quaternion.identity);
            aliveEnemies.Add(enemy);
            currentNumberOfSpawnedEnemies++;
            count--;
        }
    }

    public IEnumerator EndWave(float waitTime) {
        CameraManager.SetZoomedOut(true);
        shop.SetActive(true);
        waveInProgress = false;
        targetTime = Time.time + waitTime;

        yield return new WaitForSeconds(waitTime);

        if (numberOfWave == 5) 
            Debug.LogError("IMLEMENT BOSS FIGHT!");
        else 
            StartWave(numberOfWave++);
    }
}
