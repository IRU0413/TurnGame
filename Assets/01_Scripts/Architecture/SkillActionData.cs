
using Scripts.Cores.Skills.Skill;
using Scripts.Cores.Skills.SkillObject;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Architecture
{
    [Serializable]
    public struct SkillActionData
    {
        [SerializeField] private List<SkillObjectCore> _actionSkillObjectCore;
        public List<SkillObjectCore> ActionSkillList => _actionSkillObjectCore;

        public void Init(SkillCore core)
        {
            /*for (int i = 0; i < _actionSkillObjectCore.Count; i++)
            {
                SkillObjectCore objCore = _actionSkillObjectCore[i];
                objCore.Setting(core);
                if (!objCore.IsInitialized)
                {
                    _actionSkillObjectCore.Remove(objCore);
                    GameObject.Destroy(objCore.gameObject);
                    i--;
                }
            }*/
        }

        public void Draw()
        {
            foreach (var skillObj in _actionSkillObjectCore)
            {
                // skillObj.DrawRnage();
            }
        }
    }
}