using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ToolMan.Util;
using System;

/*
 * defines an attack
*/
namespace ToolMan.Combat.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public float attackDelay;
        public float RefractoryPeriod;
        public float Cd;

        public abstract IEnumerator Attack(PlayerController player, LayerMask targetLayer, CombatUnit combat, BoolWrapper collisionEnable);

        public string getName()
        {
            return name;
        }
    }
}