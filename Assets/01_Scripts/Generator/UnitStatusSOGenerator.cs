using Scripts.Enums;
using Scripts.SO;
using Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Generator
{
    public class UnitStatusSOGenerator : MonoBehaviour
    {
        private const string UNIT_STATUS_CSV_FILE_PATH = "CSV/Unit/Status/UnitStatus";
        private const string UNIT_SOUND_DATA_PATH = "Sound/Unit/UnitSound";

        private const string UNIT_DATA_SO_SAVE_PATH = "Assets/Resources/SO/Unit/AllData/UnitData";
        private const string UNIT_STATUS_SO_SAVE_PATH = "Assets/Resources/SO/Unit/Status/UnitStatus";
        private const string UNIT_SOUND_SO_SAVE_PATH = "Assets/Resources/SO/Unit/Sound/UnitSound";

        public int _unitlCsvVersion;

        public void CreateSOAsset()
        {
            CreateUnitStatusSOAsset(_unitlCsvVersion);
        }

        public void CreateUnitStatusSOAsset(int verstion)
        {
            string UnitCSVPath = $"{UNIT_STATUS_CSV_FILE_PATH}_{verstion}";
            List<Dictionary<string, object>> unitStatusDatas = CSVReader.Read(UnitCSVPath);

            foreach (var data in unitStatusDatas)
            {
                int id = ExtraFunction.GetValueToKeyEnum<int>(data, ActionUnitProperty.ID);
                string unitName = ExtraFunction.GetValueToKeyEnum<string>(data, ActionUnitProperty.Name);
                JobType job = ExtraFunction.GetCorrectionValueToEnumType<JobType>(data, ActionUnitProperty.JobType);

                var status = SaveStatusSO(id, data);
                var sound = SaveSoundSO(id);

                string soPath = $"{UNIT_DATA_SO_SAVE_PATH}_{id}.asset";
                UnitDataSO so = AssetDatabase.LoadAssetAtPath<UnitDataSO>(soPath);
                if (so != null)
                {
                    // so.SaveData(id, unitName, job, status, sound);
                    EditorUtility.SetDirty(so);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    so = ScriptableObject.CreateInstance<UnitDataSO>();
                    // so.SaveData(id, unitName, job, status, sound);
                    AssetDatabase.CreateAsset(so, soPath);
                }
            }
        }

        private UnitAbilitySO SaveStatusSO(int id, Dictionary<string, object> data)
        {
            string fileName = $"{UNIT_STATUS_SO_SAVE_PATH}_{id}.asset";
            UnitAbilitySO so = AssetDatabase.LoadAssetAtPath<UnitAbilitySO>(fileName);

            float health = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.Health);
            float mana = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.Mana);

            float attackPower = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.AttackPower);
            float magicPower = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.MagicPower);

            float armor = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.Armor);
            float magicResistance = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.MagicResistacne);

            float criticalStrikeChance = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.CriticalStrikeChance);
            float criticalStrikeDamage = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.CriticalStrikeDamage);

            float armorPenetration = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.ArmorPenetration);
            float magicPenetration = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.MagicPenetration);

            float activeSpeed = ExtraFunction.GetValueToKeyEnum<float>(data, ActionUnitProperty.ActionSpeed);
            int movePoint = ExtraFunction.GetValueToKeyEnum<int>(data, ActionUnitProperty.MovePoint);

            if (so != null)
            {
                // so.SaveData(id, health, mana, attackPower, magicPower, armor, magicResistance, criticalStrikeChance, criticalStrikeDamage, armorPenetration, magicPenetration, activeSpeed, movePoint);
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
            else
            {
                so = ScriptableObject.CreateInstance<UnitAbilitySO>();
                // so.SaveData(id, health, mana, attackPower, magicPower, armor, magicResistance, criticalStrikeChance, criticalStrikeDamage, armorPenetration, magicPenetration, activeSpeed, movePoint);
                AssetDatabase.CreateAsset(so, fileName);
            }

            return so;
        }

        private UnitSoundSO SaveSoundSO(int id)
        {
            string fileName = $"{UNIT_SOUND_SO_SAVE_PATH}_{id}.asset";
            UnitSoundSO so = AssetDatabase.LoadAssetAtPath<UnitSoundSO>(fileName);

            string soundPath = $"{UNIT_SOUND_DATA_PATH}_{id}";
            AudioClip[] sounds = Resources.LoadAll<AudioClip>(soundPath);

            AudioClip idle = null;
            AudioClip hit = null;
            AudioClip cc = null;
            AudioClip die = null;

            for (int i = 0; i < sounds.Length; i++)
            {
                string soundFileName = sounds[i].name;
                string[] s = soundFileName.Split("_");
                string needName = s[s.Length - 1];

                UnitStateType condition = UnitStateType.None;
                Enum.TryParse(needName, out condition);
                switch (condition)
                {
                    case UnitStateType.Idle:
                        idle = sounds[i];
                        break;
                    case UnitStateType.Hit:
                        hit = sounds[i];

                        break;
                    /* case UnitStateType.CC:
                         cc = sounds[i];

                         break;*/
                    case UnitStateType.Die:
                        die = sounds[i];
                        break;
                    default:
                        Debug.LogError("While creating \"Unit Sound SO\", we encountered problems loading and connecting the required sounds.");
                        break;
                }
            }

            if (so != null)
            {
                so.SaveData(id, idle, hit, cc, die);
                EditorUtility.SetDirty(so);
                AssetDatabase.SaveAssets();
            }
            else
            {
                so = ScriptableObject.CreateInstance<UnitSoundSO>();
                so.SaveData(id, idle, hit, cc, die);
                AssetDatabase.CreateAsset(so, fileName);
            }

            return so;
        }
    }
}
