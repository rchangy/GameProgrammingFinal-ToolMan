using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoll : Objective
{
    public float startY;
    public float endY;
    public float Span;
    public float waitTime;
    public GameObject scroller;

    public GameObject gameSetting;
    public GameObject cam;

    private float TimePassed = 0;
    private bool _started = false;
    private bool _stopped = false;
    
    private bool _isCompleted = false;

    // Update is called once per frame
    void Update()
    {
        if (!_started)
            return;

        if (TimePassed < Span)
        {
            float y = (startY * (Span - TimePassed) + endY * (TimePassed)) / Span;
            scroller.transform.position = new Vector3(scroller.transform.position.x, y, scroller.transform.position.z);
            TimePassed += Time.deltaTime;
        }

        else if (!_stopped)
        {
            _stopped = true;
            StartCoroutine(WaitUntilEnd());
        }
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    public override void StartObjective()
    {
        //gameSetting.SetActive(false);
        //cam.SetActive(true);
        scroller.SetActive(true);
        TimePassed = 0;
        _started = true;
    }

    protected override void Init()
    {
        scroller.transform.position = new Vector3(scroller.transform.position.x, startY, scroller.transform.position.z);
    }

    IEnumerator WaitUntilEnd() {
        yield return new WaitForSeconds(waitTime);
        _isCompleted = true;
    }
}
