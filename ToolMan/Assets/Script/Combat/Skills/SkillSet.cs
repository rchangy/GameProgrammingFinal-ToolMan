using UnityEngine;
using System.Collections.Generic;
namespace ToolMan.Combat.Skills
{
    // save a set of skills
    [CreateAssetMenu(menuName = "ToolMan/Skill/SkillSet")]
    public class SkillSet : ScriptableObject
    {
        // contains every skill that a combat unit can use 
        private Dictionary<string, Skill> skillSet = new Dictionary<string, Skill>();

        // for serialize
        [SerializeField]
        private List<Skill> skills;

        private List<Skill> cannotUseSkills = new List<Skill>();

        public bool HasSkill(string name)
        {
            return skillSet.ContainsKey(name);
        }

        public Skill GetSkillbyName(string name)
        {
            return skillSet[name];
        }

        public void CheckStatsExsistence(CombatUnit combat)
        {
            foreach (Skill skill in skills)
            {
                if (!skill.CheckStatsExsistence(combat))
                {
                    cannotUseSkills.Add(skill);
                }
            }
            if (cannotUseSkills.Count > 0)
            {
                foreach (Skill skill in cannotUseSkills)
                {
                    skills.Remove(skill);
                }
            }
            cannotUseSkills.Clear();
            if (skills.Count == 0) return;
            foreach (Skill skill in skills)
            {
                var skillName = skill.getName();
                if(!skillSet.ContainsKey(skillName))
                    skillSet.Add(skillName, skill);
            }
        }
    }
}