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

        uIController.SetBattleUI(false);
        uIController.SetHintUI(false);
        
        playableDirector.Play();
    }

    public void Complete() { _isCompleted = true; Debug.Log("cutScene:))), p1_controlenable = " + _p1.controlEnable); }

    public override bool isCompleted()
    {
        return _isCompleted;
    }
}
