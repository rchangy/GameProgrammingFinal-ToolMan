﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyWavesController : Objective
{
    private List<EnemyWaveController> _waves;
    private HashSet<EnemyWaveController> _activeWaves;
    private int _nextWave = 0;
    private int _total;
    private int _completed = 0;

    private bool _isCompleted = false;

    private void Start()
    {
        _waves = transform.GetComponentsInChildren<EnemyWaveController>().ToList();
        foreach(EnemyWaveController wave in _waves)
        {
            wave.gameObject.SetActive(true);
            wave.SetWaves(this);
        }
        _total = _waves.Count;
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
        EnemyWaveController releasingWave = _waves[_nextWave];
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
