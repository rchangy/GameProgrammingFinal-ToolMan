using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;

namespace ToolMan.Combat
{
    public class PlayerCombat : CombatUnit
    {
        private PlayerController _playerController;
        [SerializeField]
        private PlayerCombat _teamMate;

        public PlayerController ThisPlayerController
        {
            get => _playerController;
        }
        public PlayerCombat TeamMateCombat
        {
            get => _teamMate;
        }

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

            _playerController = gameObject.GetComponent<PlayerController>();

            comboSkillSet.checkLoad();
        }



        public override int TakeDamage(int rawDmg, CombatUnit damager)
        {
            int dmg = base.TakeDamage(rawDmg, damager);
            // check strength
            if (dmg > Str)
            {
                ThisPlayerController.grabPoint.Release();
                if (ThisPlayerController.isTool) ThisPlayerController.ToolableManTransform();
            }
            return dmg;
        }

        public void ComboSkillAttack()
        {
            Debug.Log("try use combo skill");
            if (!AttackEnabled) return;
            if (!_hasSkillToUse) return;
            if (Attacking) return;
            ComboSkill checkedComboSkill;
            if ((checkedComboSkill = comboSkillSet.GetComboSkill(ThisPlayerController, TeamMateCombat.ThisPlayerController, this)) != null)
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
            TeamMateCombat.ThisPlayerController.setChangeable(true);
            TeamMateCombat.ThisPlayerController.ToolManChange();
            skillPerforming = null;
            _vulnerable.RemoveDisability();
        }
    }
}