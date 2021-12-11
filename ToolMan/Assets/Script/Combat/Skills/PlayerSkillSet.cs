using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/PlayerSkillSet")]
    public class PlayerSkillSet : SkillSet
    {
        private List<Skill> _removingSkills = new List<Skill>();
        public void SetSkillPlayer(PlayerCombat manCombat, PlayerCombat toolCombat)
        {
            foreach(Skill s in skills)
            {
                if (typeof(PlayerSkill).IsInstanceOfType(s))
                {
                    PlayerSkill p = (PlayerSkill)s;
                    p.SetPlayers(manCombat, toolCombat);
                }
                else
                {
                    _removingSkills.Add(s);
                }
            }
            foreach(Skill s in _removingSkills)
            {
                skills.Remove(s);
            }
            skillSet.Clear();
            Load();
        }

        public PlayerSkill GetPlayerSkillbyName(string name)
        {
            return (PlayerSkill) GetSkillbyName(name);
        }
    }
}
