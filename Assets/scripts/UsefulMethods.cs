using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// convert a vector3 to vector2
public static class UsefulMethods {
    public static Vector2 ToVector2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }

    public static bool RandomChance(float chance)
    {
        return (Random.Range(0, 100) <= chance);
    }

    public static void Archive(this Room[] pop)
    {
        TextFileWriter t = new TextFileWriter();
        t.WriteRoomsToFile(pop);
    }
}
