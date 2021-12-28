using UnityEngine;
using System.Collections;

public abstract class TutorialController : Objective
{
    protected RPGTalk rpgTalk;
    private bool _isCompleted = false;

    protected bool _isTalkEnd = true;

    public override void StartObjective()
    {
        StartCoroutine(StartTutorial());
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    protected override void Init()
    {
        rpgTalk = gameObject.GetComponent<RPGTalk>();
    }

    protected virtual IEnumerator StartTutorial()
    {
        yield return TutorialProcess();
        _isCompleted = true;
    }
    protected abstract IEnumerator TutorialProcess();

    public void TalkEnd()
    {
        _isTalkEnd = true;
    }
}
