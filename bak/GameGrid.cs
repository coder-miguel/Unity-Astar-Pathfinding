using System;
using System.Collections.Generic;
using UnityEngine;

/////////////////////////////////////////////////////////////////////////////////////////////////////
//
//      class GameGrid
//
public class GameGrid : MonoBehaviour
{
    //public vars
    public bool       displayGridGizmos;
    public LayerMask  unwalkableMask;
    public Vector2    gridWorldSize;
    public float      nodeRadius;

    //env vars
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    //class vars
    GridNode[,] grid;
    float   nodeDiameter;
    public int gridSizeX  { get; private set; }
    public int gridSizeY { get; private set; }


    void Awake()
    {   nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {   grid = new GridNode[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - 
            Vector3.right   * (gridWorldSize.x / 2) - 
            Vector3.up * (gridWorldSize.y / 2);

        for(int x = 0; x < gridSizeX; x++)
        {   for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + 
                    Vector3.right   * (x * nodeDiameter + nodeRadius) + 
                    Vector3.up * (y * nodeDiameter + nodeRadius);
                //set this to 0 ----------------------------------V
                bool walkable = !(Physics.CheckSphere(worldPoint, 0, unwalkableMask));
                grid[x, y] = new GridNode(walkable, worldPoint, x, y);
            }
        }
    }

    public List<GridNode> GetNeighbors(GridNode node)
    {   List<GridNode> neighbors = new List<GridNode>();

        for(int x = -1; x <= 1; x++)
        {   for (int y = -1; y <= 1; y++)
            {   if (!(x == 0 && y == 0) && !(x != 0 && y != 0))
                {
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    { 
                        neighbors.Add(grid[checkX, checkY]);
                    }
                }
            }
        } return neighbors;
    }

    public GridNode NodeFromWorldPoint(Vector3 worldPosition)
    {   float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x,y];
    }

    void OnClick()
    {
        Debug.Log( "was clicked");
    }
}
//
//      end GameGrid
//
/////////////////////////////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////////////////////////////
//
//      class GridNode
//
public class GridNode : IComparable<GridNode>
{
    public bool walkable;
    public Vector3 worldPos;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public GridNode parent;

    public GridNode(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;


        //Make a GridSquare Object for each Node in the Grid
        if (walkable)
        {
            GameObject newNode = UnityEngine.Object.Instantiate(GameObject.Find("GridSquare"));
            newNode.transform.position = worldPos + new Vector3(0, 0, 0);
            newNode.name = "(" + gridX + ", " + gridY + ")";
        }
    }
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(GridNode node)
    {
        int compare = fCost.CompareTo(node.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(node.hCost);
        }

        return compare;
    }

    public int Compare(GridNode x, GridNode y)
    {
        return x.Equals(y)?0:-1;
    }

    public bool Equals(GridNode x)
    {
        return gridX.Equals(x.gridX) && gridY.Equals(x.gridY);
    }

    public override String ToString()
    {
        return "GridNode: (" + gridX + ", " + gridY + ")";
    }

}
//
//    end GridNode
//
/////////////////////////////////////////////////////////////////////////////////////////////////////
