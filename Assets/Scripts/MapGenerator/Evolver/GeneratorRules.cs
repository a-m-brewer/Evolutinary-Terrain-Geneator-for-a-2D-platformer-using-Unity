using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    // remember pop sizes need to be even
    private int initRandomPopulationSize = DefaultRuleArguments.populationSize;
    
    private float maxGapSize = 9;

    private Vector2 startTileIndex = new Vector2(2, 0);

    private float mutationRate;
    private int targetEnemies;
    private int maxCoins;
    private int maxTraps;

    private List<int[]> checkpoints;

    private List<int> platformSizes = new List<int>();
    private int currIndex = -1;
    int prevX = -1;
    int prevY = -1;


    public GeneratorRules(float _mutationRate, int _targetEnemies, int _maxCoins, int _maxTraps, List<int[]> _checkpoints)
    {
        this.mutationRate = _mutationRate;
        this.targetEnemies = _targetEnemies;
        this.maxCoins = _maxCoins;
        this.maxTraps = _maxTraps;
        this.checkpoints = _checkpoints;
    }

    public List<float> MainChecker(Room room)
    {
        List<float> evaluationResults = new List<float>();
        for(int i = 0; i < 8; i++)
        {
            evaluationResults.Add(0f);
        }

        ResetList();

        for (int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for(int x = 0; x < TileInformation.roomSizeX; x++)
            {
                if(room.Data[y, x] == 4 || room.Data[y,x] == 5)
                {
                    evaluationResults[0] += 1f;
                    evaluationResults[4] += AirAroundItem(x, y, room);
                }
                if(room.Data[y, x] == 2)
                {
                    evaluationResults[1] += 1f;
                }
                if(room.Data[y, x] == 3)
                {
                    evaluationResults[2] += 1f;
                }

                CountPlatformTileLengths(room, x, y, prevX, prevY);

                evaluationResults[3] += TileOnGroundIncrement(room.Data, 4, 2, x, y);
                evaluationResults[3] += TileOnGroundIncrement(room.Data, 5, 2, x, y);
                evaluationResults[5] += TileOnGroundIncrement(room.Data, 3, 1, x, y);

                prevX = x;
                prevY = y;
            }
        }

        for (int i = 0; i < platformSizes.Count; i++)
        {
            //                               current platform  len  g1  g2  
            evaluationResults[6] += Gauss2mf(platformSizes[i], 5f, 3f, 24f);
        }

        // count is the mean
        evaluationResults[3] = Gauss(evaluationResults[3], 20f, evaluationResults[0]);
        evaluationResults[4] = Gauss(evaluationResults[4], 20f, evaluationResults[0]);
        evaluationResults[5] = Gauss(evaluationResults[5], 20f, evaluationResults[2]);

        evaluationResults[0] = Gauss(evaluationResults[0], 20f, this.targetEnemies);
        evaluationResults[1] = Gauss(evaluationResults[1], 20f, this.maxCoins);
        evaluationResults[2] = Gauss(evaluationResults[2], 20f, this.maxTraps);
        evaluationResults[6] = Gauss(evaluationResults[6], 20f, platformSizes.Count);

        evaluationResults.Add(CanNavigateRoom(room));

        return evaluationResults;
    }

    public float GetMutationRate()
    {
        return this.mutationRate;
    }

    private void ResetList()
    {
        platformSizes = new List<int>();
        currIndex = -1;
        prevX = -1;
        prevY = -1;
    }

    private float AirAroundItem(int x, int y, Room room)
    {
        float toReturn = 0f;
        float toAdd = 1f / 5f;

        for (int row = y - 1; row <= y; row++)
        {
            for (int col = x - 1; col <= x + 1; col++)
            {
                if (row == y && col == x)
                {
                    continue;
                }

                if (WithinMapRange(col, row))
                {
                    if (!(room.Data[row, col] == 3) && !(room.Data[row, col] == 1) && !(room.Data[row, col] == 6))
                        toReturn += toAdd;
                } else
                {
                    toReturn += toAdd;
                }
            }
        }

        return toReturn;
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
            && grid.NodeAtPosition(TileInformation.roomSizeX - 1, endY).groundUnderSearchNode)
            && grid.NodeAtPosition(0, startY).TileAboveWalkable(0, startY, grid.WalkableGrid) 
            && grid.NodeAtPosition(TileInformation.roomSizeX - 1, endY).TileAboveWalkable(TileInformation.roomSizeX - 1, endY, grid.WalkableGrid))
        {
            return 0f;
        }

        pf.FindPath(new Vector2(0, FindPosY(grid, 0)), new Vector2(23, FindPosY(grid, 23)));
        grid.DrawPath();

        // if the a* pathfinder makes it to the target the map is navigatable for a player
        if(pf.foundpath)
        {
            return 1f;
        }

        return (Gauss(pf.distanceToEnd, 40f, 0f) == 1f) ? (Gauss(pf.distanceToEnd - 1f, 40f, 0f)) : (Gauss(pf.distanceToEnd, 40f, 0f));
    }

    private float CanNavigateToPoint(Room room, int start_x, int start_y, int end_x, int end_y)
    {
        if(!WithinMapRange(start_x, start_y) || !WithinMapRange(end_x, end_y))
        {
            return 0f;
        }

        Grid grid = new Grid(room.Data);
        grid.CreateGrid();

        Pathfinding pf = new Pathfinding(grid);

        pf.FindPath(new Vector2(start_x, start_y), new Vector2(end_x, end_y));
        grid.DrawPath();

        // if the a* pathfinder makes it to the target the map is navigatable for a player
        if (pf.foundpath)
        {
            return 1f;
        }

        return (Gauss(pf.distanceToEnd, 40f, 0f) == 1f) ? (Gauss(pf.distanceToEnd - 1f, 40f, 0f)) : (Gauss(pf.distanceToEnd, 40f, 0f));
    }

    /// <summary>
    /// Find a place to put start or end node
    /// </summary>
    /// <param name="grid"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    private int FindPosY(Grid grid, int x)
    {

        for(int y = 0; y < TileInformation.roomSizeY - 1; y++)
        {
            if(grid.WalkableGrid[y, x] == 1 && grid.WalkableGrid[y + 1, x] == 1)
            {
                return y;
            }
        }

        return TileInformation.roomSizeY - 2;
    }

    /// <summary>
    /// Check if the array index you want to access is in the array space
    /// </summary>
    /// <param name="x">x index</param>
    /// <param name="y">y index</param>
    /// <returns>if in range</returns>
    private bool WithinMapRange(int x, int y)
    {
        return (0 <= x && x < TileInformation.roomSizeX &&
                0 <= y && y < TileInformation.roomSizeY);
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
    /// Get the gausian disribution like the one in MATLAB gaussmf
    /// </summary>
    /// <param name="X">The number you want to find the membership of</param>
    /// <param name="len">length from the mean to the point at 0.5</param>
    /// <param name="mean">The mid point that will equal 1</param>
    /// <returns></returns>
    private float Gauss(float X, float len, float mean)
    {
        float top = -(Mathf.Pow((X - mean), 2f));
        float bot = 2 * (Mathf.Pow(len, 2f));
        float toPower = top / bot;

        float result = Mathf.Pow((float)System.Math.E, toPower);
        return result;
    }

    private float Gauss2mf(float X, float len, float mean1, float mean2)
    {
        float gauss1 = Gauss(X, len, mean1);
        float gauss2 = Gauss(X, len, mean2);

        if(mean1 <= X && X <= mean2)
        {
            return 1f;
        }
        else if(X < mean1)
        {
            return gauss1;
        }
        else if(mean2 < X)
        {
            return gauss2;
        }

        return 0f;
    }

    private float TileOnGroundIncrement(int[,] room, int tileType, int tilesBellow, int x, int y)
    {
        if (room[y, x] == tileType)
        {
            if (WithinMapRange(x, y - tilesBellow))
            {

                for(int z = y - 1; y - tilesBellow < z; z--)
                {
                    if(room[z, x] == 1  || room[z, x] == 3 || room[z, x] == 6)
                    {
                        return 0f;
                    }
                }

                if (room[y - tilesBellow, x] == 1)
                {
                    return 1f;
                }
            }
        }

        return 0f;
    }

    private void CountPlatformTileLengths(Room room, int currX, int currY, int prevX, int prevY)
    {
        if (currY != prevY && room.Data[currY, currX] == 1)
        {
            platformSizes.Add(0);
            currIndex++;
        }

        //if (room.Data[currY, currX] == 1 && !WithinMapRange(prevX, prevY))
        //{
        //    Debug.Log("Da two");
        //    platformSizes.Add(0);
        //    currIndex++;
        //}

        if (WithinMapRange(prevX, prevY)) { 
            if (currY == prevY && (room.Data[currY, currX] == 1 && room.Data[prevY, prevX] != 1))
            {
                platformSizes.Add(0);
                currIndex++;
            }
        }

        if(room.Data[currY, currX] == 1)
        {
            platformSizes[currIndex]++;
        }

    }

}
