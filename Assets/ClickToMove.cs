using UnityEngine;

public class ClickToMove : MonoBehaviour
{

    Color iniColor;
    Color hoverColor;
    Renderer rend;

    //GridNode[] highlightedNodes;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        iniColor = rend.material.color;
        hoverColor = Color.green;
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
        /*
        Pathfinder.shortestRoute(player.transform.position, transform.position);
        GridNode playerNode  = grid.NodeFromWorldPoint(player.transform.position);
        GridNode currentNode = grid.NodeFromWorldPoint(transform.position);
        while (!currentNode.Equals(playerNode))
        {
            GameObject.Find("(" + currentNode.gridX + ", " + currentNode.gridY + ")").GetComponent<ClickToMove>().rend.material.color = Color.green;
            currentNode = currentNode.parent;
        }
        */

    }

    void OnMouseExit()
    {
        rend.material.color = iniColor;
    }

    void OnMouseDown()
    {

        if (Input.GetMouseButtonDown(0) )//&& !player.GetComponent<Player>().isMoving)
        {
            Global.player.GetComponent<Player>().MoveTo(transform.position);
        }
    }

    private void OnDrawGizmos()
    {
        //if (Global.grid.displayGridGizmos)
        //{   Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(gameObject.transform.position, Vector3.one);
        //}
    }
}
