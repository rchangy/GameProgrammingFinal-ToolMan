using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour
{
    public int Order;

    protected GameManager _manager;

    protected bool _startup = false;

    protected bool _inputEnable; // player control & UI

    protected UIController _uIController;

    public bool Startup
    {
        get => _startup;
    }

    public UIController uIController
    {
        get => _uIController;
        set { _uIController = value; }
    }

    private void Awake()
    {
        _manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _inputEnable = false;
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
