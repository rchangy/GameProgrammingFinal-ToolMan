using UnityEngine;
using System.Collections;
using System;

namespace ToolMan.Combat.Equip
{
    public class Plug : MonoBehaviour
    {
        [SerializeField] private Flood _flood;
        private bool _pushed;
        private bool _setting;

        [SerializeField] private float MaxHeight;
        [SerializeField] private float MinHeight;

        [SerializeField] private float speed;
        private Vector3 moveVec;

        [SerializeField] private Collider _triggerCollider;

        private void Start()
        {
            moveVec = new Vector3(0, speed, 0);
            _triggerCollider.enabled = false;
        }
        private void Update()
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
                }
            }
        }

        public void SetFlood(Flood flood)
        {
            _flood = flood;
        }

        public void StartFlooding()
        {
            _setting = true;
        }

        public void StopFlooding()
        {
            _setting = false;
            _pushed = true;
            _triggerCollider.enabled = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_setting) return;
            if (!other.CompareTag("Player")) return;
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.inToolState()) return;
            _pushed = true;
            _triggerCollider.enabled = false;
        }

    }
}

