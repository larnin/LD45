using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class MyMath
{
    public static float SignedMod(float a, float n)
    {
        return a - Mathf.Floor(a / n) * n;
    }

    public static float SignedDeltaAngleRad(float from, float to)
    {
        float a = to - from;
        return SignedMod((a + Mathf.PI), Mathf.PI * 2) - Mathf.PI;
    }

    public static float SignedDeltaAngle(float from, float to)
    {
        float a = to - from;
        return SignedMod((a + 180), 360) - 180;
    }

    public static float powInt(float value, int n)
    {
        float result = 1;

        for (int i = 0; i < n; i++)
            result *= value;

        return result;
    }

    public static int powInt(int value, int n)
    {
        int result = 1;

        for (int i = 0; i < n; i++)
            result *= value;

        return result;
    }
}