using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolMan.Combat.Equip;
using ToolMan.Combat;

public class RetrieveMemory : Objective
{
    GameObject crystal;
    Crystal memoryCrystal;
    bool crystalBroken;
    bool complete = false;
    List<PlayerController> players = new List<PlayerController>();
    protected override void Init()
    {
        crystal = gameObject.transform.Find("MemoryCrystal").gameObject;
        if (crystal != null)
        {
            memoryCrystal = crystal.GetComponent<Crystal>();
            memoryCrystal.gameObject.SetActive(true);
            memoryCrystal.setObjective(this);
            crystalBroken = false;
        }
        else
        {
            Debug.Log("There is no MemoryCrystal.");
            crystalBroken = true;
        }

        
        GameObject[] playerGameObjects;
        playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach (var playerGameObject in playerGameObjects)
        {
            players.Add(playerGameObject.GetComponent<PlayerController>());
        }

    }
    public override bool isCompleted()
    {
        return complete;
    }

    public override void StartObjective()
    {
        StartCoroutine(CheckCrystal());
    }
    private IEnumerator CheckCrystal()
    {
        while (!crystalBroken)
            yield return null;

        // Release & toMan
        players[0].ResetToIdle();
        players[1].ResetToIdle();

        // set camera
        CameraManager[] cams;
        cams = GameObject.FindObjectsOfType<CameraManager>();
        foreach (var cam in cams)
        {
            cam.LookAtFace();
        }
        yield return new WaitForSeconds(2f);
        // unlock
        CheckpointManager.NextLevel();
        players[0].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player1ToolNum);
        players[1].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player2ToolNum);

        // wait for seconds
        yield return new WaitForSeconds(5f);
        complete = true;
    }
    public void CrystalDie()
    {
        crystalBroken = true;
    }
}
