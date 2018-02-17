using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private Grid grid;
    public bool foundpath = false;

	public Pathfinding(Grid _grid) 
    {
        this.grid = _grid;
    }

    public void FindPath(Vector2 startPos, Vector2 endPos)
    {
        // return if start or end inaccessable
        if(grid.WalkableGrid[(int) startPos.y,(int) startPos.x] == 0
            || grid.WalkableGrid[(int)endPos.y, (int)endPos.x] == 0)
        {
            return;
        }

        Node startNode = grid.NodeAtPosition((int)startPos.x, (int)startPos.y);
        Node endNode = grid.NodeAtPosition((int)endPos.x, (int)endPos.y);

        startNode.JumpValue = 0;

        Heap<Node> openSet = new Heap<Node>(TileInformation.roomSizeX * TileInformation.roomSizeY);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node node = openSet.RemoveFirst();

            if (node.parent != null)
                node.CalculateJumpValue(node.parent);

            node.SetLowestValue(node.JumpValue);

            closedSet.Add(node);

            if (node == endNode)
            {
                foundpath = true;
                RetraceSteps(startNode, endNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours2(node))
            {

                if (neighbour.NodeAboveAnother(node))
                {
                    if (!node.CanMoveUp())
                    {
                        neighbour.Walkable = false;
                    }
                }

                if(neighbour.NodeOnXAxis(node))
                {
                    if (!node.CanMoveOnXAxis())
                    {
                        neighbour.Walkable = false;
                    }
                }


                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = node;

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    } else
                    {
                        // try here for adding new node?
                        openSet.UpdateItem(neighbour);
                    }
                }
            }

        }
    }


    private void RetraceSteps(Node start, Node end)
    {
        List<Node> path = new List<Node>();
        Node currNode = end;

        while(currNode != start)
        {
            path.Add(currNode);
            currNode = currNode.parent;
        }
        path.Reverse();

        grid.path = path;
    }

    private int GetDistance(Node _node1, Node _node2)
    {
        int distX = Mathf.Abs(_node1.X - _node2.X);
        int distY = Mathf.Abs(_node1.Y - _node2.Y);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }

        return 14 * distX + 10 * (distY - distX);
    }
}
