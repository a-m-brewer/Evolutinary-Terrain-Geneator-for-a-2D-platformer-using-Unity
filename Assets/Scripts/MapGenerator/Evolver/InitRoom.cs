using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom {

    private const int ERROR_NO_TILE = -1;
    private const int BACKGROUND = 0;
    private const int GROUND = 1;
    private const int GAP = 6;
    private const int SPAWN_POINT_INDEX = 2;

    private float[] chanceOfSpawning = new float[6]
    {
        40f, // ground
        20f, // gap
        20f, // coin
        20f, // trap
        10f, // enemy
        5f   // enemy with gun
    };

    public int[] Generate()
    {
        int[] randomRoom = new int[TileInformation.roomSizeX * TileInformation.roomSizeY];

        for (int i = 0; i < randomRoom.Length; i++)
        {
            randomRoom[i] = SetTile(i);


        }

        return randomRoom;
    }

    private int SetTile(int index)
    {
        int toPlace = ERROR_NO_TILE;
        for (int i = 0; i < chanceOfSpawning.Length; i++)
        {
            int tileType = i + 1;
            toPlace = TryPlaceTile(chanceOfSpawning[0], tileType, toPlace);
        }

        if (index < TileInformation.roomSizeX && toPlace == ERROR_NO_TILE)
            toPlace = GAP;

        if (toPlace == ERROR_NO_TILE)
            toPlace = BACKGROUND;

        if (index == SPAWN_POINT_INDEX)
            toPlace = GROUND;

        return toPlace;
    }

    private int TryPlaceTile(float chance, int tileType, int prevValue)
    {
        if(RandomChance(chance) && prevValue == ERROR_NO_TILE)
        {
            return tileType;
        }
        // return a number that is not a tile
        return prevValue;
    }

    private int GetRandomTile()
    {
        return Random.Range(0, TileInformation.difficultyScores.Length - 1);
    }

    private bool RandomChance(float chance)
    {
        return (Random.Range(0, 100) <= chance);
    }

}
