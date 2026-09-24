using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public static class MovementUtils
    {
    public static bool IsInRange(Transform a, Transform b, float range)
    {
        return (a.position - b.position).sqrMagnitude <= range * range;
    }
}

