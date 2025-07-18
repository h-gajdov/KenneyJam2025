using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBG : MonoBehaviour
{
    SpriteRenderer renderer;
    float alpha = 0;

    private void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(Input.GetKey(KeyCode.W)) {
            alpha = 1;
        } else {
            alpha = 0;
        }

        Color t = renderer.color;
        t.a = alpha;
        renderer.color = Color.Lerp(renderer.color, t, 5f * Time.deltaTime);
    }
}
