using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private string _currentScene;
    [SerializeField] private GameObject Objectives;
    [SerializeField] private int objectivesDepth = -1;
    private List<Objective> _objectives;

    [SerializeField]
    private UIController _uIController;
    [SerializeField]
    private PlayerController _p1;
    [SerializeField]
    private PlayerController _p2;
    [SerializeField]
    private GameObject PostFX_Dead;

    public bool reset = true;
    private bool levelComplete = false;
    private void Awake()
    {
        if (reset)
        {
            CheckpointManager.resetLevel();
        }
    }

    void Start()
    {
        //_currentScene = SceneManager.GetActiveScene().name;
        if (Objectives != null)
        {
            _objectives = Objectives.GetComponentsInChildren<Objective>().ToList();
        }
        List<Objective> tmpObjectives = new List<Objective>(_objectives);
        foreach (Objective o in _objectives) {
            if (objectivesDepth != -1)
            {
                if (FindDepth(Objectives.transform, o.transform) > objectivesDepth)
                {
                    tmpObjectives.Remove(o);
                }
            }
            o.uIController = _uIController;
            o.SetPlayers(_p1, _p2);
        }
        _objectives = tmpObjectives;
        _objectives.Sort((x, y) => x.Order.CompareTo(y.Order));

        StartCoroutine(CompleteSceneObjectives());

    }

    void Update()
    {
        if (_p1.IsDead() || _p2.IsDead())
        {
            StartCoroutine(PlayerLose());
        }
        else if (levelComplete)
        {
            // next level
            SceneManager.LoadScene("Level" + CheckpointManager.GetCheckpointInfo().level);
        }
    }

    private IEnumerator CompleteSceneObjectives()
    {
        foreach(Objective obj in _objectives)
        {
            while (!obj.Startup) yield return null;
            Debug.Log("obj start " + obj.gameObject.name + " , Order: " + obj.Order);
            obj.StartObjective();
            while (!obj.isCompleted())
            {
                yield return null;
            }
        }
        levelComplete = true;
    }
    private int FindDepth(Transform root, Transform objective)
    {
        if (objective.parent == root)
            return 1;
        else
            return 1 + FindDepth(root, objective.parent);
    }

    private IEnumerator PlayerLose()
    {
        _p1.controlEnable = false;
        _p2.controlEnable = false;
        PostFX_Dead.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}
