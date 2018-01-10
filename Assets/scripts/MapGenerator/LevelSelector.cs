using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelector {

    public int CalculateRoomDifficulty(int[] roomData, Transform[] tileTypes)
    {
        int difficulty = 0;

        for (int i = 0; i < roomData.Length; i++)
        {
            Transform cur = tileTypes[roomData[i]];
            if(cur.gameObject.GetComponent<IDifficulty>() != null)
            {
                difficulty += TileInformation.difficultyScores[roomData[i]];
            }
        }
        return difficulty;
    }

    public int[] CalculateDifficultyOfEachRoom(int[][] allRoomsData, Transform[] tileTypes)
    {
        int[] diffScores = new int[allRoomsData.Length];

        for (int i = 0; i < allRoomsData.Length; i++)
        {
            diffScores[i] = CalculateRoomDifficulty(allRoomsData[i], tileTypes);
        }

        return diffScores;
    }
}
