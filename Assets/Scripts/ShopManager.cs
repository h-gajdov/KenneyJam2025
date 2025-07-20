using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {
    public static int turretPrice = 30;
    public static int satelliteShieldPrice = 20;
    public static int repairCorePrice = 50;

    public void RepairCore() {
        if (Player.instance.coins < repairCorePrice || PowerCore.instance.health == PowerCore.instance.startHealth) return;

        PowerCore.Heal(PowerCore.instance.startHealth / 10f);
        Player.instance.Pay(repairCorePrice);
    }
}
