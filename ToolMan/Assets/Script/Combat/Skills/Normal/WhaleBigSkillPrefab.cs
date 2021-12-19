using UnityEngine;
using System.Collections;
using System.Linq;


namespace ToolMan.Combat.Skills.Normal
{
    public class WhaleBigSkillPrefab : SkillCombat
    {
        protected override void Awake()
        {
            base.Awake();
            _hitRefractoryPeriod = 0.5f;
        }
        public void _Play(EnemyWhale whale, float _warningLastingTime, float attackDelay, float _lastingTime) {
            StartCoroutine(Play(whale, _warningLastingTime, attackDelay, _lastingTime));
        }

        private IEnumerator Play(EnemyWhale whale, float _warningLastingTime, float attackDelay, float _lastingTime) {
            //GetComponent<SphereCollider>().enabled = false;
            _collisionEnable.Value = false;
            //Warning
            Effect warningEffect = whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillWarning");
            warningEffect.PlayEffect();
            // play warning sound

            yield return new WaitForSeconds(_warningLastingTime);

            // anim
            yield return new WaitForSeconds(attackDelay);
            whale.GetAnimator().SetTrigger("Attack");

            // effect
            Effect bigSkillEffect = whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillEffect");
            bigSkillEffect.PlayEffect();

            //GetComponent<SphereCollider>().enabled = true;
            _collisionEnable.Value = true;
            yield return new WaitForSeconds(_lastingTime);
            bigSkillEffect.StopEffect();
            Destroy(gameObject);
        }

        protected override void OnTriggerStay(Collider other)
        {
            //Debug.Log("trigger other = " + other.name);
            base.OnTriggerStay(other);
        }

        protected override void Update()
        {
            // during attack
            lock (_refractoryPeriod)
            {
                if (_refractoryPeriod.Count > 0)
                {
                    foreach (CombatUnit c in _refractoryPeriod.Keys.ToList())
                    {
                        _refractoryPeriod[c] -= Time.deltaTime;
                        if (_refractoryPeriod[c] <= 0)
                        {
                            _refractoryPeriod.Remove(c);
                        }
                    }
                }
            }
        }
        //[SerializeField]
        //private float _lastingTime;
        //[SerializeField]
        //private float _warningLastingTime;


        //public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        //{
        //    EnemyWhale whale = combat.gameObject.GetComponent<EnemyWhale>();

        //    // Warning
        //    Effect warningEffect = whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillWarning");
        //    warningEffect.PlayEffect();
        //    // play warning sound

        //    yield return new WaitForSeconds(_warningLastingTime);

        //    // anim
        //    yield return new WaitForSeconds(attackDelay);
        //    whale.GetAnimator().SetTrigger("Attack");

        //    collisionEnable.Value = true;
        //    // effect
        //    Effect bigSkillEffect =  whale.effectController.effectList.Find(e => e.name == "WhaleBigSkillEffect");
        //    bigSkillEffect.PlayEffect();
        //    yield return new WaitForSeconds(_lastingTime);
        //    bigSkillEffect.StopEffect();

        //    collisionEnable.Value = false;
        //}
    }
}
