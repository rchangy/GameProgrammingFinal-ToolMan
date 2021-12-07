using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace ToolMan.Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField]
        private CombatModel _model;
        public CombatModel Model
        {
            get => _model;
        }

        private List<PlayerController> players = new List<PlayerController>();
        private GameObject[] enemies;
        private int currentWaveIdx = 1;
        private GameObject currentEnemyWave = null;
        private bool winOrLose = false;

        private void Awake()
        {
            Model.DmgCalculator.Load();
            Model.ComboSkills.Load();
        }

        //private void Start()
        //{
        //    GameObject[] playerGameObjects;
        //    playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        //    foreach (var playerGameObject in playerGameObjects)
        //    {
        //        players.Add(playerGameObject.GetComponent<PlayerController>());
        //    }

        //    newEnemyWave(1);
        //}

        //private void Update()
        //{
        //    if (winOrLose)
        //        return;
        //    if (CheckPlayersLose())
        //    {
        //        StartCoroutine(GameOver());
        //    }
        //    else if (CheckPlayersWin())
        //    {
        //        StartCoroutine(Win());
        //    }
        //    else if (currentEnemyWave != null)
        //    {
        //        if (EnemiesAllKilled())
        //            newEnemyWave(currentWaveIdx + 1);
        //    }
        //}

        private bool EnemiesAllKilled()
        {
            Debug.Log("enemy num = " + enemies.Length);
            foreach(var enemy in enemies) {
                if (enemy != null)
                {
                    Debug.Log("!= null");
                    return false;
                }
            }
            Debug.Log("All Killed:)");
            return true;
        }

        private bool CheckPlayersLose()
        {
            foreach (var player in players)
            {
                if (player.IsDead())
                    return true;
            }
            return false;
        }

        private bool CheckPlayersWin()
        {
            if (currentEnemyWave == null && !CheckPlayersLose())
                return true;
            return false;
        }

        private void newEnemyWave(int waveIdx)
        {
            string enemyWaveName = string.Format("EnemyWave{0}", waveIdx);
            if (currentEnemyWave != null)
            {
                currentEnemyWave.SetActive(false);
            }
            GameObject nextEnemyWave;
            Transform nextEnemyWaveTrans = GameObject.Find("EnemyWaves").transform.Find(enemyWaveName);
            if (nextEnemyWaveTrans != null)
            {
                nextEnemyWave = nextEnemyWaveTrans.gameObject;
                nextEnemyWave.SetActive(true);
                currentWaveIdx = waveIdx;
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                currentEnemyWave = nextEnemyWave;
            }
            else
            {
                currentEnemyWave = null;
            }
        }

        private IEnumerator GameOver()
        {
            Debug.Log("GameOver :(((");
            winOrLose = true;
            players[0].Lose();
            players[1].Lose();

            // Animation?
            // wait for 10 seconds
            yield return new WaitForSeconds(10);
            // Reload this Scene
            SceneManager.LoadScene("Level_" + System.Convert.ToString(GameStatus.level));
        }
        private IEnumerator Win()
        {
            Debug.Log("Win :)))");
            winOrLose = true;
            players[0].Win();
            players[1].Win();

            // Animation?
            // wait for 10 seconds
            yield return new WaitForSeconds(10);
            GameStatus.level += 1;
            // Load Next Scene
            SceneManager.LoadScene("Level_" + System.Convert.ToString(GameStatus.level));
        }
    }
}
