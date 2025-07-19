using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissleCrosshair : MonoBehaviour {
    public Transform target;
    public Image image;
    public float moveSpeed = 7f;

    private void Update() {
        if (target == null) {
            image.enabled = false;
            target = GameManager.PlayerTransform;
        }
        Vector3 targetScreenPosition = CameraManager.mainCamera.WorldToScreenPoint(target.position);
        float dist = Vector3.Distance(transform.position, targetScreenPosition);
        if (Vector3.Distance(transform.position, targetScreenPosition) > 20f) 
            transform.position = Vector3.Lerp(transform.position, targetScreenPosition, moveSpeed * Time.deltaTime);
        else
            transform.position = targetScreenPosition;
    }

    public void SetTarget(Transform value) {
        target = value;
        image.enabled = true;
    }
}
