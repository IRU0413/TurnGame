using Scripts.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

// 여기 클래스들은 모두 static 사용
// because, 어디서든 접근하여 사용하기 위함
// 공용성 있게 만들어야됨
namespace Scripts.Util
{
    public static class ExtraFunction
    {
        //public static GameObject CreateChildGameObject(this GameObject go)
        public static GameObject CreateChildGameObject(this Transform tr, string name)
        {
            GameObject result = null;
            int count = tr.childCount;

            // 리턴 분기점 포함
            for (int i = 0; i < count; i++)
            {
                if (tr.GetChild(i).name.Equals(name))
                {
                    result = tr.GetChild(i).gameObject;
                    break;
                }
            }

            if (result == null)
            {
                result = new GameObject(name);

                result.transform.parent = tr;
                result.transform.localScale = Vector2.one;
                result.transform.localPosition = Vector3.zero;
            }

            return result;
        }

        /// <summary>
        /// 필요한 컴포넌트를 지정 오브젝트에서 가져가는 함수
        /// (하지만, 없다면 강제로 넣어서 가져감)
        /// </summary>
        /// <typeparam name="T">컴포넌트 타입</typeparam>
        /// <param name="go">지정 오브젝트</param>
        /// <returns></returns>
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T evt = go.GetComponent<T>();
            if (evt == null)
                evt = go.AddComponent<T>();
            return evt;
        }

        /// <summary>
        /// 지정오브젝트의 자식 오브젝트중에서 지정된 이름의 오브젝트를 찾는 함수
        /// </summary>
        /// <param name="go">부모 오브젝트</param>
        /// <param name="name">찾을 자식 오브젝트의 이름</param>
        /// <param name="recursive">재귀적으로 찾을 것인가?</param>
        /// <returns>찾을 오브젝트 이름의 오브젝트 또는 Null</returns>
        public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
        {
            Transform transform = FindChild<Transform>(go, name, true);

            if (transform == null)
                return null;

            return transform.gameObject;
        }

        /// <summary>
        /// 지정오브젝트의 자식 오브젝트중에서 지정된 이름의 오브젝트를 찾는 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="name"></param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
        {
            // 지정 오브젝트 존재 여부
            if (go == null)
                return null;

            // 재귀 여부 판단
            if (recursive == false)
            {
                // 자식 오브젝트 수 만큼 반복
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    // 지정 오브젝트의 자식들 중, 번호해 해당하는 오브젝트 Tr 가져오기
                    Transform tr = go.transform.GetChild(i);

                    // 찾으려는 이름 없거나 비어있는 경우나 지정 오브젝트의 이름 이라면
                    if (string.IsNullOrEmpty(name) || go.transform.name == name)
                    {
                        T component = tr.GetComponent<T>();
                        if (component != null)
                        {
                            return component;
                        }
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    // 찾으려는 이름 없거나 비어있는 경우나 찾으려는 이름과 같은 이름의 자식 오브젝트가 있는 경우
                    if (string.IsNullOrEmpty(name) || component.name == name)
                    {
                        // 해당 타입 
                        return component;
                    }
                }
            }

            // 찾으려고 했던 이름이 존재하지 않는다면 Null
            return null;
        }

        public static string GetEnumString<T>(T e)
        {
            return Enum.GetName(typeof(T), e);
        }

        public static int GetRandomIdxByPercentArray(int[] pctArray)
        {
            UnityEngine.Random.InitState(DateTime.Now.Second);

            // 룸 타입 확률 데이터 가져오기
            int[] pctArr = new int[pctArray.Length];


            // 룸 확률 총 값
            int percentTotal = 0;

            for (int i = 0; i < pctArray.Length; i++)
            {
                percentTotal += pctArray[i];
                pctArr[i] = percentTotal;
            }

            int randPct = UnityEngine.Random.Range(0, percentTotal);
            int minPct = 0;
            int maxPct = 0;
            int chooseNum = -1;
            for (int i = 0; i < pctArr.Length; i++)
            {
                if (pctArr[i] <= 0)
                    continue;

                maxPct = pctArr[i];
                if (minPct <= randPct && randPct < maxPct)
                {
                    chooseNum = i;
                    break;
                }
                minPct = maxPct;
            }

            return chooseNum;
        }

        public static T GetValueToKeyEnum<T>(Dictionary<string, object> data, Enum keyEnum)
        {
            string keyName = keyEnum.ToString();
            T value = default;
            if (!data.ContainsKey(keyName))
            {
                Debug.LogError($"[{keyName}] <= 해당 키는 존재 하지 않는 키입니다.");
                return value;
            }
            value = (T)data[keyName];
            return value;
        }

        public static T GetCorrectionValueToEnumType<T>(Dictionary<string, object> data, Enum keyEnum) where T : struct
        {
            string keyName = keyEnum.ToString();

            T result = default;
            string value = data.ContainsKey(keyName) ? data[keyName].ToString() : "";
            if (!Enum.TryParse(value, true, out result))
            {
                Debug.LogError($"{keyName} Error with value {value}");
            }
            return result;
        }

        public static void LogPrint(this MonoBehaviour mono, string logMessage)
        {
            DebugPrintManager.Instance.Log(mono, logMessage);
        }

        public static void WarningPrint(this MonoBehaviour mono, string logMessage)
        {
            DebugPrintManager.Instance.Warning(mono, logMessage);
        }

        public static void ErrorPrint(this MonoBehaviour mono, string logMessage)
        {
            DebugPrintManager.Instance.Error(mono, logMessage);
        }

        public static Transform GetChild(this Transform parent, string goName)
        {
            // 없다면 null
            if (parent == null)
                return null;

            // 해당 녀석이 찾는 놈일 경우
            if (parent.name == goName)
                return parent;

            int childCount = parent.transform.childCount;
            if (childCount == 0)
                return null;

            Transform findGo = null;
            for (int i = 0; i < childCount; i++)
            {
                var childTr = parent.transform.GetChild(i);
                findGo = GetChild(childTr, goName);

                if (findGo != null)
                    break;
            }

            return findGo;
        }
    }
}