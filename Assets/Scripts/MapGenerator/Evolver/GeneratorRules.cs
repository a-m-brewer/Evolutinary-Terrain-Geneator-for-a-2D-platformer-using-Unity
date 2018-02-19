using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    // remember pop sizes need to be even
    private int initRandomPopulationSize = 80;
    private int populationSize = 100;
    private float mutationRate = 1f;
    private float maxGapSize = 9;

    private Vector2 startTileIndex = new Vector2(2, 0);

    public int PopulationSize { get { return this.populationSize; } }

    /// <summary>
    /// 0: % ground to target
    /// 1: all gaps in the map floor are in limit
    /// 2: Length of gap (only used for tracking)
    /// 3: There is a path through the level
    /// 4: check if enemies on the map have a floor to spawn on
    /// </summary>
    public float[] evaluationResults = new float[5];

    public float[] MainChecker(Room room)
    {

        evaluationResults[0] = 0f;
        evaluationResults[1] = 1f;
        evaluationResults[2] = 0f;
        evaluationResults[3] = CanNavigateRoom(room);
        evaluationResults[4] = 1f;

        float[] gapCheck = new float[2] { evaluationResults[1], evaluationResults[2] };


        for (int y = 0; y < room.Data.GetLength(0); y++)
        {
            for (int x = 0; x < room.Data.GetLength(1); x++)
            {
                evaluationResults[0] += RoomHasGroundEvaluator(y, room.Data[y, x]);
                gapCheck = LegalGapCheck(x, gapCheck, room.Data[y, x]);
                evaluationResults[1] = gapCheck[0];
                evaluationResults[2] = gapCheck[1];

                evaluationResults[4] = GetBellowEnemyIsFloor(new Vector2(x, y), room.Data, evaluationResults[4]); ;
            }
        }
        return  evaluationResults;
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

        if (index == 0)
        {
            if (tile == 1)
            {
                return perGroundFound;
            }
        }

        return 0f;
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
    public int GetInitRandomPopulationSize()
    {
        return initRandomPopulationSize;
    }

    /// <summary>
    /// Get the index of N rows bellow the tile inputed
    /// </summary>
    /// <param name="index">index you want to find the tile bellow</param>
    /// <param name="rowsBellow">how many rows down</param>
    /// <returns></returns>
    private Vector2 GetNRowsBellowIndex(Vector2 index,int rowsBellow)
    {
        Vector2 rowBellowIndex = new Vector2(index.x, index.y - rowsBellow);
        return rowBellowIndex;
    }

    /// <summary>
    /// Makes sure that all enemies spawn above a ground tile
    /// </summary>
    /// <param name="index">which tile in the room</param>
    /// <param name="room">the room data</param>
    /// <param name="lastResult">return value</param>
    /// <returns></returns>
    public float GetBellowEnemyIsFloor(Vector2 index, int[,] room, float lastResult)
    {
        Vector2 newIndex = GetNRowsBellowIndex(index, 2);

        if (room[(int)index.y, (int)index.x] == 4 ||
            room[(int)index.y, (int)index.x] == 5)
        {
            if (newIndex.y < 0)
            {
                return 0f;
            }

            if (room[(int)newIndex.y, (int)newIndex.x] != 1)
            {
                return 0f;
            }
        }
        
        return lastResult;

    }

    public float GetMutationRate()
    {
        return this.mutationRate;
    }

    /// <summary>
    /// Use a* pathfinding that take gravity and player attributes into account in order
    /// to find if the player could get through the level
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    private float CanNavigateRoom(Room room)
    {
        Grid grid = new Grid(room.Data);
        grid.CreateGrid();

        Pathfinding pf = new Pathfinding(grid);

        int startY = FindPosY(grid, 0);
        int endY = FindPosY(grid, TileInformation.roomSizeX - 1);

        // there is nowhere for the player to stand at the start and end of rooms
        if (!(grid.NodeAtPosition(0, startY).groundUnderSearchNode 
            && grid.NodeAtPosition(TileInformation.roomSizeX - 1, endY).groundUnderSearchNode)) {
            return 0f;
        }

        pf.FindPath(new Vector2(0, FindPosY(grid, 0)), new Vector2(23, FindPosY(grid, 23)));
        grid.DrawPath();

        // if the a* pathfinder makes it to the target the map is navigatable for a player
        if(pf.foundpath)
        {
            return 1f;
        }

        return 0f;
    }

    private int FindPosY(Grid grid, int x)
    {
        //int y = -1;
        //while (grid.WalkableGrid[y, x] != 1 && y < TileInformation.roomSizeY)
        //{
        //    y++;
        //    Debug.Log(y);
        //}

        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            if(grid.WalkableGrid[y, x] == 1)
            {
                return y;
            }
        }

        return TileInformation.roomSizeY - 1;
    }

}
