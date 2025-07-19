using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Pickup {
    public float value = 0f;

    public override void Effect() {
        Player.instance.AddCoins(value);
        PickupIndicator indicator = Instantiate(pickupIndicator, UIManager.CanvasTransform, true).GetComponent<PickupIndicator>();
        indicator.transform.localPosition = Vector3.up * 100f;
        indicator.SetInfo(icon, $"+{value}");
        Destroy(indicator.gameObject, 2f);
    }
}
