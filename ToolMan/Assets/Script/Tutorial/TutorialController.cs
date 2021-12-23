using UnityEngine;
using System.Collections;

public class TutorialController : Objective
{
    private bool _isCompleted = false;

    public override void StartObjective()
    {
        
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    protected override void Init()
    {
        
    }
}
