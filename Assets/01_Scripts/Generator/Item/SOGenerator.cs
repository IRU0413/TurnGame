using Scripts.Pattern;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Scripts.Generator
{
    public class SOGenerator : MonoBehaviour
    {
        protected const string SO_SAVE_PATH = "Assets/Resources/SO/";
        protected const string CSV_SAVE_PATH = "CSV/";

        [SerializeField] private bool _isReflectionDebug = false;

        private void DebugInValuesInfo(Type type)
        {
            if (!_isReflectionDebug) return;

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
        protected virtual void CreateAndSetSO<T>(string filePath, Dictionary<string, object> data) where T : ScriptableObject
        {
            DebugInValuesInfo(typeof(T));

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
        public virtual void CreateEditBtn()
        {
            Debug.Log("생성기 실행");
        }

        // 해당 타입으로 경로 설정을 할때, 타입을 문자로 만들고 대문자로 별로 나누어주기위해 만듬
        protected List<string> GetUpperSlideStringArray(Type value, bool isUpperlink = false)
        {
            string valueName = value.Name;
            int soTypeNameLenght =  valueName.Length;

            int cutStart = 0;
            List<string> cutNames = new();

            for (int i = 1; i < soTypeNameLenght; i++)
            {
                if (char.IsUpper(valueName[i]))
                {
                    if (isUpperlink && char.IsLower(valueName[i - 1]))
                    {
                        cutNames.Add(valueName.Substring(cutStart, i - cutStart));
                        cutStart = i;
                    }
                }
            }
            cutNames.Add(valueName.Substring(cutStart, soTypeNameLenght - cutStart));
            return cutNames;
        }

        protected string GetLoadPahtString(string path)
        {
            string[] resultStr = path.Split("Assets/Resources/");
            if (resultStr.Length > 1)
                return resultStr[resultStr.Length - 1];
            return string.Empty;
        }
    }
}