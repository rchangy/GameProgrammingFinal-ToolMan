using UnityEngine;
using System.Collections.Generic;

namespace ToolMan.Combat.Skills
{
    [CreateAssetMenu(menuName = "ToolMan/Skill/ComboSkillSet")]
    public class ComboSkillSet : ScriptableObject
    {
        [SerializeField]
        private List<ComboSkill> _skills;

        private Dictionary<string, Dictionary<string, ComboSkill>> _comboSkills = new Dictionary<string, Dictionary<string, ComboSkill>>();


        public ComboSkill GetComboSkill(PlayerController tool, PlayerController man, PlayerCombat toolCombat)
        {
            Debug.Log(_comboSkills.Count);

            string preTool = tool.getTool().getName();
            string postTool = man.getTool().getName();
            ComboSkill usingSkill = null;
            if (_comboSkills.ContainsKey(preTool))
            {

                if (_comboSkills[preTool].ContainsKey(postTool))
                {
                    ComboSkill uncheckedSkill = _comboSkills[preTool][postTool];
                    if (uncheckedSkill.CheckEnergyCost(toolCombat))
                    {
                        usingSkill = uncheckedSkill;
                    }
                    else
                    {
                        Debug.Log("energy not enough for combo skill");
                    } 
                }
                else Debug.Log("cannot find tool " + postTool);
            }
            else Debug.Log("cannot find tool " + preTool);


            return usingSkill;
        }


        public void Load()
        {
            Debug.Log("loading");
            if (_skills.Count == 0) return;
            Debug.Log("loading combo skills");
            foreach (ComboSkill s in _skills)
            {
                if (!_comboSkills.ContainsKey(s.getPreTool()))
                {
                    _comboSkills[s.getPreTool()] = new Dictionary<string, ComboSkill>();
                }
                if(!_comboSkills[s.getPreTool()].ContainsKey(s.getPostTool()))
                    _comboSkills[s.getPreTool()].Add(s.getPostTool(), s);
            }
        }

    }
}