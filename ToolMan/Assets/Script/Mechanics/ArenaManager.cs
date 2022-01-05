using UnityEngine;
using System.Collections;

public class ArenaManager : MonoBehaviour
{
    // Use this for initialization
    void Awake()
    {
        CheckpointManager.UpdateCheckpoint(5);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
