using System;
using UnityEngine;

public abstract class Request
{

    public Vector3 caster;
    public Vector3 target;
    public Action<Vector3[], bool> callback;

    public Request(Vector3 _caster, Vector3 _target, Action<Vector3[], bool> _callback)
    {
        caster = _caster;
        target = _target;
        callback = _callback;
    }

    public abstract void DoRequest(Action<Result> action);
}
//test