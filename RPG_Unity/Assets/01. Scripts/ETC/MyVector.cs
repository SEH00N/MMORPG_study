using System;
using UnityEngine;

public class MyVector
{
    public float x;
    public float y;
    public float z;

    public float magnitude => Mathf.Sqrt(x * x + y * y + z * z);
    public MyVector normalized => new MyVector(x / magnitude, y / magnitude, z / magnitude);

    public MyVector(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }

    public static MyVector operator+(MyVector a, MyVector b)
        => new MyVector(a.x + b.x, a.y + b.y, a.z + b.z);
    public static MyVector operator-(MyVector a, MyVector b)
        => new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
    public static MyVector operator+(MyVector a, float b)
        => new MyVector(a.x * b, a.y * b, a.z * b);
}
