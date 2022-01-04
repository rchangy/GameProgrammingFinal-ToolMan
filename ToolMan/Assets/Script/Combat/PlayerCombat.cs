using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;
using ToolMan.Combat.Stats;
using System.Collections.Generic;
using ToolMan.Util;
using ToolMan.Gameplay;
using static ToolMan.Core.Simulation;

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

        [SerializeField] private float _hitFeelCd;
        private float _hitFeelCoolingTime;
        private bool _hitFeelCooling
        {
            get => _hitFeelCoolingTime > 0;
        }


        private PlayerSkillSet _availablePlayerSkillSet;

        [SerializeField]
        private bool playingImpactEffect;
        [SerializeField]
        private float impactEffectCd;
        private float impactEffectRemain;

        protected override void Awake()
        {
            base.Awake();
            _playerController = gameObject.GetComponent<PlayerController>();
            playingImpactEffect = false;
            impactEffectRemain = impactEffectCd;
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

        

        public override int TakeDamage(float baseDmg, float pow, CombatUnit damager)
        {
            int dmg = base.TakeDamage(baseDmg, pow, damager);
            hitFeel.MakeCamShake(Mathf.Min(3, (float)dmg/(Str + 0.1f)), _playerController);
            if (dmg > 0)
                Schedule<PlayerHurt>().player = _playerController;
            return dmg;
        }

        protected override void Interrupted()
        {
            if (!_isInterrupted) return;
            base.Interrupted();
            ThisPlayerController.Hurt();
            if(ThisPlayerController.IsGrabbing())
                ThisPlayerController.Release();
            var dir = transform.position - _lastDamager.transform.position;
            dir.y = 0f;
            dir = Vector3.Normalize(dir);
            dir.y = 2;
            _rb.drag = 4;
            _rb.AddForce(dir * 300 * _rb.mass);
            _timeToStopRb = _stopVelTime;
            if (Movable)
                _movable.Disable();
        }

        public void AddHP(int hpAdd)
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
                if (!_hitFeelCooling)
                {
                    //hitFeel.MakeTimeStop();
                    _hitFeelCoolingTime = _hitFeelCd;
                }
                hitFeel.MakeCamShake(skill.HitFeelMul, _playerController);
                TeamMateCombat.hitFeel.MakeCamShake(skill.HitFeelMul, _teamMate._playerController);
            }
            if (ThisPlayerController.IsGrabbed())
            {
                Schedule<PlayerHit>().player = ThisPlayerController;
                Debug.Log(name + " hit " + target.name);
                target.TakeDamage(Atk * currentUsingSkill.Multiplier, Pow * currentUsingSkill.PowMuliplier,TeamMateCombat);
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

        protected override void OnTriggerStay(Collider other)
        {
            if (!LayerMaskUtil.IsInLayerMask(TargetLayers, other.gameObject.layer)) return;
            if (!CollisionEnable) return;
            CombatUnit target = other.gameObject.GetComponent<CombatUnit>();
            if (target == null) return;
            if (_refractoryPeriod.ContainsKey(target)) return;
            _refractoryPeriod.Add(target, _hitRefractoryPeriod);
            //_hitDir = transform.position - target.transform.position;
            Hit(target);

            if (!playingImpactEffect) {
                Impact effect = _playerController.effectController.effectList.Find(e => e.name == "ImpactEffect").GetComponent<Impact>();
                //effect.contactPoint = transform.position;
                string toolName = _playerController.getTool().getName();
                effect.setToolName(toolName);
                effect.PlayEffect();
                playingImpactEffect = true;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (playingImpactEffect)
                impactEffectRemain -= Time.deltaTime;
            if (impactEffectRemain <= 0){
                impactEffectRemain = impactEffectCd;
                playingImpactEffect = false;
            }
            if (_hitFeelCooling)
            {
                _hitFeelCoolingTime -= Time.deltaTime;
            }
        }
    }
}