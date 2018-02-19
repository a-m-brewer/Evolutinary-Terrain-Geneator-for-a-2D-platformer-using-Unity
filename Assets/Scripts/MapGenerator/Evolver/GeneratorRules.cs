using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    // remember pop sizes need to be even
    private int initRandomPopulationSize = 80;
    private int populationSize = 100;
    
    private float maxGapSize = 9;

    private Vector2 startTileIndex = new Vector2(2, 0);

    public int PopulationSize { get { return this.populationSize; } }

    private float mutationRate;
    private int targetEnemies;
    private int maxCoins;
    private int maxTraps;

    public GeneratorRules(float _mutationRate, int _targetEnemies, int _maxCoins, int _maxTraps)
    {
        this.mutationRate = _mutationRate;
        this.targetEnemies = _targetEnemies;
        this.maxCoins = _maxCoins;
        this.maxTraps = _maxTraps;
    }

    ///// <summary>
    ///// 0: % ground to target
    ///// 1: all gaps in the map floor are in limit
    ///// 2: Length of gap (only used for tracking)
    ///// 3: There is a path through the level
    ///// 4: check if enemies on the map have a floor to spawn on
    ///// </summary>
    //public float[] MainChecker(Room room)
    //{
    //    float[] evaluationResults = new float[5];
    //    evaluationResults[0] = 0f;
    //    evaluationResults[1] = 1f;
    //    evaluationResults[2] = 0f;
    //    evaluationResults[3] = CanNavigateRoom(room);
    //    evaluationResults[4] = 1f;

    //    float[] gapCheck = new float[2] { evaluationResults[1], evaluationResults[2] };


    //    for (int y = 0; y < room.Data.GetLength(0); y++)
    //    {
    //        for (int x = 0; x < room.Data.GetLength(1); x++)
    //        {
    //            evaluationResults[0] += RoomHasGroundEvaluator(y, room.Data[y, x]);
    //            gapCheck = LegalGapCheck(x, gapCheck, room.Data[y, x]);
    //            evaluationResults[1] = gapCheck[0];
    //            evaluationResults[2] = gapCheck[1];

    //            evaluationResults[4] = GetBellowEnemyIsFloor(new Vector2(x, y), room.Data, evaluationResults[4]); ;
    //        }
    //    }
    //    return  evaluationResults;
    //}
    public float[] MainChecker(Room room)
    {
        float[] evaluationResults = new float[4];
        evaluationResults[0] = CanNavigateRoom(room);

        int enemyCount = 0;
        int enemyWithGunCount = 0;
        int coinCount = 0;
        int trapCount = 0;

        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for(int x = 0; x < TileInformation.roomSizeX; x++)
            {
                enemyCount = CountNumberOfID(room.Data, x, y, 4, enemyCount);
                enemyWithGunCount = CountNumberOfID(room.Data, x, y, 5, enemyWithGunCount);
                coinCount = CountNumberOfID(room.Data, x, y, 2, coinCount);
                trapCount = CountNumberOfID(room.Data, x, y, 3, trapCount);
            }
        }
        Debug.Log(enemyCount);
        enemyCount = enemyCount + enemyWithGunCount;

        return evaluationResults;
    }


    /// <summary>
    /// How many rooms to generate per round
    /// </summary>
    /// <returns>size of the population</returns>
    public int GetInitRandomPopulationSize()
    {
        return initRandomPopulationSize;
    }

    private bool WithinMapRange(int x, int y)
    {
        return (0 <= x && x < TileInformation.roomSizeX &&
                0 <= y && y < TileInformation.roomSizeY);
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
        // how long it is until the end TODO: Mabye chance logic
        return 1f / pf.distanceToEnd;
    }

    private int FindPosY(Grid grid, int x)
    {

        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            if(grid.WalkableGrid[y, x] == 1)
            {
                return y;
            }
        }

        return TileInformation.roomSizeY - 1;
    }

    private int CountNumberOfID(int[,] _room, int x, int y, int idToCheck, int lastResult)
    {
        if(_room[y, x] == idToCheck)
        {
            return lastResult + 1;
        }
        return lastResult;
    }

}
