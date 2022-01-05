using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyWaveController : MonoBehaviour
{
    private EnemyWavesController _waves;

    private List<Enemy> _enemies;

    private int total = 0;
    private int currentAlive;

    public int Order;

    private void Init()
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
            Debug.Log("test " + currentAlive);
        }
    }
    private void Update()
    {
        Debug.Log("alive = " + currentAlive);
        if(currentAlive == 0)
        {
            Init();
            // report to enemy waves manager
            if(currentAlive == 0 && _waves != null)
            {
                _waves.WaveEnd(this);
            }
            //gameObject.SetActive(false);
        }
    }

    public void StartWave()
    {
        Init();
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
