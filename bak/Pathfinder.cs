using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinder : Request
{
    GameGrid grid = GameObject.Find("A*").GetComponent<GameGrid>();


    public Pathfinder(Vector3 _caster, Vector3 _target, Action<Vector3[], bool> _callback): base(_caster, _target, _callback)
    {
        caster = _caster;
        target = _target;
        callback = _callback;
    }

    public static Vector3[] shortestRoute(Vector3 start, Vector3 end)
    {
        GameGrid grid = GameObject.Find("A*").GetComponent<GameGrid>();

        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        GridNode startNode = grid.NodeFromWorldPoint(start);
        GridNode targetNode = grid.NodeFromWorldPoint(end);

        if (targetNode.walkable)
        {
            Heap<GridNode> openSet = new Heap<GridNode>(grid.gridSizeX * grid.gridSizeY); // Sorted so that we search for the lowest cost path each time
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
                    pathSuccess = true;
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
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        return waypoints;
    }


    public override void DoRequest(Action<Result> callback)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] waypoints   = new Vector3[0];
        bool      pathSuccess = false;

        GridNode startNode  = grid.NodeFromWorldPoint(caster);
        GridNode targetNode = grid.NodeFromWorldPoint(target);

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
                    pathSuccess = true;
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
        if(pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
            pathSuccess = waypoints.Length > 0;
        }
        callback(new Result(waypoints, pathSuccess, this.callback));
    }

    static Vector3[] RetracePath(GridNode startNode, GridNode endNode)
    {
        List<GridNode>  path = new List<GridNode>();
        GridNode currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = PathArray(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    static Vector3[] PathArray(List<GridNode> path)
    {   List<Vector3> waypoints = new List<Vector3>();
        foreach (GridNode node in path)
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
