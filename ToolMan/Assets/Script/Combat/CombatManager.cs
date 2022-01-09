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
        [SerializeField] private Transform Player1Cam, Player2Cam;

        private List<PlayerController> players = new List<PlayerController>();
        //private GameObject[] enemies;
        //private int currentWaveIdx = 1;
        //private GameObject currentEnemyWave = null;
        //private bool winOrLose = false;

        private void Awake()
        {
            Model.DmgCalculator.Load();
            Model.ComboSkills.Load();
        }

        private void Start()
        {
            GameObject[] playerGameObjects;
            playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (var playerGameObject in playerGameObjects)
            {
                players.Add(playerGameObject.GetComponent<PlayerController>());
            }
            players.Sort((x, y) => { return x.playerNum.CompareTo(y.playerNum); });
            LoadTool();
        }

        public void LoadTool()
        {
            CheckpointManager.LoadCheckpoint();
            players[0].LoadTool(CheckpointManager.GetCheckpointInfo().player1ToolNum);
            players[1].LoadTool(CheckpointManager.GetCheckpointInfo().player2ToolNum);
        }

        public void UnLockTool()
        {
            // set camera to look at bearMan's face
            players[0].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player1ToolNum);
            players[1].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player2ToolNum);
        }

        //private void Update()
        //{
        //    if (!enableEnemyWaves)
        //        return;
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

        //private bool EnemiesAllKilled()
        //{
        //    Debug.Log("enemy num = " + enemies.Length);
        //    foreach(var enemy in enemies) {
        //        if (enemy != null)
        //        {
        //            Debug.Log("enemy num != null");
        //            return false;
        //        }
        //    }
        //    Debug.Log("All Killed:)");
        //    return true;
        //}

        //private bool CheckPlayersLose()
        //{
        //    foreach (var player in players)
        //    {
        //        if (player.IsDead())
        //            return true;
        //    }
        //    return false;
        //}

        //private bool CheckPlayersWin()
        //{
        //    if (currentEnemyWave == null && !CheckPlayersLose())
        //        return true;
        //    return false;
        //}

        public void SetHPCanvas(Enemy enemy)
        {
            List<Enemy> enemies = new List<Enemy>();
            enemies.Add(enemy);
            SetHPCanvas(enemies);
        }

        public void SetHPCanvas(List<Enemy> enemies)
        {
            foreach(var enemy in enemies)
            {
                Transform canvas1 = enemy.transform.Find("Canvas1");
                Transform canvas2 = enemy.transform.Find("Canvas2");
                EnemyHPCanvas EHPC1, EHPC2;
                if (canvas1 != null)
                {
                    EHPC1 = canvas1.GetComponent<EnemyHPCanvas>();
                    if (EHPC1 != null)
                    {
                        EHPC1.setCamera(Player1Cam);
                    }
                    else
                    {
                        Debug.Log("Set EnemyHPCanvas for Canvas1 to keep Canvas1 looking at player1's camera.");
                    }
                }
                else
                {
                    Debug.Log("Cannot find canvas 1");
                }

                if (canvas2 != null)
                {
                    EHPC2 = canvas2.GetComponent<EnemyHPCanvas>();
                    if (EHPC2 != null)
                    {
                        EHPC2.setCamera(Player2Cam);
                    }
                    else
                    {
                        Debug.Log("Set EnemyHPCanvas for Canvas2 to keep Canvas2 looking at player2's camera.");
                    }
                }
                else
                {
                    Debug.Log("Cannot find canvas 2");
                }
            }
        }

        private IEnumerator GameOver()
        {
            Debug.Log("GameOver :(((");
            //winOrLose = true;
            players[0].Lose();
            players[1].Lose();

            // Animation?
            // wait for 10 seconds
            yield return new WaitForSeconds(10);
            // Reload this Scene
            //SceneManager.LoadScene("Level_" + System.Convert.ToString(GameStatus.level));
        }
        private IEnumerator Win()
        {
            Debug.Log("Win :)))");
            //winOrLose = true;
            players[0].Win();
            players[1].Win();

            // Animation?
            // wait for 10 seconds
            yield return new WaitForSeconds(10);
            //GameStatus.level += 1;
            // Load Next Scene
            //SceneManager.LoadScene("Level_" + System.Convert.ToString(GameStatus.level));
        }
    }
}
