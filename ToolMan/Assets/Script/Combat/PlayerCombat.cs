using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;
using System.Collections.Generic;
using ToolMan.Util;

namespace ToolMan.Combat
{
    public class PlayerCombat : SkillCombat
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

        private ComboSkillSet comboSkillSet;

        private Dictionary<string, Resource> _skillEnergy = new Dictionary<string, Resource>();

        private Resource _currentEnergy;
        public int Energy
        {
            get => _currentEnergy.Value;
        }


        [SerializeField]
        private HitFeel hitFeel;

        private PlayerSkillSet _availablePlayerSkillSet;


        protected override void Start()
        {
            _playerController = gameObject.GetComponent<PlayerController>();

            if (typeof(PlayerSkillSet).IsInstanceOfType(availableSkillSet))
            {
                _availablePlayerSkillSet = (PlayerSkillSet)availableSkillSet;
                _availablePlayerSkillSet.SetSkillPlayer(TeamMateCombat, this);
            }
            base.Start();

            comboSkillSet = manager.Model.ComboSkills;
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            int dmg = base.TakeDamage(baseDmg, damager);
            // check strength
            if (dmg > Str)
            {
                //hitFeel.MakeHitFeel();
                ThisPlayerController.Hurt();
                ThisPlayerController.Release();
                if (ThisPlayerController.inToolState()) ThisPlayerController.ToolableManTransform();
            }
            return dmg;
        }

        public int GetEnergyOfTool(string name)
        {
            if (_skillEnergy.ContainsKey(name))
            {
                return _skillEnergy[name].Value;
            }
            return -1;
        }

        public override bool SetCurrentUsingSkill(string skillName)
        {
            bool isSkillSet = base.SetCurrentUsingSkill(skillName);
            if (isSkillSet && skillName != null)
            {
                if (!_skillEnergy.ContainsKey(skillName))
                {
                    if(_stats == null)
                    {
                        Debug.Log("null stat");
                    }
                    Resource _newEnergy = _stats.AddResource(new Resource(skillName + "Energy", 100, 0));
                    _skillEnergy.Add(skillName, _newEnergy);
                }
                _currentEnergy = _skillEnergy[skillName];
            }
            return isSkillSet;
        }

        public bool ComboSkillAvailable()
        {
            return comboSkillSet.GetComboSkill(ThisPlayerController, TeamMateCombat.ThisPlayerController, this) != null;
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
                _currentEnergy.ChangeValueBy(-checkedComboSkill.Cost);
                _skillCoroutine = StartCoroutine(PerformComboSkill(checkedComboSkill));
            }
        }

        

        private IEnumerator PerformComboSkill(ComboSkill skill)
        {
            _vulnerable.Disable();
            yield return StartCoroutine(skill.Attack(this, _collisionEnable));
            TeamMateCombat.ThisPlayerController.setChangeable(true);
            TeamMateCombat.ThisPlayerController.ToolManChange();
            _vulnerable.RemoveDisability();
            _skillCoroutine = null;
        }

        protected override void Hit(CombatUnit target)
        {
            base.Hit(target);
            //hitFeel.MakeHitFeel();
        }
        protected override void Die()
        {
            ThisPlayerController.Die();
        }
    }
}