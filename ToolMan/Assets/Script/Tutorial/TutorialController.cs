using UnityEngine;
using System.Collections;
using ToolMan.UI;
using UnityEngine.Playables;

public abstract class TutorialController : Objective
{
    public ToolMan.UI.Notification notification;
    public PlayableDirector timelineDirector;

    protected RPGTalk rpgTalk;
    protected bool _isTalkEnd = true;
    private bool _isCompleted = false;

    public override void StartObjective()
    {
        if (timelineDirector != null)
        {
            timelineDirector.Pause();
        }
        StartCoroutine(StartTutorial());
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    protected override void Init()
    {
        rpgTalk = gameObject.GetComponent<RPGTalk>();
        notification.canvasGroup.alpha = 0;
    }

    protected virtual IEnumerator StartTutorial()
    {
        yield return TutorialProcess();
        _isCompleted = true;
        if (timelineDirector != null)
        {
            timelineDirector.Resume();
        }
    }
    protected abstract IEnumerator TutorialProcess();

    public void TalkEnd()
    {
        _isTalkEnd = true;
    }
}
