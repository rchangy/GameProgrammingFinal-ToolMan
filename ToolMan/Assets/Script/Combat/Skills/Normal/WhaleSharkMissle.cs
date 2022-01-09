using UnityEngine;
using System.Collections;
using ToolMan.Util;
using ToolMan.Combat.Equip;

namespace ToolMan.Combat.Skills.Normal
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/Enemy/WhaleSharkMissle")]
    public class WhaleSharkMissle : Skill
    {
        private EnemyWhale _whale;
        public GameObject SharkPrefab;

        public int MaxWave;

        private int sharkPosIdx = 0;

        public float TimeToNextSharkWave;

        public float InstantiateInterval = 0.2f;

        public override IEnumerator Attack(SkillCombat combat, BoolWrapper collisionEnable)
        {
            // instantiate shark
            if(_whale == null)
            {
                _whale = combat.gameObject.GetComponent<EnemyWhale>();
                if (_whale == null) yield break;
            }
            yield return new WaitForSeconds(1f);
            for(int wave = 0; wave < MaxWave; wave++)
            {
                int sharkNum = _whale.GetCurrentSharkWave();
                sharkPosIdx = Random.Range(0, _whale.SharkPos.Length);
                for(int i = 0; i < sharkNum; i++)
                {
                    var sharkObj = GameObject.Instantiate(SharkPrefab, _whale.SharkPos[sharkPosIdx].position, Quaternion.identity);
                    Shark shark = sharkObj.GetComponent<Shark>();
                    if (shark != null) shark.SetWhale(combat);
                    sharkPosIdx++;
                    if (sharkPosIdx == _whale.SharkPos.Length) sharkPosIdx = 0;
                    yield return new WaitForSeconds(InstantiateInterval);
                }
                yield return new WaitForSeconds(TimeToNextSharkWave);
            }

        }
    }
}
