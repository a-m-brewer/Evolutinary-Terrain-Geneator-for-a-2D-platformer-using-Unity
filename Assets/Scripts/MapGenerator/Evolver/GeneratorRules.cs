using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    private int populationSize = 100;
    private float mutationRate = 0.01f;
    private float maxGapSize = 9;
    private int startTileIndex = 2;


    public float MainChecker(int[] room)
    {

        float resRoomGround = 0f;
        float[] gapCheck = new float[2] { 1f, 0f };
        float hasStartingTile = IsTileGround(room[startTileIndex]);
        float enemiesOnFloor = 1f;

        for(int i = 0; i < room.Length; i++)
        {
            resRoomGround += RoomHasGroundEvaluator(i, room[i]);

            gapCheck = LegalGapCheck(i, gapCheck, room[i]);

            enemiesOnFloor = GetBellowEnemyIsFloor(i, room, enemiesOnFloor);

        }

        return  resRoomGround * gapCheck[0] * hasStartingTile * enemiesOnFloor;
    }
    
    private float[] LegalGapCheck(int index, float[] gapCheck, int tile)
    {
        if (index < TileInformation.roomSizeX)
        {
            gapCheck[1] = IncreaseGapSize(gapCheck[1], tile);
            gapCheck[0] = GetGapsWithinLimit(gapCheck[1], gapCheck[0]);
        }
        return gapCheck;
    }

    private float IncreaseGapSize(float prevGap, int tile)
    {
        return (tile == 6) ? (prevGap += 1f) : 0f;
    }

    private float GetGapsWithinLimit(float gapSize, float gapInLimit)
    {
        if (maxGapSize < gapSize)
        {
            return 0f;
        }

        return gapInLimit;
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

    public float IsTileGround(int tile)
    {
        return (tile == 1) ? 1f : 0f;
    }

    public int GetPopulationSize()
    {
        return populationSize;
    }

    private int GetNRowsBellowIndex(int index,int rowsBellow)
    {
        int rowBellowIndex = index - (TileInformation.roomSizeX * rowsBellow);
        return rowBellowIndex;
    }

    public float GetBellowEnemyIsFloor(int index, int[] room, float lastResult)
    {
        int newIndex = GetNRowsBellowIndex(index, 2);

        if (room[index] == 4 || room[index] == 5)
        {
            if (newIndex < 0)
            {
                return 0f;
            }

            if (room[newIndex] != 1)
            {
                return 0f;
            }
        }

        return lastResult;

    }



}
