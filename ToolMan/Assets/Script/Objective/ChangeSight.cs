using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSight : Objective
{
    [SerializeField]
    private Camera targetCam;
    [SerializeField]
    private Camera p1Cam;
    [SerializeField]
    private Camera p2Cam;

    private bool _isCompleted;

    protected override void Init()
    {
        _isCompleted = false;
    }

    public override void StartObjective()
    {
        p1Cam.enabled = false;
        p2Cam.enabled = false;
        targetCam.enabled = true;
        _isCompleted = true;
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }
}
