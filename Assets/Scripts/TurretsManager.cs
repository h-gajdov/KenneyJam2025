using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretsManager : MonoBehaviour {
    public List<Sprite> turretLevelSprites = new List<Sprite>();
    public Transform turretsParent;
    
    private Turret[] turrets = new Turret[12];

    public static TurretsManager instance;

    private void Awake() {
        if (instance == null) instance = this;
        else {
            Destroy(this);
            return;
        }

        for(int i = 0; i < 12; i++) {
            turrets[i] = turretsParent.GetChild(i).GetComponent<Turret>();
        }
    }

    public void UpgradeTurret() {
        if (Player.instance.coins < ShopManager.turretPrice) return;

        GameObject clickedButton = EventSystem.current.currentSelectedGameObject;
        int turretIndex = clickedButton.transform.GetSiblingIndex();
        turrets[turretIndex].Upgrade();

        Player.instance.Pay(ShopManager.turretPrice);
        if (turrets[turretIndex].level == 3) clickedButton.SetActive(false);
    }

    public static Sprite GetLevelTurretSprite(int level) {
        return instance.turretLevelSprites[level - 1];
    }
}
