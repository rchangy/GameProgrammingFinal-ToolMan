using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;

namespace ToolMan.Combat.Equip
{
    public class Sardine : MonoBehaviour
    {
        private Transform _target;
        private GameObject _whale;
        private SkillCombat _whaleCombat;

        private bool _returning = false;

        [SerializeField] private Skill _skill;

        [SerializeField]
        private float _speed;


        [SerializeField]
        private GameObject _explosionPrefab;
        [SerializeField] private float _explosionRange;

        [SerializeField] private float DieAfterSeconds = 5f;

        [SerializeField] private float _collisionTime;
        private bool _startCollision;

        private PlayerCombat _collidingPlayer;

        protected void Start()
        {
            // find target
            GameObject[] players =  GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0)
            {
                _target = findClosestPlayer(players);
                //Debug.Log("sardine target: " + _target.name);
            }
            if(_target == null)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!_startCollision || _returning) return;
            if (_collisionTime > 0) _collisionTime -= Time.deltaTime;
            else
            {
                Die();
            }
        }

        private void FixedUpdate()
        {
            MoveTowardTarget();
            DieAfterSeconds -= Time.deltaTime;
            if (DieAfterSeconds <= 0) Die();
        }

        public void SetWhale(CombatUnit whaleCombat)
        {
            if (typeof(SkillCombat).IsInstanceOfType(whaleCombat))
            {
                _whaleCombat = (SkillCombat)whaleCombat;
            }
            _whale = whaleCombat.gameObject;
            //_whaleCombat = whaleCombat;
        }

        private void MoveTowardTarget()
        {
            if (_returning)
            {
                transform.Rotate(0, 0, Time.deltaTime * 800);
            }
            else
            {
                transform.LookAt(_target);
            }
            Vector3 colliderPos = _target.GetComponent<Collider>().bounds.center;
            //transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
            transform.position = Vector3.MoveTowards(transform.position, colliderPos, Time.deltaTime * _speed);

        }

        private Transform findClosestPlayer(GameObject[] players)
        {
            if(players.Length == 1) { return players[0].transform; }
            float minDist = float.MaxValue;
            float dist;
            Transform ret = null;
            foreach(GameObject p in players){
                dist = Vector3.Distance(p.transform.position, transform.position);
                if(dist < minDist)
                {
                    minDist = dist;
                    ret = p.transform;
                }
            }
            return ret;
        }

        private void Die()
        {
            // Explosion effect
            Effect effect = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Effect>();
            effect.PlayEffect();
            Explode();

            Destroy(gameObject);
        }

        private void Explode()
        {
            Collider[] hitTargets = Physics.OverlapSphere(transform.position, _explosionRange, _whaleCombat.TargetLayers);
            foreach (Collider target in hitTargets)
            {
                CombatUnit targetCombat = target.GetComponent<CombatUnit>();
                if (targetCombat != null)
                {
                    targetCombat.TakeDamage(_whaleCombat.Atk * _skill.Multiplier, _whaleCombat.Pow * _skill.PowMuliplier, _whaleCombat);
                }
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (_whale == null) return;
            if (other.gameObject == _whale)
            {
                if (_returning)
                {
                    // hit whale
                    _whale.GetComponent<EnemyWhale>().TakeSardine();
                    Die();
                }
                return;
            }
            if (!other.gameObject.CompareTag("Player"))
            {
                Debug.Log("sardine hits " + other.name);
                Die();
                return;
            }

            //Debug.Log(other.name);
            if (_returning) return;
            _startCollision = true;
            if(_collidingPlayer == null || _collidingPlayer.gameObject != other.gameObject)
                _collidingPlayer = other.gameObject.GetComponent<PlayerCombat>();
            if (_collidingPlayer != null)
            {
                if (!_collidingPlayer.Vulnerable)
                {
                    _target = _whale.transform;
                    _returning = true;
                    DieAfterSeconds = 100;
                }
            }
        }
    }
}
