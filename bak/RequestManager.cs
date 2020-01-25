using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RequestManager:MonoBehaviour
{
    static RequestManager instance;
    Queue<Result>   results = new Queue<Result>();

    private void Awake()
    {
        instance = this;
    }

    public static void SendRequest(Request request)
    {
        //ThreadStart threadStart = delegate {
            request.DoRequest(instance.FinishedProcessing);
        //};
        //threadStart.Invoke();

    }

    private void Update()
    {
        if (results.Count > 0)
        {
            int itemsInQueue = results.Count;
            lock (results)
            {
                for(int i = 0; i < itemsInQueue; i++)
                {
                    Result result = results.Dequeue();
                    result.callback(result.affectedCells, result.success);
                }
            }
        }
    }

    public void FinishedProcessing(Result result)
    {
        lock (results)
        {
            results.Enqueue(result);
        }
    }
}

public struct Result
{
    public Vector3[] affectedCells;
    public bool success;
    public Action<Vector3[], bool> callback;

    public Result(Vector3[] _affectedCells, bool _success, Action<Vector3[], bool> _callback)
    {
        affectedCells = _affectedCells;
        success = _success;
        callback = _callback;
    }
}


