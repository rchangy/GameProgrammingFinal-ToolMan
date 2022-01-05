using UnityEngine;
using System.Collections;
using ToolMan.Combat;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] private CombatManager _combatManager;
    // Use this for initialization
    void Start()
    {
        CheckpointManager.UpdateCheckpoint(5);
        _combatManager.LoadTool();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
