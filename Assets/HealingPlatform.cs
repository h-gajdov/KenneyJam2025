using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPlatform : MonoBehaviour {
    public float healRate = 1f;
    public GameObject pickupIndicator;
    public Sprite indicatorIcon;

    float nextTimeToHeal;

    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log("Called");
        if(other.CompareTag("Player") && Time.time >= nextTimeToHeal) {
            Player.instance.health = (Player.instance.health < Player.instance.startHealth) ? 10 + Player.instance.health : Player.instance.startHealth;
            PickupIndicator indicator = Instantiate(pickupIndicator, UIManager.CanvasTransform, true).GetComponent<PickupIndicator>();
            indicator.transform.localPosition = Vector3.up * 100f;
            indicator.SetInfo(indicatorIcon, "+");
            
            nextTimeToHeal = Time.time + healRate;
        }
    }
}
