using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRules { 

    private int numGenerations = 1;
    // remember pop sizes need to be even
    private int initRandomPopulationSize = 80;
    
    private float maxGapSize = 9;

    private Vector2 startTileIndex = new Vector2(2, 0);

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

    public float[] MainChecker(Room room)
    {
        float[] evaluationResults = new float[6];

        evaluationResults[0] = CanNavigateRoom(room);
        

        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for(int x = 0; x < TileInformation.roomSizeX; x++)
            {
                if(room.Data[y, x] == 4 || room.Data[y,x] == 5)
                {
                    evaluationResults[1] += 1f;
                    //evaluationResults[5] += AirAroundItem(x, y, room);
                }
                if(room.Data[y, x] == 2)
                {
                    evaluationResults[2] += 1f;
                }
                if(room.Data[y, x] == 3)
                {
                    evaluationResults[3] += 1f;
                }


                evaluationResults[4] += TileOnGroundIncrement(room.Data, 4, 2, x, y);
                evaluationResults[4] += TileOnGroundIncrement(room.Data, 5, 2, x, y);

            }
        }
        // count is the mean
        evaluationResults[4] = Gauss(evaluationResults[4], 20f, evaluationResults[1]);
        //evaluationResults[5] = Gauss(evaluationResults[5], 20f, evaluationResults[1]);
        evaluationResults[1] = Gauss(evaluationResults[1], 20f, this.targetEnemies);
        evaluationResults[2] = Gauss(evaluationResults[2], 20f, this.maxCoins);
        evaluationResults[3] = Gauss(evaluationResults[3], 20f, this.maxTraps);

        return evaluationResults;
    }

    public float GetMutationRate()
    {
        return this.mutationRate;
    }

    private float AirAroundItem(int x, int y, Room room)
    {
        float toReturn = 0f;
        float toAdd = 1f / 8f;

        for (int yi = y - 1; yi < y + 1; y++)
        {
            for(int xi = x - 1; xi < x + 1; x++)
            {
                if(0 <= xi && xi < TileInformation.roomSizeX &&
                    0 <= yi && yi < TileInformation.roomSizeY)
                {
                    if(xi == 0 && yi == 0)
                    {
                        continue;
                    }

                    if((!(room.Data[yi, xi] == 1) && !(room.Data[yi, xi] == 3) && !(room.Data[yi, xi] == 6)))
                    {
                        toReturn += toAdd;
                    }
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

    private float TileOnGroundIncrement(int[,] room, int tileType, int tilesBellow, int x, int y)
    {
        if (room[y, x] == tileType)
        {
            if (WithinMapRange(x, y - tilesBellow))
            {
                if (room[y - tilesBellow, x] == 1)
                {
                    return 1f;
                }
            }
        }

        return 0f;
    }
}
