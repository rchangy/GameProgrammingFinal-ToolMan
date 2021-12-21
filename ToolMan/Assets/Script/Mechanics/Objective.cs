using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour
{
    protected GameManager _manager;


    private void Awake()
    {
        _manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    public abstract void StartObjective();
    public abstract bool isCompleted();
}
