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

    public static float SignedDeltaAngle(float from, float to)
    {
        float a = to - from;
        return SignedMod((a + 180), 360) - 180;
    }
}