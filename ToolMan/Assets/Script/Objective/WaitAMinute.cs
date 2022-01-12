using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAMinute : Objective
{
    public float waitTime = 0f;
    private bool _isCompleted;
    public override bool isCompleted()
    {
        return _isCompleted;
    }

    public override void StartObjective()
    {
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        _isCompleted = true;
    }

    protected override void Init()
    {
        _isCompleted = false;
    }
}
