using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private string _currentScene;
    [SerializeField] private GameObject Objectives;
    private List<Objective> _objectives;

    [SerializeField]
    private UIController _uIController;
    [SerializeField]
    private PlayerController _p1;
    [SerializeField]
    private PlayerController _p2;

    public bool reset = true;
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
        foreach (Objective o in _objectives) {
            o.uIController = _uIController;
            o.SetPlayers(_p1, _p2);
        }
        _objectives.Sort((x, y) => x.Order.CompareTo(y.Order));

        StartCoroutine(CompleteSceneObjectives());

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
        Debug.Log("level1 complete");
        // next scene
    }

    
}
