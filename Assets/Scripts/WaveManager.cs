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
    public float spawnRadius;
    public GameObject scoutPrefab, tankPrefab, meteorPrefab;
    public GameObject shop;
    public TextMeshProUGUI timeText;

    public List<GameObject> aliveEnemies = new List<GameObject>();

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
            StartCoroutine(EndWave(10f));
        }
    }

    private void StartWave(int index) {
        CameraManager.SetZoomedOut(false);
        shop.SetActive(false);

        int numberOfScouts = GetScoutCount(numberOfWave);
        int numberOfMeteors = GetMeteorCount(numberOfWave);
        int numberOfTanks = GetTankCount(numberOfWave);

        float scoutSpawnRate = GetScoutSpawnInterval(numberOfWave);
        float meteorSpawnRate = GetMeteorSpawnInterval(numberOfWave);
        float tankSpawnRate = GetTankSpawnInterval(numberOfWave);

        targetNumberOfEnemies = numberOfScouts + numberOfMeteors + numberOfTanks;
        currentNumberOfSpawnedEnemies = 0;

        if(numberOfScouts != 0) StartCoroutine(SpawnEnemy(scoutPrefab, scoutSpawnRate, numberOfScouts));
        if(numberOfTanks != 0) StartCoroutine(SpawnEnemy(tankPrefab, tankSpawnRate, numberOfTanks));
        if(numberOfMeteors != 0) StartCoroutine(SpawnEnemy(meteorPrefab, meteorSpawnRate, numberOfMeteors));
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

        StartWave(numberOfWave++);
    }

    public int GetScoutCount(int wave) {
        return 3 + wave; // Base 3, +1 per wave
    }

    public int GetTankCount(int wave) {
        return Mathf.FloorToInt(wave / 2f); // 1 tank every 2 waves
    }

    public int GetMeteorCount(int wave) {
        return Mathf.FloorToInt(wave / 3f); // 1 meteor every 3 waves
    }

    public float GetScoutSpawnInterval(int wave) {
        return Mathf.Clamp(1.5f - wave * 0.04f, 0.4f, 1.5f);
    }

    public float GetMeteorSpawnInterval(int wave) {
        return Mathf.Clamp(3.5f - wave * 0.06f, 1.0f, 3.5f);
    }

    public float GetTankSpawnInterval(int wave) {
        return Mathf.Clamp(6f - wave * 0.1f, 2f, 6f);
    }
}
