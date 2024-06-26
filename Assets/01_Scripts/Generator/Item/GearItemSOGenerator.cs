using Scripts.Cores.Unit;
using Scripts.Enums;
using Scripts.Pattern;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Generator
{
    public class GearItemSOGenerator : ItemSOGenerator
    {
        int _gearAbtilityStartNum = (int)GearAbilityType.MaxHealth;
        int _gearAbtilityEndNum = (int)GearAbilityType.ActionSpeed;

        public override void CreateSOAssets()
        {
            base.CreateSOAssets();
            Debug.Log("��� ������ ����");

            foreach (var data in _datas)
            {
                // ���̵�
                int id = ExtraFunction.GetValueToKeyEnum<int>(data, GearAbilityDataProperty.Id);
                // ������ �̸�
                string itemName = ExtraFunction.GetValueToKeyEnum<string>(data, GearAbilityDataProperty.ItemName);
                // ������ ��� Ÿ��
                ItemGradeType itemGradeType = ExtraFunction.GetCorrectionValueToEnumType<ItemGradeType>(data, GearAbilityDataProperty.ItemGradeType);
                // ��� Ÿ��
                GearType gearType = ExtraFunction.GetCorrectionValueToEnumType<GearType>(data, GearAbilityDataProperty.GearType);

                // so ���� ���
                string soSavePath = GetPath(id);
                // so �ε�
                GearItemSO so = AssetDatabase.LoadAssetAtPath<GearItemSO>(soSavePath);
                // �����Ϳ� �ִ� ������ �ɷ�ġ�� �Ľ�
                SerializableDictionary<GearAbilityType, GearAbility> abilities = GetAbility(data);

                // �Ľ��� �ɷ�ġ�� �ϳ��� ���ٸ�
                if (abilities.Count <= 0)
                {
                    Debug.Log($"Gear Item is not Ability > [Not Ability GearItem ID]:{id}");
                    continue;
                }

                // ���� ������ ����
                if (so != null)
                {
                    // ��
                    so.SetData(id, itemName, itemGradeType, gearType, abilities);
                    EditorUtility.SetDirty(so); // ���� ���� ���
                    AssetDatabase.SaveAssets(); // ����
                }
                else
                {
                    // ��
                    so = ScriptableObject.CreateInstance<GearItemSO>(); // �� new �� ������.
                    so.SetData(id, itemName, itemGradeType, gearType, abilities);
                    AssetDatabase.CreateAsset(so, soSavePath); // �������� �����ϰ� ���� ��ġ�� ����
                }
            }
        }

        private string GetPath(int id)
        {
            return $"{GetSOFilePath(_itemType.ToString(), $"{ItemType.Gear}_Item_{id}")}.asset";
        }
        private SerializableDictionary<GearAbilityType, GearAbility> GetAbility(Dictionary<string, object> data)
        {
            SerializableDictionary<GearAbilityType, GearAbility> abilities = new();
            string strPoint = "_Point";
            string strPercent = "_Percent";

            for (int typeNum = _gearAbtilityStartNum; typeNum <= _gearAbtilityEndNum; typeNum++)
            {
                GearAbilityType keyType = (GearAbilityType)typeNum;
                string keyTypeStr = keyType.ToString();

                // str
                string pointKey = keyTypeStr + strPoint;
                string percentKey = keyTypeStr + strPercent;

                // value
                float pointValue = 0;
                float percentValue = 0;

                // ���߿� �ϳ��� ���Ե��� �ʾҴٸ�.
                if (!(data.ContainsKey(pointKey) || data.ContainsKey(percentKey)))
                {
                    Debug.LogWarning($"�ش� KeyType({keyType})�� �ش��ϴ� �����Ͱ� �����ϴ�.");
                    continue;
                }

                // Dictionary > Value.ToString()
                var pointValueStr = data[pointKey].ToString();
                var percentValueStr = data[percentKey].ToString();

                bool addPoint = false;
                bool addPercent = false;
                // Value Parse Float
                if (float.TryParse(pointValueStr, out pointValue))
                    addPoint = (pointValue > 0);

                if (float.TryParse(percentValueStr, out percentValue))
                    addPercent = (percentValue > 0);

                // Add
                if (addPoint || addPercent)
                {
                    var ability = new GearAbility();

                    ability.Point = pointValue;
                    ability.Percent = percentValue;

                    abilities.Add(keyType, ability);
                }
            }
            abilities.OnBeforeSerialize();
            return abilities;
        }
    }
}
