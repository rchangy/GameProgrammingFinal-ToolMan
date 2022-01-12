using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : Objective
{
    [SerializeField]
    private PlayableDirector playableDirector;
    private bool _isCompleted;
    private bool _started = false;
    [SerializeField] private Transform[] InactiveBeforeStart;

    public bool _skipEnable = true;

    public Transform cam1, cam2, AnotherCam;
    protected override void Init()
    {
        _isCompleted = false;
        playableDirector.Stop();
        foreach (var obj in InactiveBeforeStart)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public override void StartObjective()
    {
        foreach (var obj in InactiveBeforeStart)
        {
            obj.gameObject.SetActive(true);
        }

        _started = true;
        _uIController.SetControlEnable(false);
        _p1.controlEnable = false;
        _p2.controlEnable = false;
        

        uIController.SetBattleUI(false);
        uIController.SetHintUI(false);

        if (cam1 && cam2 && AnotherCam)
        {
            cam1.gameObject.SetActive(false);
            cam2.gameObject.SetActive(false);
            AnotherCam.gameObject.SetActive(true);
        }
        playableDirector.Play();
    }

    private void Update()
    {
        if (_started && Input.GetKeyDown(KeyCode.Escape) && _skipEnable)
        {
            playableDirector.Stop();
            if (cam1)
                cam1.gameObject.SetActive(true);
            if (cam2)
                cam2.gameObject.SetActive(true);
            if (AnotherCam)
                AnotherCam.gameObject.SetActive(false);
            Complete();
        }
    }

    public void Complete() {
        if (cam1 && cam2 && AnotherCam) {
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(true);
            AnotherCam.gameObject.SetActive(false);
        }

        _isCompleted = true;
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }
}
