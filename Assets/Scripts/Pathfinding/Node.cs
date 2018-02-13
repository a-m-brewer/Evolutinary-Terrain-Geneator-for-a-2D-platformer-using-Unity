using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    private bool walkable;
    public bool Walkable { get { return this.walkable; } }
    private int x;
    public int X { get { return this.x; } set { x = value; } }
    private int y;
    public int Y { get { return this.y; } set { y = value; } }

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(int tile, int x, int y)
    {
        this.walkable = IsWalkable(tile);
        this.X = x;
        this.Y = y;
    }

    private bool IsWalkable(int tile)
    {
        bool currTileWalkable = (!(tile == 1) && !(tile == 3) && !(tile == 6));
        return currTileWalkable;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
