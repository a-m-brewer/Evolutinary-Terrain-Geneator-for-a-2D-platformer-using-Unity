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

    public static void ArchiveRoom(this int[,] array)
    {
        TextFileWriter t = new TextFileWriter();
        t.Write2DArrayToFile(array);
    }

    private static System.Random rng = new System.Random();

    public static List<T> Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }
}
