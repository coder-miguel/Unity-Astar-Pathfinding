using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinder
{
    static GameGrid grid = Global.grid;
    public bool HasPath { get; private set; }
    public Vector3[] Path { get; private set; }


    public Pathfinder(Vector3 from, Vector3 to)
    {
        Path = shortestRoute(from, to);
    }

    public Vector3[] shortestRoute(Vector3 start, Vector3 end)
    {

        Stopwatch sw = new Stopwatch();
        sw.Start();

        HasPath = false;
        Vector3[] waypoints = new Vector3[0];


        GridNode startNode = grid.NodeFromWorldPoint(start);
        GridNode targetNode = grid.NodeFromWorldPoint(end);

        if (targetNode.walkable)
        {
            Heap<GridNode>    openSet   = new Heap<GridNode>(grid.gridSizeX * grid.gridSizeY); // Sorted so that we search for the lowest cost path each time
            HashSet<GridNode> closedSet = new HashSet<GridNode>();


            startNode.gCost = 0;
            startNode.hCost = GetDistance(startNode, targetNode);
            openSet.Add(startNode);

            //while there are still qualified neighbors to check
            while (openSet.Count > 0)
            {
                GridNode currentNode = openSet.Pop();

                //add GridNode to the heap
                closedSet.Add(currentNode);

                //if the GridNode you are looking at is the target
                if (currentNode == targetNode)
                {
                    HasPath = true;
                    break;
                }

                //for each immediate neighbor of the current node
                foreach (GridNode neighbor in grid.GetNeighbors(currentNode))
                {
                    //if that GridNode is walkable and is not in the closed set/already the best path
                    if (neighbor.walkable && !closedSet.Contains(neighbor))
                    {
                        //calculate the new cost of movement
                        int newMovementCost = currentNode.gCost + 10;//GetDistance(currentNode, neighbor);

                        if (newMovementCost <= neighbor.gCost || !openSet.Contains(neighbor))
                        {
                            neighbor.gCost = newMovementCost;
                            neighbor.hCost = GetDistance(neighbor, targetNode);
                            neighbor.parent = currentNode;

                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                            else
                            {
                                openSet.Sort(neighbor);
                            }
                        }
                    }
                }//end foreach neighbor of the currentNode
            }//end while openSet is not empty
        }//end if start and end are walkable
        sw.Stop();
        UnityEngine.Debug.Log("Elapsed Time " + sw.ElapsedMilliseconds + "ms");
        if (HasPath)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        return waypoints;
    }

    static Vector3[] RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode>  listpath = new List<GridNode>();
        GridNode currentNode = endNode;

        while(currentNode != startNode)
        {
            listpath.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = PathArray(listpath);
        Array.Reverse(waypoints);
        return waypoints;
    }

    static Vector3[] PathArray(List<GridNode> listpath)
    {   List<Vector3> waypoints = new List<Vector3>();
        foreach (GridNode node in listpath)
        {
            waypoints.Add(node.worldPos);
        }
        return waypoints.ToArray();
    }

    static int GetDistance(GridNode nodeA, GridNode nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        int sqr = dstX * dstX + dstY * dstY;
        double guess = (dstX > dstY) ? ((14 * dstY + 10 * (dstX - dstY)) / 10) : (( 14 * dstX + 10 * (dstY - dstX)) / 10);

        return 10 * (int)(guess - ((guess * guess - sqr) / (2 * guess)));
    }

}
