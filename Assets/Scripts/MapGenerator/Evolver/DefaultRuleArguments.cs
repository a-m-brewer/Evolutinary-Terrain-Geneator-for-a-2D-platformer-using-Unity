using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DefaultRuleArguments
{
    public static float mutationRate = 0.5f;
    public static int targetEnemies = 4;
    public static int maxCoins = 6;
    public static int maxTraps = 4;
    public static int populationSize = 100;
    public static List<int[]> checkpoints = new List<int[]>()
    {
        //new int[2] {2, 2},
        //new int[2] {7, 5},
        //new int[2] {15, 1}
        new int[2] {9,0}
    };
}
