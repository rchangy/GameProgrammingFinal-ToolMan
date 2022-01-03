using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/SharkRush")]
    public class SharkRush : Skill
    {
        private Shark _shark;
        [SerializeField] private float _aimingTime;
        [SerializeField] private float _waitingTime;
        [SerializeField] private float _timeToDie;

        public float YAxisOffset;
        public float XAxisOffset;
        public float Speed;


        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if (_shark == null)
            {
                _shark = combat.gameObject.GetComponent<Shark>();
                if (_shark == null) yield break;
            }

            // move to destination
            Vector3 targetPosAfterYMove = new Vector3(_shark.transform.position.x, _shark.transform.position.y + YAxisOffset, _shark.transform.position.z);
            Vector3 targetPosAfterXMove = new Vector3(_shark.transform.position.x + XAxisOffset, _shark.transform.position.y + YAxisOffset, _shark.transform.position.z);
            //while (Vector3.Distance(_shark.transform.position, targetPosAfterYMove) < 0.05f)
            //{
            //    _shark.transform.position = Vector3.MoveTowards(_shark.transform.position, targetPosAfterYMove, Speed * Time.deltaTime);
            //    yield return null;
            //}
            //while (Vector3.Distance(_shark.transform.position, targetPosAfterXMove) < 0.05f)
            //{
            //    _shark.transform.position = Vector3.MoveTowards(_shark.transform.position, targetPosAfterXMove, Speed * Time.deltaTime);
            //    yield return null;
            //}
            Debug.Log("Start aiming");
            _shark.StartAiming();
            yield return new WaitForSeconds(_aimingTime);
            _shark.StopAiming();
            Debug.Log("Stop aiming");
            yield return new WaitForSeconds(_waitingTime);
            _shark.Launch();
            Debug.Log("Launch");
            collisionEnable.Value = true;
            yield return new WaitForSeconds(_timeToDie);
            Debug.Log("Destroy");
            GameObject.Destroy(_shark.gameObject);
        }

    }
}
