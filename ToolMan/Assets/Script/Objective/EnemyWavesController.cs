using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using ToolMan.Combat;

public class EnemyWavesController : Objective
{
    private List<EnemyWaveController> _waves;
    private HashSet<EnemyWaveController> _activeWaves;
    private int _nextWave = 0;
    private int _total = 0;
    private int _completed = 0;

    private bool _isCompleted;
    public CombatManager combatManager;

    protected override void Init()
    {
        GameObject cmObject = GameObject.Find("CombatManager");
        if (cmObject != null)
        {
            combatManager = cmObject.GetComponent<CombatManager>();
        }
        else
        {
            Debug.Log("combatManager is null");
        }
        _waves = transform.GetComponentsInChildren<EnemyWaveController>(true).ToList();
        if(_waves != null)
        {
            foreach(EnemyWaveController wave in _waves)
            {
                wave.gameObject.SetActive(false);
                wave.SetWaves(this);
            }
            _waves.Sort((x, y) => x.Order.CompareTo(y.Order));
            _total = _waves.Count;
            _activeWaves = new HashSet<EnemyWaveController>();
        }
    }

    public override void StartObjective()
    {
        uIController.SetBattleUI(true);
        uIController.SetControlEnable(true);
        _p1.controlEnable = true;
        _p2.controlEnable = true;
        ReleaseNextWave();
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    public void ReleaseNextWave()
    {
        if(_waves == null || _waves.Count <= 0) return;
        EnemyWaveController releasingWave = _waves[_nextWave];
        releasingWave.gameObject.SetActive(true);
        releasingWave.StartWave();
        _activeWaves.Add(releasingWave);
        _nextWave++;
    }

    public void WaveEnd(EnemyWaveController wave)
    {
        if (_activeWaves.Contains(wave)){
            _activeWaves.Remove(wave);
            _completed++;
        }
        if(_completed == _total)
        {
            FinishWaves(); 
        }
        else
        {
            ReleaseNextWave();
        }
    }

    private void FinishWaves()
    {
        _isCompleted = true;
    }


}
