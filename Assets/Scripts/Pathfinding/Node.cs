using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Influenced by 
/// https://github.com/SebLague/Pathfinding/tree/master/Episode%2004%20-%20heap/Assets/Scripts
/// TODO: Remember to reference
/// </summary>
public class Node : IHeapItem<Node> {

    private bool walkable;
    public bool Walkable { get { return this.walkable; } set { this.walkable = value; } }
    private int x;
    public int X { get { return this.x; } set { x = value; } }
    private int y;
    public int Y { get { return this.y; } set { y = value; } }

    public int gCost;
    public int hCost;
    public Node parent;

    private int jumpValue = 0;
    public int JumpValue { get { return this.jumpValue; } set { this.jumpValue = value; } }
    public int lowestJumpValue = short.MaxValue;

    private int heapIndex;
    public int HeapIndex { get { return this.heapIndex; } set { this.heapIndex = value; } }

    bool groundUnderSearchNode = true;

    public Node(int[,] room, int x, int y)
    {
        this.walkable = IsWalkable(x,y,room);
        this.X = x;
        this.Y = y;
    }

    private bool IsWalkable(int x, int y, int[,] room)
    {
        int currTile = room[y, x];
        groundUnderSearchNode = GroundUnderSearchTile(x, y, room);
        bool currTileWalkable = (!(currTile == 1) && !(currTile == 3) && !(currTile == 6));

        bool toReturn = currTileWalkable;// && groundUnderSearchNode; 
        //    toReturn = toReturn || CanMoveUp(); //&& groundUnderSearchNode;

        return toReturn;
    }

    private bool GroundUnderSearchTile(int x, int y, int[,] room)
    {
        if (y - 1 < 0)
        {
            return false;
        }

        return room[y - 1, x] == 1;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public void CalculateJumpValue(Node node)
    {
        if(this.groundUnderSearchNode)
        {
            this.jumpValue = 0;
            return;
        }

        if(node.Y == this.Y)
        {
            this.jumpValue = parent.jumpValue + 1;
            return;
        }

        if (node.Y < this.Y)
        {
            this.jumpValue = NextEven(parent.jumpValue);
            return;
        }

        if (node.Y > this.Y)
        {
            this.jumpValue = NextEven(parent.jumpValue);
            return;
        }

    }

    private int NextEven(int n)
    {
        int toReturn = n++;
        if(toReturn % 2 != 0)
        {
            toReturn++;
        }
        return toReturn;
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public bool CanMoveUp()
    {
        return (this.jumpValue < PlayerMovementRestrictions.maxJumpHeight);
    }

    public bool NodeAboveAnother(Node n)
    {
        return (this.Y > n.Y); 
    }

    public bool NodeOnRight(Node n)
    {
        return (this.X > n.X);
    }

    public bool NodeOnLeft(Node n)
    {
        return (this.X < n.X);
    }

    public bool NodeOnXAxis(Node n)
    {
        return NodeOnLeft(n) || NodeOnRight(n);
    }

    public bool CanMoveOnXAxis()
    {
        return (this.jumpValue % 2) == 0;
    }

    public void SetLowestValue(int value)
    {
        lowestJumpValue = (lowestJumpValue < value) ? lowestJumpValue : value;
    }
}
