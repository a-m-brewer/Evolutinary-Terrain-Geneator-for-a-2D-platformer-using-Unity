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

    /// <summary>
    /// This is where all the methods created in this document are combined.
    /// The idea is that due to the amount of checks that are done per evolution, the number of for loops
    /// used per run should be kept to a minimum. That is why here there is one big for loop that contains all the
    /// checks for this room.
    /// </summary>
    /// <param name="room">Current room</param>
    /// <returns></returns>
    public List<float> MainChecker(Room room)
    {
        List<float> evaluationResults = new List<float>();
        for(int i = 0; i < 8; i++)
        {
            evaluationResults.Add(0f);
        }

        List<Vector2> positionOfWorldItems = new List<Vector2>();

        ResetList();

        for (int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for(int x = 0; x < TileInformation.roomSizeX; x++)
            {
                positionOfWorldItems = FindWorldItems(positionOfWorldItems, room, x, y);

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


        Grid roomGrid = CreateGrid(room);
        int startY = FindPosY(roomGrid, 0);

        evaluationResults.Add(CanNavigateRoom(room, roomGrid, startY));

        return evaluationResults;
    }

    /// <summary>
    /// Get how often to mutate
    /// </summary>
    /// <returns></returns>
    public float GetMutationRate()
    {
        return this.mutationRate;
    }

    /// <summary>
    /// Reset the values of the platform list
    /// </summary>
    private void ResetList()
    {
        platformSizes = new List<int>();
        currIndex = -1;
        prevX = -1;
        prevY = -1;
    }

    /// <summary>
    /// Check if the tiles to the left and right of the player and it's feet are all not ground
    /// </summary>
    /// <param name="x">Current X coordinate</param>
    /// <param name="y">Current Y coordinate</param>
    /// <param name="room">Current room being evaluated</param>
    /// <returns></returns>
    private float AirAroundItem(int x, int y, Room room)
    {
        // Five tiles to check to score for each correct tile is 1/5 of 1
        float toReturn = 0f;
        float toAdd = 1f / 5f;

        for (int row = y - 1; row <= y; row++)
        {
            for (int col = x - 1; col <= x + 1; col++)
            {
                // if the current tile being check skip
                if (row == y && col == x)
                {
                    continue;
                }
                // If within map range and does not equal ground or gap add score
                if (WithinMapRange(col, row))
                {
                    if (!(room.Data[row, col] == 1) && !(room.Data[row, col] == 6))
                        toReturn += toAdd;
                } else
                // if outside of the map it doesn't matter so give score
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
    private float CanNavigateRoom(Room room, Grid grid, int _startY)
    {

        Pathfinding pf = new Pathfinding(grid);

        int startY = _startY;
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

    private Grid CreateGrid(Room room)
    {
        Grid grid = new Grid(room.Data);
        grid.CreateGrid();

        return grid;
    }

    /// <summary>
    /// Can the player get from one position in the map to another using A* Pathfinding.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="start_x"></param>
    /// <param name="start_y"></param>
    /// <param name="end_x"></param>
    /// <param name="end_y"></param>
    /// <returns></returns>
    private float CanNavigateToPoint(Room room, Grid grid, int start_x, int start_y, int end_x, int end_y)
    {
        if(!WithinMapRange(start_x, start_y) || !WithinMapRange(end_x, end_y))
        {
            return 0f;
        }

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

    /// <summary>
    /// Implementation of MATLABS gauss2mf function for finding if X is
    /// close to two mean values. With X's between mean1 and mean2 returning
    /// 1
    /// </summary>
    /// <param name="X">The item to find membership value of</param>
    /// <param name="len">distance between full membership and half membership</param>
    /// <param name="mean1">Lower value that will return full membership</param>
    /// <param name="mean2">Higher value that will return full membership</param>
    /// <returns></returns>
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

    /// <summary>
    /// Check for the current tiletype if it has a ground tile "tilesBellow" them.
    /// </summary>
    /// <param name="room">Current room being evaluated</param>
    /// <param name="tileType">What type of tile to check</param>
    /// <param name="tilesBellow">how many rows bellow to check</param>
    /// <param name="x">X of tile</param>
    /// <param name="y">Y of tile</param>
    /// <returns></returns>
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

    /// <summary>
    /// Count the length of each platform in the map and store it's value in a list.
    /// </summary>
    /// <param name="room">Current room being evaluated</param>
    /// <param name="currX">Current tiles X coordinate</param>
    /// <param name="currY">Current tiles Y coordinate</param>
    /// <param name="prevX">Previous tiles X coordinate</param>
    /// <param name="prevY">Previous tiles Y coordinate</param>
    private void CountPlatformTileLengths(Room room, int currX, int currY, int prevX, int prevY)
    {
        // if different row and current tile is ground start a new list item
        if (currY != prevY && room.Data[currY, currX] == 1)
        {
            platformSizes.Add(0);
            currIndex++;
        }

        // if previous tile in map range and on the same row and if current tile is ground and 
        // the previous tile is not. Start a new list item
        if (WithinMapRange(prevX, prevY)) { 
            if (currY == prevY && (room.Data[currY, currX] == 1 && room.Data[prevY, prevX] != 1))
            {
                platformSizes.Add(0);
                currIndex++;
            }
        }

        // if the current tile is ground increase the platform length.
        if(room.Data[currY, currX] == 1)
        {
            platformSizes[currIndex]++;
        }

    }

    /// <summary>
    /// Count the number of Traps, Coins and Enemies in a map and store their coordinates
    /// </summary>
    /// <param name="worldItemList">The list of c</param>
    /// <param name="room"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private List<Vector2> FindWorldItems(List<Vector2> worldItemList, Room room, int x, int y)
    {
        if(WithinMapRange(x, y))
        {
            if(IsWorldItem(room.Data[y, x]))
            {
                worldItemList.Add(new Vector2(x, y));
            }
        }
        return worldItemList;
    }

    /// <summary>
    /// is the current tile a Trap, coin or enemy
    /// </summary>
    /// <param name="tile"></param>
    /// <returns></returns>
    private bool IsWorldItem(int tile)
    {
        return tile != 0 && tile != 1 && tile != 6;
    }

}
