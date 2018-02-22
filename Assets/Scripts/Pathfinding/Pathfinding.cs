using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private Grid grid;
    public bool foundpath = false;
    public int distanceToEnd = 0;

	public Pathfinding(Grid _grid) 
    {
        this.grid = _grid;
    }

    int i = 0;
    public void FindPath(Vector2 startPos, Vector2 endPos)
    {

        Node startNode = grid.NodeAtPosition((int)startPos.x, (int)startPos.y);
        Node endNode = grid.NodeAtPosition((int)endPos.x, (int)endPos.y);

        distanceToEnd = GetDistance(startNode, endNode);
        // return if start or end inaccessable
        if (grid.WalkableGrid[(int)startPos.y, (int)startPos.x] == 0
            || grid.WalkableGrid[(int)endPos.y, (int)endPos.x] == 0
            || startNode.TileAboveWalkable(startNode.X, startNode.Y, grid.room))
        {
            return;
        }

        startNode.JumpValue = 0;

        Heap<Node> openSet = new Heap<Node>(TileInformation.roomSizeX * TileInformation.roomSizeY);
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            i++;
            Node node = openSet.RemoveFirst();

            closedSet.Add(node);

            distanceToEnd = GetDistance(node, endNode);

            if (node == endNode)
            {
                foundpath = true;
                RetraceSteps(startNode, endNode);
                return;
            }

            foreach(Node neighbour in grid.GetVonNeumannNeighbourhood(node))
            {

                if (neighbour.NodeAboveAnother(node))
                {
                    if (!node.CanMoveUp())
                    {
                        continue;
                    }
                }

                if (neighbour.NodeOnXAxis(node))
                {
                    if (!node.CanMoveOnXAxis())
                    {
                        continue;
                    }
                }


                if (!neighbour.Walkable || !neighbour.TileAboveWalkable(neighbour.X, neighbour.Y, grid.room))
                {
                    continue;
                }

                if (closedSet.Contains(neighbour))
                {
                    if(neighbour.NodeAboveAnother(node))
                    {
                        // if was falling in the past
                        if(neighbour.pastSeekerStatus.Contains(SeekerStatus.Falling))
                        {
                            continue;
                        }
                        // if was jumping
                        if(neighbour.pastSeekerStatus.Contains(SeekerStatus.Jumping))
                        {
                            int nextJumpValue = neighbour.CalculateJumpValue(neighbour.parent.Peek());
                            bool canGo = true;

                            foreach(int usedJumpValue in neighbour.usedJumpValues)
                            {
                                if (nextJumpValue >= usedJumpValue)
                                {
                                    canGo = false;
                                    break;
                                }
                            }

                            if(!canGo)
                            {
                                continue;
                            } else
                            {
                                closedSet.Remove(neighbour);
                            }
                        } else
                        {
                            closedSet.Remove(neighbour);
                        }

                    }
                    else
                    {
                        continue;
                    }
                }

                int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                if(newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    
                    if(!neighbour.parent.Contains(node))
                    {
                        neighbour.parent.Push(node);
                        if (neighbour.parent.Count >= 3)
                        {
                            neighbour.parent.Pop();
                        }
                    }

                    if (neighbour.NodeAboveAnother(node))
                    {
                        neighbour.JumpValue = neighbour.CalculateJumpValue(neighbour.parent.Peek());
                        neighbour.seekerStatus = SeekerStatus.Jumping;
                        neighbour.pastSeekerStatus.Add(SeekerStatus.Jumping);
                        if(!neighbour.usedJumpValues.Contains(neighbour.JumpValue))
                        {
                            neighbour.usedJumpValues.Add(neighbour.JumpValue);
                        }
                    }
                    else if(neighbour.groundUnderSearchNode)
                    {
                        if(!neighbour.pastSeekerStatus.Contains(SeekerStatus.Grounded))
                        {
                            neighbour.seekerStatus = SeekerStatus.Grounded;
                        }
                        neighbour.pastSeekerStatus.Add(SeekerStatus.Grounded);
                    }
                    else
                    {
                        neighbour.seekerStatus = SeekerStatus.Falling;
                        if(!neighbour.pastSeekerStatus.Contains(SeekerStatus.Falling))
                        {
                            neighbour.pastSeekerStatus.Add(SeekerStatus.Falling);
                        }
                        neighbour.JumpValue = neighbour.CalculateJumpValue(neighbour.parent.Peek());
                    }

                    if(!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    } else
                    {
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
        Node lastParent = null;

        while(currNode != start)
        {
            path.Add(currNode);
            if( currNode.parent.Count <= 0)
            {
                currNode.parent.Push(lastParent);
            }
            lastParent = currNode;
            currNode = currNode.parent.Pop();
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
