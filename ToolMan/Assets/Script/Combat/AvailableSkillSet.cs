using UnityEngine;
using System.Collections.Generic;

// save a set of skills

public class AvailableSkillSet : MonoBehaviour
{
    // contains every skill that a combat unit can use 
    private Dictionary<string, Skill> skillSet;

    // for serialize
    public Skill[] skills;

    private void Awake()
    {
        skillSet = new Dictionary<string, Skill>();
        foreach(Skill skill in skills)
        {
            var skillName = skill.getName();
            skillSet.Add(skillName, skill);
        }
    }

    public bool HasSkill(string name)
    {
        return skillSet.ContainsKey(name);
    }

    public Skill GetSkillbyName(string name)
    {
        return skillSet[name];
    }
}
