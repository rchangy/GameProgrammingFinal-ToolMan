using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;

namespace ToolMan.Combat
{
    public class PlayerCombat : CombatUnit
    {
        [SerializeField]
        private PlayerController thisPlayer;
        [SerializeField]
        private PlayerController anotherPlayer;
        [SerializeField]
        private PlayerCombat anotherPlayerCombat;
        [SerializeField]
        private ComboSkillSet comboSkillSet;

        private Resource _energy;
        public int Energy
        {
            get => _energy.Value;
        }

        protected override void Start()
        {
            base.Start();
            _energy = stats.GetResourceByName("Energy");
            if (_energy == null) Debug.Log(gameObject.name + " has no energy resource.");
        }

        public PlayerController getThisPlayer()
        {
            return thisPlayer;
        }

        public PlayerCombat getAnotherPlayerCombat()
        {
            return anotherPlayerCombat;
        }

        public override int TakeDamage(int rawDmg, CombatUnit damager)
        {
            int dmg = base.TakeDamage(rawDmg, damager);
            // check strength
            if (dmg > Str)
            {
                thisPlayer.grabPoint.Release();
                if (thisPlayer.isTool) thisPlayer.ToolableManTransform();
            }
            return dmg;
        }

        public void ComboSkillAttack()
        {
            if (!AttackEnabled) return;
            if (!_hasSkillToUse) return;
            if (Attacking) return;
            ComboSkill checkedComboSkill;
            if ((checkedComboSkill = comboSkillSet.GetComboSkill(thisPlayer, anotherPlayer, this)) != null)
            {
                _vulnerable.Disable();
                _energy.ChangeValueBy(-checkedComboSkill.Cost);
                skillPerforming = StartCoroutine(PerformComboSkill(checkedComboSkill));
            }
        }
        private IEnumerator PerformComboSkill(ComboSkill skill)
        {
            yield return StartCoroutine(skill.Attack(Anim, TargetLayers, this));
            yield return new WaitForSeconds(skill.attackInterval / Aspd);
            skillPerforming = null;
            _vulnerable.RemoveDisability();
        }
    }
}