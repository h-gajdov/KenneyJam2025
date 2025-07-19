using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global {
    public static List<Sprite> explosionSprites = new List<Sprite>();
    public static Sprite tankSprite, scoutSprite;
    public static Dictionary<EnemyType, Sprite> enemySprites = new Dictionary<EnemyType, Sprite>();

    public static void Initialize() {
        explosionSprites.Clear();
        for(int i = 0; i < 9; i++) {
            Sprite explosion = Resources.Load<Sprite>($"SmokeParticles/PNG/Explosion/explosion0{i}");
            explosionSprites.Add(explosion);
        }

        tankSprite = Resources.Load<Sprite>("SpaceShooterExtension/PNG/Sprites X2/Ships/spaceShips_002");
        scoutSprite = Resources.Load<Sprite>("SpaceShooterExtension/PNG/Sprites X2/Ships/spaceShips_007");
        enemySprites.Clear();
        enemySprites.Add(EnemyType.Tank, tankSprite);
        enemySprites.Add(EnemyType.Scout, scoutSprite);
    }
}
