using System.Collections;
using UnityEngine;

public class Player : Entity
{
    Vector3[] path;
    int targetIndex;
    public bool isMoving;


    private void Start()
    {
        StartCoroutine(UpdatePath());
    }

    public void MoveTo(Vector3 newPosition)
    {
        target.position = newPosition;
        StopCoroutine(UpdatePath());
        isMoving = true;
        StartCoroutine(UpdatePath());
    }


    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator UpdatePath()
    {
        
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        //Vector3 waypoints = 
        Pathfinder moveRequest = new Pathfinder(transform.position, target.position, OnPathFound);
        RequestManager.SendRequest(moveRequest);
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        targetIndex = 0;
        while (true)
        {
            if (transform.position.Equals(currentWaypoint))
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    //isMoving = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawCube(path[i] + new Vector3(0, 0, 0), new Vector3(.9f, .9f, 1f));
            }
        }
    }
}
