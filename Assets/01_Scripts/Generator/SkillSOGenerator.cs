using Scripts.Enums;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Generator
{
    public class SkillSOGenerator : MonoBehaviour
    {
        private const string SKILL_SO_DATA_PATH = "CSV/Skill/Skill";
        private const string SKILL_ICON_PATH = "Image/Skill_Icon";
        private const string SKILL_GAMEOBJECT_PATH = "Prefab/Skill";

        private const string SKILL_SAVE_PATH = "Assets/Resources/SO/Skill";

        public string _skillCsvVersion;

        public void CreateSOAsset()
        {
            string path = $"{SKILL_SO_DATA_PATH}_{_skillCsvVersion}";
            List<Dictionary<string, object>> skills = CSVReader.Read(path);
            Sprite[] icons = Resources.LoadAll<Sprite>($"{SKILL_ICON_PATH}/spritesheet");
            GameObject loadGo = Resources.Load<GameObject>($"{SKILL_GAMEOBJECT_PATH}/Test");

            foreach (var data in skills)
            {
                SkillSO asset = ScriptableObject.CreateInstance<SkillSO>();

                int id = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.ID);

                AbilityType abilityType = ExtraFunction.GetCorrectionValueToEnumType<AbilityType>(data, SkillProperty.AbilityType);

                string name = ExtraFunction.GetValueToKeyEnum<string>(data, SkillProperty.Name);

                int mana = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.Mana);
                int coolTime = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.CoolTime);

                SearchFormatType searchFormatType = ExtraFunction.GetCorrectionValueToEnumType<SearchFormatType>(data, SkillProperty.SearchFormat);
                ActionPointCorrection drawPoint = ExtraFunction.GetCorrectionValueToEnumType<ActionPointCorrection>(data, SkillProperty.DrawPoint);
                int searchDistance = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.SearchDistance);

                int min = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.MinEffectApplyUnitCount);
                int max = ExtraFunction.GetValueToKeyEnum<int>(data, SkillProperty.MaxEffectApplyUnitCount);

                int iconSpriteIndex = id;
                Sprite icon = null;
                if (iconSpriteIndex < icons.Length)
                    icon = icons[iconSpriteIndex];

                GameObject go = loadGo;
                AudioClip activeSound = null;

                asset.SaveData(id, abilityType, name, mana, coolTime, searchFormatType, drawPoint, searchDistance, icon, activeSound);
                // asset.SaveData(id, name, mana, coolTime, searchFormatType, searchDistance, min, max, icon, activeSound);
                AssetDatabase.CreateAsset(asset, $"{SKILL_SAVE_PATH}/Skill_{id}.asset");
                AssetDatabase.SaveAssets();
            }
        }
    }
}
