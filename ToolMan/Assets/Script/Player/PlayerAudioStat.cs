using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioStat
{
    public float lastWalkTime;
    public float lastHurtTime;
    public float lastHitTime;
    public float lastAttackTime;
    public PlayerAudioStat()
    {
        lastWalkTime = float.MaxValue;
        lastHurtTime = float.MaxValue;
        lastHitTime = float.MaxValue;
        lastAttackTime = float.MaxValue;
    }
    public void update()
    {
        lastWalkTime += Time.deltaTime;
        lastHurtTime += Time.deltaTime;
        lastHitTime += Time.deltaTime;
        lastAttackTime += Time.deltaTime;
    }
}
