using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float defaultSize = 8f;
    public float zoomedOutSize = 16f;
    public float blendingSmoothnessSpeed = 5f;

    public static Camera mainCamera;

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Z)) FocusOnPowerCore();
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
}
