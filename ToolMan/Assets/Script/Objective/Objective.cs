using UnityEngine;
using System.Collections;

public abstract class Objective : MonoBehaviour
{
    public AudioClip Bgm;
    public bool noBgm;
    public bool LoopBgm = true;
    public float Order;

    protected GameManager _manager;

    protected bool _startup = false;

    protected PlayerController _p1;
    protected PlayerController _p2;

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

    public void SetPlayers(PlayerController p1, PlayerController p2) { _p1 = p1; _p2 = p2; }

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
