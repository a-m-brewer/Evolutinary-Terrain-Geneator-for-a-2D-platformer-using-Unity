using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private Grid grid;

	public Pathfinding(Grid _grid) 
    {
        this.grid = _grid;
    }

    public void FindPath(Vector2 startPos, Vector2 endPos)
    {
        Node startNode = grid.NodeAtPosition((int)startPos.x, (int)startPos.y);
        Node endNode = grid.NodeAtPosition((int)endPos.x, (int)endPos.y);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node node = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost <= node.fCost)
                {
                    if (openSet[i].hCost < node.hCost)
                    {
                        node = openSet[i];
                    }
                }
            }

            openSet.Remove(node);
            closedSet.Add(node);

            if (node == endNode)
            {
                RetraceSteps(startNode, endNode);
                return;
            }

            foreach(Node neighbour in grid.GetNeighbours(node))
            {
                if(!neighbour.Walkable || closedSet.Contains(neighbour))
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
