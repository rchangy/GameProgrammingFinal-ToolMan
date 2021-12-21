using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private string _currentScene;
    [SerializeField] private List<Objective> _objectives;

    void Start()
    {
        _currentScene = SceneManager.GetActiveScene().name;

        switch (_currentScene)
        {
            case "Level1":
                StartCoroutine(Level1());
                break;
            default:
                break;
        }

    }

    private IEnumerator Level1()
    {
        foreach(Objective obj in _objectives)
        {
            obj.StartObjective();
            while (!obj.isCompleted())
            {
                yield return null;
            }
            Debug.Log("level1 complete");
        }
        // next scene
    }

    
}
