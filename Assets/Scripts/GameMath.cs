using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameMath {
    public static Vector2 GetRandomPointOnArc(Vector2 center, float radius, float startAngleDeg, float endAngleDeg) {
        float angle = Random.Range(startAngleDeg, endAngleDeg);
        float rad = angle * Mathf.Deg2Rad;

        float x = center.x + radius * Mathf.Cos(rad);
        float y = center.y + radius * Mathf.Sin(rad);

        return new Vector2(x, y);
    }

    public static Quaternion GetLookAtRotation(Vector3 source, Vector3 destination) {
        return GetLookAtRotation(source, destination, Vector3.forward, -90);
    }

    public static Quaternion GetLookAtRotation(Vector3 source, Vector3 destination, Vector3 axis, float offset = 0) {
        Vector3 lookDirection = (source - destination).normalized;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(axis * (angle + offset));
    }

    public static float ReductionFunctionValueForEnemy(float t) {
        if (t > 1) return 0;
        if (t < 0) return 1;
        return -Mathf.Pow(t, 46) + 1;
    }

    public static float ReductionFunctionForHealthBar(float x) {
        return 1 - x * x;
    }
}
