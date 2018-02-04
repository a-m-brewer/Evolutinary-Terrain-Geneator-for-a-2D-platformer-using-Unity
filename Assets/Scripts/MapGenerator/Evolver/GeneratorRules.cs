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
        float gapsWithinLimit = 1f;

        for(int i = 0; i < room.Length; i++)
        {
            resRoomGround += RoomHasGroundEvaluator(i, room[i]);

            if (i < TileInformation.roomSizeX)
            {
                gapSize = (room[i] == 6) ? (gapSize += 1) : 0;
                if (maxGapSize < gapSize)
                {
                    gapsWithinLimit = 0f;
                }
            }
           

        }
        // score + score2 / number of scores
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

    // somting is wrong

    public float RoomSizeInLimit(int gapSize)
    {
        if (maxGapSize < gapSize)
        {
            return 0f;
        }
        return 1f;
    }

    public int GetPopulationSize()
    {
        return populationSize;
    }


    

}
