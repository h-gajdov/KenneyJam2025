using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float defaultSize = 8f;
    public float zoomedOutSize = 16f;
    public float blendingSmoothnessSpeed = 5f;

    public static Camera mainCamera;
    public static CameraManager instance;
    bool zoomedOut = false;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) zoomedOut = !zoomedOut;

        if (zoomedOut) FocusOnPowerCore();
        else FocusOnPlayer();
    }

    private void FocusOnPowerCore() {
        transform.parent = null;
        transform.position = Vector3.Lerp(transform.position, Vector3.forward * -10f, blendingSmoothnessSpeed * Time.deltaTime);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomedOutSize, blendingSmoothnessSpeed * Time.deltaTime);
    }

    private void FocusOnPlayer() {
        transform.parent = GameManager.PlayerTransform;
        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.forward * -10f, blendingSmoothnessSpeed * Time.deltaTime);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, defaultSize, blendingSmoothnessSpeed * Time.deltaTime);
    }

    public static void SetZoomedOut(bool value) {
        instance.zoomedOut = value;
    }
}
