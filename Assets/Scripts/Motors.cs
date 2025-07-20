using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motors : MonoBehaviour {
    public List<SpriteRenderer> motorEffects;
    public float alphaSmoothness = 5f;

    float alpha = 0;

    private void Update() {
        foreach(SpriteRenderer renderer in motorEffects) {
            Color target = renderer.color;
            target.a = alpha;
            renderer.color = Color.Lerp(renderer.color, target, alphaSmoothness * Time.deltaTime);
        }
    }

    public void TurnOnMotors() {
        alpha = 1;
        AudioManager.StartPlaying("Engine");
    }

    public void TurnOffMotors() {
        alpha = 0;
        AudioManager.StopPlaying("Engine");
    }
}
