using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoom {

    private const int ERROR_NO_TILE = -1;
    private const int BACKGROUND = 0;
    private const int GROUND = 1;
    private const int GAP = 6;

    private float[] chanceOfSpawning = new float[6]
    {
        10f, // ground
        20f, // gap
        1f, // coin
        1f, // trap
        0.5f, // enemy
        0.25f   // enemy with gun
    };

    public Room GenerateCompleteRandom(EvaluateRoom evaluateRoom)
    {
        int[,] randomRoom = new int[TileInformation.roomSizeY, TileInformation.roomSizeX];
        for (int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for (int x = 0; x < TileInformation.roomSizeX; x++)
            {
                randomRoom[y, x] = Random.Range(0, 6);
            }
        }
        return new Room(randomRoom, evaluateRoom);
    }

    public Room Generate(EvaluateRoom evaluateRoom)
    {
        int[,] randomRoom = new int[TileInformation.roomSizeY,TileInformation.roomSizeX];

        for (int y = 0; y < randomRoom.GetLength(0); y++)
        {
            for (int x = 0; x < randomRoom.GetLength(1); x++)
            {
                randomRoom[y, x] = SetTile(new Vector2(x, y));
            }
        }

        return new Room(randomRoom, evaluateRoom);
    }

    private int SetTile(Vector2 index)
    {
        int toPlace = ERROR_NO_TILE;
        
        toPlace = InitGround((int)index.y);
        
        for (int i = 0; i < chanceOfSpawning.Length; i++)
        {
            int tileType = i + 1;
            toPlace = TryPlaceTile(chanceOfSpawning[i], tileType, toPlace);
        }

        if (index.x < TileInformation.roomSizeX && toPlace == ERROR_NO_TILE)
            toPlace = GAP;

        if (toPlace == ERROR_NO_TILE)
            toPlace = BACKGROUND;

        if (index == TileInformation.spawnPoint)
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

    private int InitGround(int y)
    {
        if (y == 0)
        {
            if(RandomChance(chanceOfSpawning[1]))
            {
                return GAP;
            } else
            {
                return GROUND;
            }
        }

        return ERROR_NO_TILE;
        
    }

}
