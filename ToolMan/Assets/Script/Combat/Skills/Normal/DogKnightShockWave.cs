using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/DogKnightShockWave")]
    public class DogKnightShockWave : Skill
    {
        [SerializeField] private GameObject _shockWavePrefab;
        private EnemyDogKnight _dogKnight;


        [SerializeField] private Vector3 _instantiateOffset;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            if(_dogKnight == null)
            {
                _dogKnight = combat.gameObject.GetComponent<EnemyDogKnight>();
                if (_dogKnight == null)
                    yield break;
            }
            if (_shockWavePrefab == null)
                yield break;

            _dogKnight.Anim.SetTrigger("Attack1");
            yield return new WaitForSeconds(attackDelay);
            GameObject shockWaveObj = Instantiate(_shockWavePrefab, combat.transform.position + _instantiateOffset, Quaternion.identity);
            ShockWave shockWave = shockWaveObj.GetComponent<ShockWave>();
            shockWave.SetCombat(combat);
            Vector3 target = _dogKnight.closestPlayer;
            Vector3 targetDir = Vector3.Normalize(target - combat.transform.position);

            shockWave.SetDir(targetDir);
            
        }
    }
}
