using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public static float moveSpeed = 10f;
    public static float distanceToStop = 25f;
    public Rigidbody2D rb;

    Vector3 targetDestination;
    float totalDistance;

    private void Start() {
        targetDestination = transform.position.normalized * distanceToStop;
        totalDistance = Vector3.Distance(transform.position, targetDestination);
        transform.rotation = GameMath.GetLookAtRotation(transform.position, Vector3.zero, Vector3.forward, 90f);
    }

    private void FixedUpdate() {
        float distanceFromDestination = Vector3.Distance(transform.position, targetDestination);
        float percentToDestination = 1f - Mathf.Clamp01(distanceFromDestination / totalDistance);
        float speed = moveSpeed * GameMath.ReductionFunctionValueForEnemy(percentToDestination);
        if (speed < 0.1) speed = 0;
        
        rb.velocity = transform.up * speed;
    }

    //private void OnDrawGizmosSelected() {
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(Vector3.zero, distanceToStop);
    //}
}
