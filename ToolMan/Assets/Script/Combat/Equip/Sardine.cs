using UnityEngine;
using System.Collections;
using ToolMan.Combat.Skills;

namespace ToolMan.Combat.Equip
{
    public class Sardine : MonoBehaviour
    {
        private Transform _target;
        private GameObject _whale;
        private CombatUnit _whaleCombat;

        private bool _returning = false;

        [SerializeField] private Skill _skill;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private EffectController effectController;

        [SerializeField]
        private GameObject _explosionPrefab;

        private bool explodeOnDeath;

        [SerializeField] private float DieAfterSeconds = 5f;

        protected void Start()
        {
            // find target
            GameObject[] players =  GameObject.FindGameObjectsWithTag("Player");

            if (players.Length > 0)
            {
                _target = findClosestPlayer(players);
            }
            if(_target == null)
            {
                Destroy(gameObject);
            }
            explodeOnDeath = true;
        }

        private void FixedUpdate()
        {
            MoveTowardTarget();
            DieAfterSeconds -= Time.deltaTime;
            if (DieAfterSeconds <= 0) Die();
        }

        public void SetWhale(CombatUnit whaleCombat)
        {
            _whale = whaleCombat.gameObject;
            _whaleCombat = whaleCombat;
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
            transform.position = Vector3.MoveTowards(transform.position, _target.position, Time.deltaTime * _speed);
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
            if (explodeOnDeath)
            {
                Effect effect = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Effect>();
                effect.PlayEffect();
            }

            Destroy(gameObject);
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    if(collision.gameObject == _whale)
        //    {
        //        if (_returning)
        //        {
        //            // hit whale
        //            _whale.GetComponent<EnemyWhale>().TakeSardine();
        //            explodeOnDeath = true;
        //            Die();
        //        }
        //        return;
        //    }
        //    PlayerCombat playerCombat = collision.gameObject.GetComponent<PlayerCombat>();
        //    if(playerCombat != null)
        //    {
        //        if (_returning) return;
        //        if (!playerCombat.Vulnerable)
        //        {
        //            _target = _whale;
        //            _returning = true;
        //        }
        //        else
        //        {
        //            playerCombat.TakeDamage(_whaleCombat.Atk * _atkMultiplier, _whaleCombat);
        //            // maybe add buff?
        //            Die();
        //        }
        //    }
        //    else
        //    {
        //        Die();
        //    }
        //}

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _whale)
            {
                if (_returning)
                {
                    // hit whale
                    _whale.GetComponent<EnemyWhale>().TakeSardine();
                    explodeOnDeath = true;
                    Die();
                }
                return;
            }
            if (!other.gameObject.CompareTag("Player")) return;

            Debug.Log(other.name);
            PlayerCombat playerCombat = other.gameObject.GetComponent<PlayerCombat>();
            if (playerCombat != null)
            {
                if (_returning) return;
                if (!playerCombat.Vulnerable)
                {
                    _target = _whale.transform;
                    _returning = true;
                    DieAfterSeconds = 100;
                }
                else
                {
                    playerCombat.TakeDamage(_whaleCombat.Atk * _skill.Multiplier, _whaleCombat.Pow * _skill.PowMuliplier, _whaleCombat);
                    // maybe add buff?
                    Debug.Log(other.name);
                    Die();
                }
            }
            else
            {
                Debug.Log(other.name);
                Die();
            }
        }
    }
}
