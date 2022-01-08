using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScnene : Objective
{
    private bool _iscompleted = false;

    GameManager gameManager;

    public override bool isCompleted()
    {
        return _iscompleted;
    }

    public override void StartObjective()
    {
        
    }

    protected override void Init()
    {
        
    }
}
