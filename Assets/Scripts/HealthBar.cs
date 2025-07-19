using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    public Image sliderFill;
    public Image sliderBackground;

    Slider slider;
    Entity entity;
    float timeOfLastHealthBarUpdate;
    float prevValue;

    private void Start() {
        slider = GetComponent<Slider>();
        slider.value = prevValue = 1;
        entity = GetComponentInParent<Entity>();
    }

    public void Update() {
        float healthPercent = entity.health / 100f;
        slider.value = healthPercent;
        sliderFill.color = GameManager.SampleGradient(healthPercent);

        if (prevValue != healthPercent) timeOfLastHealthBarUpdate = Time.time;
        prevValue = healthPercent;

        float timePassed = Time.time - timeOfLastHealthBarUpdate;
        if (timePassed > 5f) timePassed = 5f;

        float alpha = GameMath.ReductionFunctionForHealthBar(timePassed);
        SetImageTransparency(sliderFill, alpha);
        SetImageTransparency(sliderBackground, alpha);
    }

    private void SetImageTransparency(Image image, float alpha) {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
