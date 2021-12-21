using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyWaveController : MonoBehaviour
{
    private EnemyWavesController _waves;

    private List<Enemy> _enemies;

    private int total = 0;
    private int currentAlive;

    private void Start()
    {
        _enemies = transform.GetComponentsInChildren<Enemy>().ToList();
        if(_enemies != null)
        {
            foreach(Enemy enemy in _enemies)
            {
                enemy.gameObject.SetActive(true);
                enemy.SetWave(this);
                enemy.gameObject.SetActive(false);
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
        foreach (Enemy enemy in _enemies)
        {
            enemy.gameObject.SetActive(true);
        }
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
