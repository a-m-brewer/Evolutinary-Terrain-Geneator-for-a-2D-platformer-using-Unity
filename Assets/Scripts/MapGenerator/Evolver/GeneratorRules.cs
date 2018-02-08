using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    private int populationSize = 100;
    private float mutationRate = 0.01f;
    private float maxGapSize = 9;
    private int startTileIndex = 2;

    /// <summary>
    /// 0: how much of the floor is a ground tile
    /// 1: all gaps in the map floor are in limit
    /// 2: Length of gap (only used for tracking)
    /// 3: tile 3 is a ground tile or not
    /// 4: check if enemies on the map have a floor to spawn on
    /// </summary>
    public float[] evaluationResults = new float[5];

    public float[] MainChecker(int[] room)
    {

        evaluationResults[0] = 0f;
        evaluationResults[1] = 1f;
        evaluationResults[2] = 0f;
        evaluationResults[3] = IsTileGround(room[startTileIndex]);
        evaluationResults[4] = 1f;

        float[] gapCheck = new float[2] { evaluationResults[1], evaluationResults[2] };

        for(int i = 0; i < room.Length; i++)
        {
            evaluationResults[0] += RoomHasGroundEvaluator(i, room[i]);

            gapCheck = LegalGapCheck(i, gapCheck, room[i]);
            evaluationResults[1] = gapCheck[0];
            evaluationResults[2] = gapCheck[1];

            evaluationResults[4] = GetBellowEnemyIsFloor(i, room, evaluationResults[4]);

        }

        return  evaluationResults;
    }
    
    /// <summary>
    /// Check if a gap in a room is a jumpable distance for the player
    /// 0f = not suitable
    /// 1f = suitable
    /// </summary>
    /// <param name="index">which tile in the map</param>
    /// <param name="gapCheck">results of the function</param>
    /// <param name="tile">data from that tile in the map</param>
    /// <returns></returns>
    private float[] LegalGapCheck(int index, float[] gapCheck, int tile)
    {
        if (index < TileInformation.roomSizeX)
        {
            gapCheck[1] = IncreaseGapSize(gapCheck[1], tile);
            gapCheck[0] = GetGapsWithinLimit(gapCheck[1], gapCheck[0]);
        }
        return gapCheck;
    }

    /// <summary>
    /// Increases the gap size if the current tile is gap tile
    /// sets to if not gap set to 0
    /// </summary>
    /// <param name="prevGap"></param>
    /// <param name="tile"></param>
    /// <returns></returns>
    private float IncreaseGapSize(float prevGap, int tile)
    {
        return (tile == 6) ? (prevGap += 1f) : 0f;
    }

    /// <summary>
    /// if the gapsize is larger than the players jump give a score of 0
    /// </summary>
    /// <param name="gapSize">current size of the gap</param>
    /// <param name="gapInLimit">the return value</param>
    /// <returns></returns>
    private float GetGapsWithinLimit(float gapSize, float gapInLimit)
    {
        if (maxGapSize < gapSize)
        {
            return 0f;
        }

        return gapInLimit;
    }

    /// <summary>
    /// increase the mesurement of percentage of bottom row that is ground
    /// </summary>
    /// <param name="index">which tile in the room</param>
    /// <param name="tile">the tiles data</param>
    /// <returns></returns>
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

    /// <summary>
    /// return 1 if that tile is ground
    /// </summary>
    /// <param name="tile">data of a tile in a room</param>
    /// <returns></returns>
    public float IsTileGround(int tile)
    {
        return (tile == 1) ? 1f : 0f;
    }

    /// <summary>
    /// How many rooms to generate per round
    /// </summary>
    /// <returns>size of the population</returns>
    public int GetPopulationSize()
    {
        return populationSize;
    }

    /// <summary>
    /// Get the index of N rows bellow the tile inputed
    /// </summary>
    /// <param name="index">index you want to find the tile bellow</param>
    /// <param name="rowsBellow">how many rows down</param>
    /// <returns></returns>
    private int GetNRowsBellowIndex(int index,int rowsBellow)
    {
        int rowBellowIndex = index - (TileInformation.roomSizeX * rowsBellow);
        return rowBellowIndex;
    }

    /// <summary>
    /// Makes sure that all enemies spawn above a ground tile
    /// </summary>
    /// <param name="index">which tile in the room</param>
    /// <param name="room">the room data</param>
    /// <param name="lastResult">return value</param>
    /// <returns></returns>
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
