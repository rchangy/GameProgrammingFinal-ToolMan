﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyWaveController : MonoBehaviour
{
    private EnemyWavesController _waves;

    private List<Enemy> _enemies;

    private int total = 0;
    private int currentAlive;

    public int Order;

    private void Start()
    {
        
        _enemies = transform.GetComponentsInChildren<Enemy>().ToList();
        if(_enemies != null)
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.SetWave(this);
            }
            total = _enemies.Count;
            currentAlive = total;
        }
    }
    private void Update()
    {
        if(currentAlive == 0)
        {
            // report to enemy waves manager
            if(_waves != null)
            {
                _waves.WaveEnd(this);
            }
            gameObject.SetActive(false);
        }
    }

    public void StartWave()
    {
        if (_enemies == null) return;
        gameObject.SetActive(true);
        _waves.combatManager.SetHPCanvas(_enemies);
    }

    public void EnemyDie()
    {
        currentAlive--;
    }

    public void SetWaves(EnemyWavesController waves)
    {
        _waves = waves;
    }
}
