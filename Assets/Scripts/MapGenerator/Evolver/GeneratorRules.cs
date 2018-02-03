using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    private int populationSize = 100;
    private float mutationRate = 0.01f;
    private int maxGapSize = 9;


    public float MainChecker(int[] room)
    {

        float resRoomGround = 0f;
        int gapSize = 0;
        float gapsWithinLimit = 0f;

        for(int i = 0; i < room.Length; i++)
        {
            resRoomGround += RoomHasGroundEvaluator(i, room[i]);


            gapSize = GetGapSize(gapSize, i, room[i]);
            Debug.Log(gapSize);
            gapsWithinLimit = RoomSizeInLimit(gapSize);


        }

        return gapsWithinLimit;
    }

    // closer to 1f the more ground
    public float RoomHasGroundEvaluator(int index, int tile)
    {
        float perGroundFound = 1f / (TileInformation.roomSizeX);

        if(index < TileInformation.roomSizeX)
        {
            if (tile == 1)
            {
                return perGroundFound;
            }
        }

        return 0f;
    }

    // returns 0 if the player would not be able to jump the gap
    public float RoomsGapsWithinLimit(int[] room)
    {
        int gapSize = 0;

        for (int i = 0; i < TileInformation.roomSizeX; i++)
        {
            if (maxGapSize < gapSize)
                return 0f;
            if (room[i] == 6)
                gapSize++;
            if (room[i] != 6)
                gapSize = 0;
        }

        return 1f;
    }

    // somting is wrong

    public float RoomSizeInLimit(int gapSize)
    {
        if (maxGapSize < gapSize)
        {
            return 0f;
        }
        return 1f;
    }

    public int GetGapSize(int gapSize ,int index, int tile)
    {
        if (index < TileInformation.roomSizeX)
        {
            if(tile == 6)
            {
                return gapSize++;
            }
        }

        return 0;
    }

    public int GetPopulationSize()
    {
        return populationSize;
    }


    

}
