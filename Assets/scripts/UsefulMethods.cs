﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// convert a vector3 to vector2
public static class UsefulMethods {
    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
}