using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class EnemyIndicator : MonoBehaviour
{
    public Animator anim;
    public Transform enemy;
    public GameObject visual;
    public Image enemyTypeImage;
    public bool isBlinking;

    private RectTransform canvasRect;
    private RectTransform indicatorRect;

    public float edgePadding = 50f;

    public void Initialize(Enemy target) {
        transform.parent = UIManager.IndicatorsParent;
        enemy = target.transform;
        enemyTypeImage.sprite = Global.enemySprites[target.type];
    }

    public void Initialize(Meteor meteor) {
        transform.parent = UIManager.IndicatorsParent;
        enemy = meteor.transform;
        enemyTypeImage.sprite = meteor.sprite;
    }

    private void Start() {
        indicatorRect = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    private void Update() {
        if (enemy == null) {
            Destroy(gameObject);
            return;
        }

        Vector3 enemyScreenPos = CameraManager.mainCamera.WorldToScreenPoint(enemy.position);

        bool isOnScreen = enemyScreenPos.z > 0 &&
                          enemyScreenPos.x >= 0 && enemyScreenPos.x <= Screen.width &&
                          enemyScreenPos.y >= 0 && enemyScreenPos.y <= Screen.height;

        if (isOnScreen) {
            visual.SetActive(false);
            return;
        }

        visual.SetActive(true);

        Vector2 screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
        Vector2 screenPos = new Vector2(enemyScreenPos.x, enemyScreenPos.y);
        Vector2 dir = (screenPos - screenCenter).normalized;
        float halfWidth = canvasRect.rect.width / 2f;
        float halfHeight = canvasRect.rect.height / 2f;

        Vector2 canvasEdgePos = dir * 1000f;
        float x = canvasEdgePos.x;
        float y = canvasEdgePos.y;

        float slope = y / x;

        if (x > 0)
            x = halfWidth - edgePadding;
        else
            x = -halfWidth + edgePadding;

        y = x * slope;

        if (y > halfHeight - edgePadding) {
            y = halfHeight - edgePadding;
            x = y / slope;
        } else if (y < -halfHeight + edgePadding) {
            y = -halfHeight + edgePadding;
            x = y / slope;
        }

        indicatorRect.anchoredPosition = new Vector2(x, y);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        indicatorRect.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    public void StartBlinking() {
        isBlinking = true;
        anim.SetBool("isBlinking", isBlinking);
    }
}
