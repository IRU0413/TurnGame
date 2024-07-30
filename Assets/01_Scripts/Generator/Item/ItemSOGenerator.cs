using Scripts.Enums;
using Scripts.Pattern;
using Scripts.SO;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scripts.Generator
{
    public class ItemSOGenerator : MonoBehaviour
    {
        private const string _dataPath = "CSV/Item/Gear/Gear_CSV_Data"; // 데이터 경로
        private const string _savePath = "Assets/Resources/SO/Item"; // 저장 경로

        [SerializeField] private bool _isReflectionDebug = false;
        [SerializeField] private int _dataVersion; // 데이터 버전
        [SerializeField] private ItemType _saveItemType; // 아이템 타입

        private void DebugInValuesInfo(Type type)
        {
            string resultStr = $"타입 이름: {type}\n\n";
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
                resultStr += $"field : {field.Name}\n";
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
                resultStr += $"property : {property.Name}\n";
            MethodInfo[] methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                resultStr += $"method : {method.Name}\n";
                var mPars = method.GetParameters();
                foreach (ParameterInfo param in mPars)
                {
                    resultStr += $">[{param.ParameterType}]: {param.Name},\n";
                }
            }
            Debug.Log(resultStr);
        }
        public void CreateAndSetSO<T>(string filePath, Dictionary<string, object> data) where T : ScriptableObject
        {
            if (_isReflectionDebug)
            {
                DebugInValuesInfo(typeof(T));
            }

            T so = Resources.Load<T>(filePath);
            if (so == null)
                so = ScriptableObject.CreateInstance<T>();

            Type soType = so.GetType();
            var soFields = soType.GetFields();

            if (soFields.Length == 0) return;

            foreach (var field in soFields)
            {
                Type fType = field.FieldType;
                if (fType.IsEnum)
                {
                    if (!data.ContainsKey(field.Name))
                        continue;

                    if (Enum.TryParse(fType, data[field.Name].ToString(), out var enumValue))
                        field.SetValue(so, enumValue);
                    continue;
                }
                else if (fType.IsGenericType && fType.GetGenericTypeDefinition() == typeof(SerializableDictionary<,>))
                {
                    var genericArguments = fType.GetGenericArguments();

                    var keyType = genericArguments[0];
                    var valueType = genericArguments[1];

                    var serDictionary = Activator.CreateInstance(typeof(SerializableDictionary<,>).MakeGenericType(keyType, valueType));
                    var addFunc = serDictionary.GetType().GetMethod("Add");
                    var onBeforeSerializeFunc = serDictionary.GetType().GetMethod("OnBeforeSerialize");

                    if (keyType.IsEnum)
                    {
                        var keyLis = Enum.GetValues(keyType);
                        var valueFields = valueType.GetFields();

                        if (valueFields.Length != 0)
                        {
                            foreach (var key in keyLis)
                            {
                                var valueGroup = Activator.CreateInstance(valueType);
                                bool valueChanged = false;

                                foreach (var valueF in valueFields)
                                {
                                    string dataKeyName = $"{key}_{valueF.Name}";
                                    if (!data.ContainsKey(dataKeyName))
                                        continue;

                                    var currentValue = valueF.GetValue(valueGroup);
                                    var newValue = Convert.ChangeType(data[dataKeyName], valueF.FieldType);

                                    if (!Equals(currentValue, newValue))
                                    {
                                        valueF.SetValue(valueGroup, newValue);
                                        valueChanged = true;
                                    }
                                }

                                if (valueChanged)
                                {
                                    addFunc.Invoke(serDictionary, new object[] { key, valueGroup });
                                }
                            }
                        }
                    }
                    onBeforeSerializeFunc.Invoke(serDictionary, null);
                    field.SetValue(so, serDictionary);
                }
                else
                {
                    if (!data.ContainsKey(field.Name))
                        continue;

                    field.SetValue(so, data[field.Name]);
                }
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.CreateAsset(so, $"{filePath}.asset");
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public void Create()
        {
            string resultDataPath = $"{_dataPath}_{_dataVersion}"; // 최종 데이터 경로
            string resultSavePath = $"{_savePath}/{_saveItemType}"; // 아이템 경로
            string saveFileName = $"{_saveItemType}_Item"; // 저장 파일 이름

            var datas = CSVReader.Read(resultDataPath); // 데이터 추출

            foreach (var data in datas)
            {
                data.Add(typeof(ItemType).Name, _saveItemType);
                CreateAndSetSO<GearItemSO>($"{resultSavePath}/{saveFileName}_{data["Id"]}", data);
            }
        }
    }
}
