using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom {


    public int[] Generate()
    {
        int[] randomRoom = new int[TileInformation.roomSizeX * TileInformation.roomSizeY];

        for (int i = 0; i < randomRoom.Length; i++)
        {
            randomRoom[i] = GetRandomTile();
        }

        Debug.Log(randomRoom.Length);

        return randomRoom;
    }

    private int GetRandomTile()
    {
        return Random.Range(0, TileInformation.difficultyScores.Length - 1);
    }
}
