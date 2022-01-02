using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : Objective
{
    [SerializeField]
    private PlayableDirector playableDirector;
    private bool _isCompleted;
    protected override void Init()
    {
        _isCompleted = false;
        playableDirector.Stop();
    }

    public override void StartObjective()
    {
        _p1.controlEnable = false;
        _p2.controlEnable = false;
        _uIController.SetControlEnable(false);
        //Debug.Log("hi");
        playableDirector.Play();
    }

    public void Complete() { _isCompleted = true; Debug.Log("cutScene:)))"); }

    public override bool isCompleted()
    {
        return _isCompleted;
    }
}
