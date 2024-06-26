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
            Debug.Log("장비 아이템 생성");

            foreach (var data in _datas)
            {
                // 아이디
                int id = ExtraFunction.GetValueToKeyEnum<int>(data, GearAbilityDataProperty.Id);
                // 아이템 이름
                string itemName = ExtraFunction.GetValueToKeyEnum<string>(data, GearAbilityDataProperty.ItemName);
                // 아이템 등급 타입
                ItemGradeType itemGradeType = ExtraFunction.GetCorrectionValueToEnumType<ItemGradeType>(data, GearAbilityDataProperty.ItemGradeType);
                // 장비 타입
                GearType gearType = ExtraFunction.GetCorrectionValueToEnumType<GearType>(data, GearAbilityDataProperty.GearType);

                // so 저장 경로
                string soSavePath = GetPath(id);
                // so 로드
                GearItemSO so = AssetDatabase.LoadAssetAtPath<GearItemSO>(soSavePath);
                // 데이터에 있는 아이템 능력치들 파싱
                SerializableDictionary<GearAbilityType, GearAbility> abilities = GetAbility(data);

                // 파싱한 능력치가 하나도 없다면
                if (abilities.Count <= 0)
                {
                    Debug.Log($"Gear Item is not Ability > [Not Ability GearItem ID]:{id}");
                    continue;
                }

                // 기존 데이터 유무
                if (so != null)
                {
                    // 유
                    so.SetData(id, itemName, itemGradeType, gearType, abilities);
                    EditorUtility.SetDirty(so); // 변경 사항 등록
                    AssetDatabase.SaveAssets(); // 저장
                }
                else
                {
                    // 무
                    so = ScriptableObject.CreateInstance<GearItemSO>(); // 걍 new 한 느낌임.
                    so.SetData(id, itemName, itemGradeType, gearType, abilities);
                    AssetDatabase.CreateAsset(so, soSavePath); // 실질적을 생성하고 저장 위치에 생성
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

                // 둘중에 하나라도 포함되지 않았다면.
                if (!(data.ContainsKey(pointKey) || data.ContainsKey(percentKey)))
                {
                    Debug.LogWarning($"해당 KeyType({keyType})에 해당하는 데이터가 없습니다.");
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
