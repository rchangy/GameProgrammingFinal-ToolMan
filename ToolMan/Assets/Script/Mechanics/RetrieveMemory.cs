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
    private AudioSource _audioSource;

    public float waitAfterUnlock = 5f;
    protected override void Init()
    {
        if (_audioSource)
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _audioSource.clip = Bgm;
        }
        crystal = gameObject.transform.Find("MemoryCrystal").gameObject;
        if (crystal != null)
        {
            memoryCrystal = crystal.GetComponent<Crystal>();
            memoryCrystal.gameObject.SetActive(false);
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
        players.Sort((x, y) => { return x.playerNum.CompareTo(y.playerNum); });

    }
    public override bool isCompleted()
    {
        return complete;
    }

    public override void StartObjective()
    {
        players[0].controlEnable = true;
        players[1].controlEnable = true;
        uIController.SetBattleUI(true);
        memoryCrystal.gameObject.SetActive(true);
        StartCoroutine(CheckCrystal());
    }
    private IEnumerator CheckCrystal()
    {
        while (!crystalBroken)
            yield return null;

        // Release & toMan
        players[0].ResetToIdle();
        players[1].ResetToIdle();
        movePlayer1();

        // set camera
        CameraManager[] cams;
        cams = GameObject.FindObjectsOfType<CameraManager>();
        foreach (var cam in cams)
        {
            cam.LookAtFace();
        }
        yield return new WaitForSeconds(3f);
        // unlock
        CheckpointManager.NextLevel();
        players[0].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player1ToolNum);
        players[1].UnlockTool(CheckpointManager.GetCheckpointInfo().level, CheckpointManager.GetCheckpointInfo().player2ToolNum);

        // wait for seconds
        if (_audioSource)
            _audioSource.Play();
        yield return new WaitForSeconds(waitAfterUnlock);
        foreach (var cam in cams)
        {
            cam.ResetCam();
        }
        complete = true;
    }
    public void CrystalDie()
    {
        crystalBroken = true;
    }

    private void movePlayer1()
    {
        RaycastHit m_Hit;
        float m_MaxDistance;
        bool m_HitDetect;

        float moveDis = 5f;
        float accumulateRotates = 0f;
        bool moved = false;
        m_MaxDistance = moveDis;

        int count = 0;
        _p1.transform.forward = _p2.transform.forward;
        _p1.transform.position = _p2.transform.position;
        while (!moved)
        {
            accumulateRotates += 90f;
            count++;
            _p1.transform.eulerAngles += new Vector3(0f, 90f, 0f);
            m_HitDetect = Physics.BoxCast(_p1.GetCollider().bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, moveDis);
            if (!m_HitDetect || m_Hit.collider.gameObject.tag == "Player" || m_Hit.collider.isTrigger)
            {
                _p1.transform.position += moveDis * transform.forward;
                moved = true;
            }
            else
            {
                //Debug.Log("Hit : " + m_Hit.collider.name);
            }
            if (count >= 4)
                break;
        }
        if (moved)
            _p1.transform.eulerAngles -= new Vector3(0f, accumulateRotates, 0f);
    }
}
