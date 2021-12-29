using UnityEngine;
using System.Collections;

namespace ToolMan.Combat
{
    public class TurtleSkillCombat : SkillCombat
    {
        // skills
        //[SerializeField]
        //private LayerMask _targetLayers;

        [SerializeField] private EnemyTurtle _turtle;
        //private List<ScriptableBuff> _buffs;
        //private float _buffRange;

        protected override void Awake()
        {
            base.Awake();
            //_buffs = _turtle._buffs;
            //_buffRange = _turtle._buffRange;
        }

        public override bool Attack()
        {

            if (!AttackEnabled) return false;
            if (!_hasSkillToUse) return false;
            if (Attacking) return false;
            if(_skillCd[currentUsingSkillName] <= 0)
            {
                _skillCoroutine = StartCoroutine(PerformSkill());
                return true;
            }
            return false;

        }

        private IEnumerator PerformSkill()
        {
            _hitRefractoryPeriod = currentUsingSkill.RefractoryPeriod;
            yield return StartCoroutine(currentUsingSkill.Attack(this, _collisionEnable));
            SkillFinish();
        }

        private void InterruptAttack()
        {
            Debug.Log(name + " attack interrupted, coroutine stopped");
            StopCoroutine(_skillCoroutine);
            SkillFinish();
        }

        private void SkillFinish()
        {
            _collisionEnable.Value = false;
            _refractoryPeriod.Clear();
            _skillCd[currentUsingSkill.getName()] = currentUsingSkill.Cd;
            _skillCoroutine = null;
        }

        //protected virtual void OnTriggerStay(Collider other)
        //{
        //    if (!LayerMaskUtil.IsInLayerMask(TargetLayers, other.gameObject.layer)) return;
        //    if (!CollisionEnable) return;
        //    CombatUnit target = other.gameObject.GetComponent<CombatUnit>();
        //    if (target == null) return;
        //    if (_refractoryPeriod.ContainsKey(target)) return;
        //    _refractoryPeriod.Add(target, _hitRefractoryPeriod);
        //    Hit(target);
        //}

        protected override void Hit(CombatUnit target)
        {
            Debug.Log(name + " adds buff to " + target.name);
            StartCoroutine(currentUsingSkill.Hit(this, target));
            //StartCoroutine(currentUsingSkill.Hit(this, target));
        }

    }
}