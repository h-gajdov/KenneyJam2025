using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup {
    public void Effect();
}

public abstract class Pickup : MonoBehaviour, IPickup {
    public float moveSpeed = 10f;
    protected bool moveToPlayer;
    public Sprite icon;
    public SpriteRenderer spriteRenderer;
    public GameObject pickupIndicator;

    private void Start() {
        float angle = Random.Range(0, 360f);
        float distance = Random.Range(0.5f, 1.25f);
        Vector3 target = GameMath.GetRandomPointOnArc(transform.position, distance, 0f, 360f);
        StartCoroutine(MoveToTarget(target));

        spriteRenderer.sprite = icon;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.CompareTag("Player")) {
            Effect();
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (!moveToPlayer) return;
        StopAllCoroutines();
        transform.position = Vector3.Lerp(transform.position, GameManager.PlayerPosition, moveSpeed * Time.deltaTime);
    }

    public void StartMovingToPlayer() {
        moveToPlayer = true;
    }

    private IEnumerator MoveToTarget(Vector3 target) {
        while(Vector3.Distance(transform.position, target) > 0.1f) {
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }

    public abstract void Effect();
}
