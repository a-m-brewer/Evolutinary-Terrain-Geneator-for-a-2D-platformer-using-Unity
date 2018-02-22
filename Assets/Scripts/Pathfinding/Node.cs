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
    

    private int jumpValue = 0;
    public int JumpValue { get { return this.jumpValue; } set { this.jumpValue = value; } }

    private int heapIndex;
    public int HeapIndex { get { return this.heapIndex; } set { this.heapIndex = value; } }

    public bool groundUnderSearchNode = true;

    // from thing
    public Stack<Node> parent;
    public SeekerStatus seekerStatus;
    public ArrayList usedJumpValues;
    public ArrayList pastSeekerStatus;

    public Node(int[,] room, int x, int y)
    {
        this.walkable = IsWalkable(x,y,room);
        this.X = x;
        this.Y = y;
        this.jumpValue = 0;
        parent = new Stack<Node>();
        usedJumpValues = new ArrayList();
        pastSeekerStatus = new ArrayList();
        seekerStatus = SeekerStatus.Default;
    }

    private bool IsWalkable(int x, int y, int[,] room)
    {
        int currTile = room[y, x];
        groundUnderSearchNode = GroundUnderSearchTile(x, y, room);
        bool currTileWalkable = (!(currTile == 1) && !(currTile == 3) && !(currTile == 6));
        bool tileAboveWalkable = TileAboveWalkable(x, y, room);
        bool toReturn = currTileWalkable; //&& tileAboveWalkable;

        return toReturn;
    }

    public bool TileAboveWalkable(int x, int y, int[,] room) 
    {
        if(TileInRoomRange(x, y + 1))
        {
            int tileAbove = room[y + 1, x];
            return (!(tileAbove == 1) && !(tileAbove == 3) && !(tileAbove == 6));
        } else if(TileInRoomRange(x, y))
        {
            return true;
        }

        return false;
    }

    private bool TileInRoomRange(int x, int y)
    {
        return (0 <= x && x < TileInformation.roomSizeX &&
                0 <= y && y < TileInformation.roomSizeY);
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

    public int CalculateJumpValue(Node node)
    {
        int newJump = 0;
        if(this.groundUnderSearchNode)
        {
            newJump = 0;
        }

        if(node.Y == this.Y)
        {
            newJump = node.jumpValue + 1;
        }

        if (node.Y < this.Y)
        {
            newJump = NextEven(node.jumpValue);
        }

        if (node.Y > this.Y)
        {
            newJump = NextEven(node.jumpValue);
        }
        return newJump;
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
        return (this.jumpValue < PlayerMovementRestrictions.maxJumpHeightValue);
    }

    public bool NodeAboveAnother(Node n)
    {
        return (this.Y > n.Y); 
    }

    public bool NodeUnderAnother(Node n)
    {
        return (this.Y < n.Y);
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

}
