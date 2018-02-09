using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System;
using System.IO;

public class RulesTesting : MonoBehaviour {

    const int i = 24 * 10;

    public TextAsset huristicMaps;

    int[] testingMap = new int[i] 
        {
            1,1,1,1,1,1,1,1,1,6,6,6,6,1,1,1,1,1,6,6,6,6,6,6,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
        };

    InitPopulation ip = new InitPopulation();
    EvaluateRoom er = new EvaluateRoom(0.75f);

    // TODO: Cleanup this mess
    private void Start()
    {
        int[][] randomPopulation = ip.Generate(er.GetGroundPercent());
        float[] populationScores = er.EvaluatePopulation(randomPopulation);
        int[][] randomRandomRooms = SelectRandom(randomPopulation, 30);

        int[][] hLevels = LoadMaps(huristicMaps);
        float[] hScores = er.EvaluatePopulation(hLevels);
        int[][] randomHRooms = SelectRandom(hLevels, 10);

        int[][] toBeCrossedOverAndMutated = new int[randomHRooms.Length + randomRandomRooms.Length][];
        Array.Copy(randomRandomRooms, toBeCrossedOverAndMutated, randomRandomRooms.Length);
        Array.Copy(randomHRooms, 0, toBeCrossedOverAndMutated, randomRandomRooms.Length, randomHRooms.Length);

        TextFileWriter tfw = new TextFileWriter("boob");
        tfw.WriteRoomsToFile(toBeCrossedOverAndMutated);
    }

    // TODO: add top scores getter, if it can't reach target amount get as many as possible
    // TODO: if all scores are 0 pick random sample
    // TODO: Then mutate and copy

    public int[][] LoadMaps(TextAsset inFile)
    {
        string[][] levels = new string[1][];
        int[][] levelsInt = new int[1][];

        if (inFile != null)
        {
            string[] wholeLevels = (inFile.text.Split('.'));
            levels = new string[wholeLevels.Length][];
            levelsInt = new int[wholeLevels.Length][];

            for (int i = 0; i < wholeLevels.Length; i++)
            {
                levels[i] = wholeLevels[i].Split(',');
                levelsInt[i] = new int[levels[i].Length];
                for (int j = 0; j < levels[i].Length; j++)
                {
                    int.TryParse(levels[i][j], out levelsInt[i][j]);
                }
            }

        }
        return levelsInt;
    }

    private int[][] SelectRandom(int[][] rooms, int amount)
    {
        int[][] randomMaps = new int[amount][];
        for (int i = 0; i < amount; i++)
        {
            int indexRandom = UnityEngine.Random.Range(0, rooms.Length - 1);
            randomMaps[i] = rooms[indexRandom];
        }
        return randomMaps;
    }

}
