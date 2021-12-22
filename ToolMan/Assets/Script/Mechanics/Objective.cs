using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour
{
    public int Order;

    protected GameManager _manager;

    protected bool _startup = false;

    public bool Startup
    {
        get => _startup;
    }

    private void Awake()
    {
        _manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    protected void Start()
    {
        Init();
        _startup = true;
    }
    protected abstract void Init();
    public abstract void StartObjective();
    public abstract bool isCompleted();
}
