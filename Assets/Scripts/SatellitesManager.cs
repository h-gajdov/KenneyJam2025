using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatellitesManager : MonoBehaviour
{
    public GameObject satellitePrefab;
    public float satelliteRadius = 5f;
    public float satellitesParentRotationSpeed = 2f;
    public int maxNumberOfSatellitesPerLevel = 100;
    public Transform satellitesParent;

    float satellitesOrbit = 0;
    int numberOfSatellites;
    Vector3[] spawnPositions;

    private void Initialize() {
        spawnPositions = new Vector3[] {
            new Vector3(0, satelliteRadius, 0),
            new Vector3(satelliteRadius, 0, 0),
            new Vector3(0, -satelliteRadius, 0),
            new Vector3(-satelliteRadius, 0, 0),
        };
    }

    private void Awake() {
        Initialize();
    }

    private void Update() {
        satellitesOrbit += satellitesParentRotationSpeed * Time.deltaTime;
        satellitesParent.transform.rotation = Quaternion.Euler(Vector3.forward * satellitesOrbit);
    }

    public void BuySatellite() {
        if (Player.instance.coins < ShopManager.satelliteShieldPrice) return;
        if (numberOfSatellites >= 3 * maxNumberOfSatellitesPerLevel) return;

        GameObject satellite = Instantiate(satellitePrefab, satellitesParent, true);
        satellite.transform.localPosition = spawnPositions[numberOfSatellites % 4];
        satellite.transform.rotation = GameMath.GetLookAtRotation(satellite.transform.position, Vector3.zero);

        float angle = -360f / (maxNumberOfSatellitesPerLevel / 2);
        spawnPositions[numberOfSatellites % 4] = GameMath.RotateVector(spawnPositions[numberOfSatellites % 4], angle);
        numberOfSatellites++;

        if (numberOfSatellites % maxNumberOfSatellitesPerLevel == 0) {
            satelliteRadius += 2;
            for (int i = 0; i < spawnPositions.Length; i++) spawnPositions[i] = spawnPositions[i].normalized * satelliteRadius;
        }

        Player.instance.Pay(ShopManager.satelliteShieldPrice);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Vector3.zero, satelliteRadius);
    }
}
