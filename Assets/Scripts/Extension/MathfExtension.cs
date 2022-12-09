using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MathfExtension
{
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t));
    }

    public static Vector2 CosWave(Vector2 startPos, Vector2 targetPos, float minAmplitude, float wave, int face, float t)
    {
        float startX = startPos.x;
        float startY = startPos.y;
        float xDistance = targetPos.x - startX;
        float yDistance = targetPos.y - startY;
        float amplitude = Mathf.Abs(xDistance / 2);

        amplitude = Mathf.Clamp(amplitude, minAmplitude, 6f);

        float frequency = t * (Mathf.PI + (wave - 1) * Mathf.PI);
        float x = startX + (-Mathf.Cos(frequency) + 1) * amplitude * -face;
        float y = startY + yDistance * t;

        return new Vector2(x, y);
    }

    public static int GetHorizontalLineAngle(int value)
    {
        if (value > 90)
            return 180 - value;
        else if (value < -90)
            return 180 + value;
        return value;
    }

    public static T GetRandom<T>(List<T> list)
    {
        int r = Random.Range(0, list.Count);
        return list[r];
    }

    public static T GetRandom<T>(T targetA, T targetB)
    {
        int r = Random.Range(0, 2);
        if (r == 0)
            return targetA;
        else
            return targetB;
    }
}
