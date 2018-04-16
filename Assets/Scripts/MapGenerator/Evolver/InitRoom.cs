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
        25f, // ground
        20f, // gap
        1f, // coin
        2f, // trap
        2f, // enemy
        1f   // enemy with gun
    };

    public Room GenerateCompleteRandom()
    {
        int[,] randomRoom = new int[TileInformation.roomSizeY, TileInformation.roomSizeX];
        for (int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for (int x = 0; x < TileInformation.roomSizeX; x++)
            {
                randomRoom[y, x] = Random.Range(0, 6);
            }
        }
        return new Room(randomRoom);
    }

    public Room Generate()
    {
        int[,] randomRoom = new int[TileInformation.roomSizeY,TileInformation.roomSizeX];

        for (int y = 0; y < randomRoom.GetLength(0); y++)
        {
            for (int x = 0; x < randomRoom.GetLength(1); x++)
            {
                randomRoom[y, x] = SetTile(new Vector2(x, y));
            }
        }

        return new Room(randomRoom);
    }

    private int SetTile(Vector2 index)
    {
        // Start with a number that is not a tile
        int toPlace = ERROR_NO_TILE;
        // If it is on the lowest level choose between ground and gap tiles
        toPlace = InitGround((int)index.y);
        // Loop through each tile type and try to place one of them.
        for (int i = 0; i < chanceOfSpawning.Length; i++)
        {
            int tileType = i + 1;
            toPlace = TryPlaceTile(chanceOfSpawning[i], tileType, toPlace);
        }
        // if can't place a tile by random chance place the background tile
        if (toPlace == ERROR_NO_TILE)
            toPlace = BACKGROUND;
        // return new tile gene e.g. return background tile (0)
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
