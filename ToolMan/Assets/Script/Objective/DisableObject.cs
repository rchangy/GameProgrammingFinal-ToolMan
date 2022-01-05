using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableObject : Objective
{
    private bool _isCompleted = false;
    public List<GameObject> objs;

    public override bool isCompleted()
    {
        return _isCompleted;

    }

    public override void StartObjective()
    {
        foreach (GameObject o in objs) o.SetActive(false);
        _isCompleted = true;
    }

    protected override void Init()
    {
        
    }
}
