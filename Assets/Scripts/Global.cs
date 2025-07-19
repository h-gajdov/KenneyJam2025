using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global {
    public static List<Sprite> explosionSprites = new List<Sprite>();

    public static void Initialize() {
        explosionSprites.Clear();
        for(int i = 0; i < 9; i++) {
            Sprite explosion = Resources.Load<Sprite>($"SmokeParticles/PNG/Explosion/explosion0{i}");
            explosionSprites.Add(explosion);
        }
    }
}
