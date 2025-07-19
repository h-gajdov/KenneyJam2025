using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerCore : MonoBehaviour, ITarget {
    public float startHealth = 100;
    public float health = 100;
    public Slider healthBar;
    public Image sliderFill;

    private void Update() {
        float healthPercent = health / startHealth;
        healthBar.value = healthPercent;
        sliderFill.color = GameManager.SampleGradient(healthPercent);
    }

    public void TakeDamage(float amount) {
        health -= amount;

        if (health <= 0) Die();
    }

    public Transform GetTransform() {
        return transform;
    }

    public void Die() {
        Destroy(gameObject);
        Player.instance.Die();
    }
}
