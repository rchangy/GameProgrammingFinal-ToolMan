using UnityEngine;
using System.Collections;

public class DialogController : Objective
{
    private RPGTalk rpgTalk;

    [SerializeField] private int _startLine;
    [SerializeField] private int _endLine;

    private bool _isCompleted = false;

    void Start()
    {
        rpgTalk = gameObject.GetComponent<RPGTalk>();
    }

    public override void StartObjective()
    {
        rpgTalk.NewTalk(_startLine.ToString(), _endLine.ToString());
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    public void CompleteDialog()
    {
        _isCompleted = true;
    }
}
