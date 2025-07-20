using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerCore : MonoBehaviour, ITarget {
    public float startHealth = 100;
    public float health = 100;
    public Slider healthBar;
    public Image sliderFill;

    public static PowerCore instance;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }
    }

    private void Update() {
        float healthPercent = health / startHealth;
        healthBar.value = healthPercent;
        sliderFill.color = GameManager.SampleGradient(healthPercent);
    }

    public static void Heal(float amount) {
        instance.health = (instance.health + amount < instance.startHealth) ? instance.health + amount : instance.startHealth;
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
