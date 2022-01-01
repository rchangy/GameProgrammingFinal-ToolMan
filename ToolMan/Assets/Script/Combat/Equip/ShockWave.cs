using UnityEngine;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class ShockWave : CombatUnit
    {
        private CombatUnit _combat;

        private Vector3 _dir;

        private float _duration = 0.5f;

        [SerializeField] private Explosion _vfx;

        [SerializeField] private float _effectPlayingTime;
        [SerializeField] private float _effectStoppingTime;

        protected override void Start()
        {
            base.Start();
            _vfx.PlayEffect();
            //StartCoroutine(ManageVfx());
        }


        public override bool Attack()
        {
            Debug.Log("shock wave cannot attack");
            return false;
        }

        protected override void Update()
        {
            transform.position += _dir * Spd * Time.deltaTime;
        }

        private IEnumerator ManageVfx()
        {
            if (_vfx == null) yield break;
            
            while (true)
            {
                _vfx.PlayEffect();
                yield return new WaitForSeconds(_effectPlayingTime);
                _vfx.StopEffect();
                yield return new WaitForSeconds(_effectStoppingTime);
            }
        }

        public override int TakeDamage(float baseDmg, CombatUnit damager)
        {
            Debug.Log("shock wave cannot be damaged");
            return 0;
        }

        public void SetCombat(CombatUnit combat)
        {
            _combat = combat;
        }

        public void SetDir(Vector3 dir)
        {
            _dir = Vector3.Normalize(dir);
        }

        public void OnTriggerEnter(Collider other)
        {
            if ( LayerMask.NameToLayer("Ground") == other.gameObject.layer) return;
            if (LayerMask.NameToLayer("GrabbedPoint") == other.gameObject.layer) return;
            if (LayerMask.NameToLayer("Wall") == other.gameObject.layer)
                StartCoroutine(DieAfterSeconds());
            CombatUnit target = other.gameObject.GetComponent<CombatUnit>();
            if(target != null)
            {
                if (target == _combat) return;
                if (typeof(PlayerCombat).IsInstanceOfType(target))
                {
                    PlayerCombat targetPlayer = (PlayerCombat)target;
                    if(_combat != null)
                        targetPlayer.TakeDamage(_combat.Atk, this);
                }
            }
        }

        private IEnumerator DieAfterSeconds()
        {
            yield return new WaitForSeconds(_duration);
            Destroy(gameObject);
        }
    }
}
