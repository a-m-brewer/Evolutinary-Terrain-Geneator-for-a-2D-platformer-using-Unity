using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid {

    private const int WALKABLE = 1;
    private const int NOT_WALKABLE = 0;
    private const int PATH = 2;

    private int[,] room = new int[TileInformation.roomSizeY, TileInformation.roomSizeX];
    private Node[,] grid = new Node[TileInformation.roomSizeY, TileInformation.roomSizeX];
    private int[,] walkableGrid = new int[TileInformation.roomSizeY, TileInformation.roomSizeX];

    public int[,] WalkableGrid { get { return this.walkableGrid; } }

    public List<Node> path;

    public Grid(int[,] roomData)
    {
        this.room = roomData;
        CreateGrid();
    }

    public void CreateGrid()
    {
        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for(int x = 0; x < TileInformation.roomSizeX; x++)
            {
                int tile = room[y, x];
                grid[y, x] = new Node(room, x, y);
                
                if (grid[y, x].Walkable)
                {
                    walkableGrid[y, x] = WALKABLE;
                } else
                {
                    walkableGrid[y, x] = NOT_WALKABLE;
                }

            }
        }
    }

    public void DrawPath()
    {
        for(int y = 0; y < TileInformation.roomSizeY; y++)
        {
            for (int x = 0; x < TileInformation.roomSizeX; x++)
            {
                if (path != null)
                {
                    if (path.Contains(grid[y,x]))
                    {
                        walkableGrid[y, x] = PATH;
                    }
                }
            }
        }
    }

    public Node NodeAtPosition(int x, int y)
    {
        return grid[y, x];
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.X + x;
                int checkY = node.Y + y;

                if (checkX >= 0 && checkX < TileInformation.roomSizeX && checkY >= 0 && checkY < TileInformation.roomSizeY)
                {
                    neighbours.Add(grid[checkY, checkX]);
                }
            }
        }

        return neighbours;
    }

    public List<Node> GetNeighbours2(Node node)
    {
        List<Node> output = new List<Node>();

        int leftX = node.X - 1;
        int leftY = node.Y;
        if (leftX >= 0 && leftX < TileInformation.roomSizeX && leftY >= 0 && leftY < TileInformation.roomSizeY)
        {
            output.Add(grid[leftY, leftX]);
        }

        int rightX = node.X + 1;
        int rightY = node.Y;
        if (rightX >= 0 && rightX < TileInformation.roomSizeX && rightY >= 0 && rightY < TileInformation.roomSizeY)
        {
            output.Add(grid[rightY, rightX]);
        }

        int upX = node.X;
        int upY = node.Y + 1;
        if (upX >= 0 && upX < TileInformation.roomSizeX && upY >= 0 && upY < TileInformation.roomSizeY)
        {
            output.Add(grid[upY, upX]);
        }

        int downX = node.X;
        int downY = node.Y - 1;
        if (downX >= 0 && downX < TileInformation.roomSizeX && downY >= 0 && downY < TileInformation.roomSizeY)
        {
            output.Add(grid[downY, downX]);
        }

        return output;
    }
}
