using UnityEngine;
using System.Collections;
using System;

public struct Circle {
    public float Radius;
    public Vector2 Center;

    public Circle(float radius, Vector2 center) {
        Radius = radius;
        Center = center;
    }

    public bool Contains(Vector2 point) {
        return Math2d.Pow2(point.x - Center.x) + Math2d.Pow2(point.y - Center.y) < Math2d.Pow2(Radius);
    }
}

public class Math2d {
    public static float Pow2(float number) { return number * number; }
    public static float Pow3(float number) { return number * number * number; }
}
