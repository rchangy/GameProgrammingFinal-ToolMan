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

        public HitFeel HitFeel
        {
            get => hitFeel;
        }

        private PlayerSkillSet _availablePlayerSkillSet;

        protected override void Awake()
        {
            base.Awake();
            _playerController = gameObject.GetComponent<PlayerController>();
        }


        protected override void Start()
        {

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
            hitFeel.MakeCamShake(Mathf.Min(3, (float)dmg/(Str + 0.1f)));
            return dmg;
        }

        protected override void Interrupted(CombatUnit damager)
        {
            base.Interrupted(damager);
            ThisPlayerController.Hurt();
            ThisPlayerController.Release();
            if (ThisPlayerController.inToolState()) ThisPlayerController.ToolableManTransform();
        }

        public void AddHp(int hpAdd)
        {
            _hp.ChangeValueBy(hpAdd);
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
            PlayerSkill skill = _availablePlayerSkillSet.GetPlayerSkillbyName(currentUsingSkillName);

            if (skill.UsingHitFeel)
            {
                hitFeel.MakeTimeStop();
                hitFeel.MakeCamShake(skill.HitFeelMul);
                TeamMateCombat.hitFeel.MakeCamShake(skill.HitFeelMul);
            }
            if (ThisPlayerController.IsGrabbed())
            {
                Debug.Log(name + " hit " + target.name);
                target.TakeDamage(Atk, TeamMateCombat);
                StartCoroutine(currentUsingSkill.Hit(this, target));
            }
            else
            {
                base.Hit(target);
            }
        }
        protected override void Die()
        {
            ThisPlayerController.Die();
        }

    }
}