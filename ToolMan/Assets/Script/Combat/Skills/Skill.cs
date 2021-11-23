using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * defines an attack
*/
namespace ToolMan.Combat.Skills
{
    public abstract class Skill : ScriptableObject
    {
        public float attackInterval;
        public float attackDelay;

        // requirements
        private List<string> _requiredStatsName = new List<string>();
        private List<string> _requiredResourcesName = new List<string>();
        private List<string> _requiredAbilitiesName = new List<string>();

        // shooting point or attack point
        protected Transform attackPoint;

        public void SetAttackPoint(Transform t)
        {
            attackPoint = t;
        }

        public string getName()
        {
            return name;
        }

        public abstract IEnumerator Attack(Animator anim, LayerMask targetLayer, CombatUnit combat);



        //public abstract bool GetRunTimeData(CombatUnit combat);
        //public abstract IEnumerator Action(Animator anim, LayerMask targetLayer, CombatUnit combat);


        public bool CheckStatsExsistence(CombatUnit combat)
        {

            foreach (string s in _requiredStatsName)
            {
                if (combat.GetStatBaseValue(s) == null)
                {
                    Debug.Log(combat.gameObject.name + " can't use skill " + name + " for not having stat " + s);
                    return false;
                }
            }
            foreach (string r in _requiredResourcesName)
            {
                if (combat.GetResourceMaxValue(r) == null)
                {
                    Debug.Log(combat.gameObject.name + " can't use skill " + name + " for not having resource " + r);
                    return false;
                }
            }
            foreach (string a in _requiredAbilitiesName)
            {
                if (combat.GetAbilityInitState(a) == null)
                {
                    Debug.Log(combat.gameObject.name + " can't use skill " + name + " for not having ability " + a);
                    return false;
                }
            }
            return true;
        }
        protected virtual List<string> GetRequiredStatsName()
        {
            return _requiredStatsName;
        }
        protected virtual List<string> GetRequiredResourcesName()
        {
            return _requiredResourcesName;
        }
        protected virtual List<string> GetRequiredAbilitiesName()
        {
            return _requiredAbilitiesName;
        }
    }
}