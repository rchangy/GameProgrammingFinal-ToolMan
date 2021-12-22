using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyWavesController : Objective
{
    private List<EnemyWaveController> _waves;
    private HashSet<EnemyWaveController> _activeWaves;
    private int _nextWave = 0;
    private int _total = 0;
    private int _completed = 0;

    private bool _isCompleted;

    protected override void Init()
    {
        _waves = transform.GetComponentsInChildren<EnemyWaveController>().ToList();
        if(_waves != null)
        {
            foreach(EnemyWaveController wave in _waves)
            {
                wave.gameObject.SetActive(true);
                wave.SetWaves(this);
            }
            _waves.Sort((x, y) => x.Order.CompareTo(y.Order));
            _total = _waves.Count;
            _activeWaves = new HashSet<EnemyWaveController>();
        }
    }

    public override void StartObjective()
    {
        ReleaseNextWave();
    }

    public override bool isCompleted()
    {
        return _isCompleted;
    }

    public void ReleaseNextWave()
    {
        if(_waves == null) return;
        EnemyWaveController releasingWave = _waves[_nextWave];
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
