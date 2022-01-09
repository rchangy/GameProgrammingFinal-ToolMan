using UnityEngine;
using System.Collections;
using System;


namespace ToolMan.Combat.Equip
{
    public class Plug : CombatUnit
    {
        [SerializeField] private Flood _flood;
        private bool _pushed;
        private bool _setting;

        [SerializeField] private float MaxHeight;
        [SerializeField] private float MinHeight;

        [SerializeField] private float speed;
        private Vector3 moveVec;

        [SerializeField] private Collider _triggerCollider;

        override protected void Start()
        {
            moveVec = new Vector3(0, speed, 0);
            _triggerCollider.enabled = false;
        }
        override protected void Update()
        {
            if (_setting)
            {
                if (transform.position.y < MaxHeight)
                {
                    transform.position += moveVec * Time.deltaTime;
                    if(transform.position.y >= MaxHeight)
                    {
                        _setting = false;
                        _triggerCollider.enabled = true;
                        if (transform.position.y > MaxHeight)
                        {
                            transform.position = new Vector3(transform.position.x, MaxHeight, transform.position.z);
                        }
                    }
                }
            }
            if (_pushed)
            {
                if(transform.position.y > MinHeight)
                {
                    transform.position -= moveVec * Time.deltaTime;
                }
                if(transform.position.y <= MinHeight)
                {
                    _flood.StopFlooding();
                    _pushed = false;
                }
            }
        }

        protected override void FixedUpdate() { }

        public void SetFlood(Flood flood)
        {
            _flood = flood;
        }

        public void StartFlooding()
        {
            Debug.Log("plug start");
            _setting = true;
        }

        public void StopFlooding()
        {
            _setting = false;
            _pushed = true;
            _triggerCollider.enabled = false;
        }

        public override bool Attack()
        {
            Debug.Log("no attck action for plug");
            return false;
        }

        public override int TakeDamage(float baseDmg, float pow, CombatUnit damager)
        {

            if (damager.gameObject.CompareTag("Player"))
            {
                _pushed = true;
            }
            return 0;
        }


        protected override void Die() { }
        protected override void Interrupted() { }
    }
}

