using UnityEngine;
using System.Collections.Generic;
using ToolMan.Player;
using ToolMan.Util;
using System.Collections;

namespace ToolMan.Combat.Equip
{
    public class Shark : MonoBehaviour
    {
        private bool _aiming = false;
        private bool _startUpdateLine = false;
        protected Transform[] Players;
        protected PlayerController[] playerControllers;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float speed;
        [SerializeField] private float settingPosSpeed;
        private bool _launched = false;

        [SerializeField] private float _aimingTime;
        [SerializeField] private float _waitingTime;
        [SerializeField] private float _timeToDie;

        public float YAxisOffset;
        public float ForwardOffset;

        [SerializeField] private float atk;

        private BoolWrapper collisionEnable = new BoolWrapper();

        private SkillCombat whaleCombat;

        private Vector3 _targetDir;

        [SerializeField] private LineRenderer _line;
        [SerializeField] private int _maxLineSegments;
        private int _lineSegmentCount = 0;
        private List<Vector3> _linePoints = new List<Vector3>();
        public float stepTime;

        private Collider _collider;

        private void Start()
        {
            GameObject[] PlayerGameObjects = GameObject.FindGameObjectsWithTag("Player");
            int playerNum = PlayerGameObjects.Length;

            Players = new Transform[playerNum];
            for (int i = 0; i < playerNum; i++)
            {
                Players[i] = PlayerGameObjects[i].transform;
            }
            playerControllers = new PlayerController[playerNum];
            for (int i = 0; i < playerNum; i++)
            {
                playerControllers[i] = PlayerGameObjects[i].GetComponent<PlayerController>();
            }
            Vector3 initLookDir = new Vector3(0, transform.position.y, 0);
            transform.LookAt(initLookDir);
            _linePoints.Clear();
            StartCoroutine(Attack());
            _collider = GetComponent<Collider>();
        }

        public void SetWhale(SkillCombat whale)
        {
            whaleCombat = whale;
        }

        public void UpdateTrajectory()
        {
            _linePoints.Clear();

            for (int i = 0; i < _lineSegmentCount; i++)
            {
                float stepTimePassed = stepTime * i;
                Vector3 moveVec = new Vector3
                (
                    0,
                    0,
                    stepTimePassed
                );

                _linePoints.Add(moveVec);
            }

            _line.positionCount = _linePoints.Count;
            _line.SetPositions(_linePoints.ToArray());
        }

        public void StopAiming()
        {
            _aiming = false;
            _collider.isTrigger = true;
        }

        public void StartAiming()
        {
            _aiming = true;
            _startUpdateLine = true;
        }

        private void Update()
        {
            if (_aiming)
            {
                LookAtClosestPlayer();
                if(_lineSegmentCount < _maxLineSegments)
                    _lineSegmentCount++;
            }
            else
            {
                if (_launched)
                    Rush();
            }
            if(_startUpdateLine)
                UpdateTrajectory();
        }

        public void Launch()
        {
            _launched = true;
        }

        private void Rush()
        {
            transform.position += _targetDir * speed * Time.deltaTime;
        }

        private void LookAtClosestPlayer()
        {
            Vector3 lookatDest = GetClosestplayer().position;
            lookatDest.y = transform.position.y;
            var targetDirection = lookatDest - transform.position;
            if (transform.position != lookatDest)
            {
                var newDir = Vector3.RotateTowards(transform.forward, targetDirection, Time.deltaTime * rotateSpeed, 0f);
                if (!Physics.Raycast(transform.position, newDir, LayerMask.NameToLayer("Environment")))
                {
                    transform.rotation = Quaternion.LookRotation(newDir);
                    _targetDir = newDir;
                }
            }
        }

        private Transform GetClosestplayer()
        {
            Transform target = null;
            float minDist = float.MaxValue;

            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == null) continue;
                if (playerControllers[i].IsGrabbed()) continue;
                float dist = Vector3.Distance(transform.position, Players[i].position);
                if (dist < minDist)
                {
                    target = Players[i];
                    minDist = dist;
                }
            }
            return target;
        }

        private IEnumerator Attack()
        {
            // move to destination
            Vector3 targetPosAfterYMove = new Vector3(transform.position.x, transform.position.y + YAxisOffset, transform.position.z);
            Vector3 targetPosAfterXMove = targetPosAfterYMove + transform.forward * ForwardOffset;
            while (Vector3.Distance(transform.position, targetPosAfterYMove) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosAfterYMove, settingPosSpeed * Time.deltaTime);
                yield return null;
            }
            while (Vector3.Distance(transform.position, targetPosAfterXMove) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosAfterXMove, settingPosSpeed * Time.deltaTime);
                yield return null;
            }
            Debug.Log("Start aiming");
            StartAiming();
            yield return new WaitForSeconds(_aimingTime);
            StopAiming();
            Debug.Log("Stop aiming");
            yield return new WaitForSeconds(_waitingTime);
            Launch();
            Debug.Log("Launch");
            collisionEnable.Value = true;
            yield return new WaitForSeconds(_timeToDie);
            Debug.Log("Destroy");
            GameObject.Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!collisionEnable.Value) return;
            PlayerCombat playerCombat;
            if ((playerCombat = other.gameObject.GetComponent<PlayerCombat>()) == null) return;
            playerCombat.TakeDamage(atk, whaleCombat);
        }
    }
}
