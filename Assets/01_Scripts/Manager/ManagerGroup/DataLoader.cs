using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Manager
{
    public class DataLoader
    {
        private const string RESOURCE_PATH = "Prefab/Managers";
        public bool LoadManager<T>() where T : MonoBehaviourSingleton<T>
        {
            // T 타입을 이름으로 변환하여 게임 오브젝트 찾기
            string objectName = $"@{typeof(T).Name} (Singleton)";
            GameObject existingObject = GameObject.Find(objectName);

            if (existingObject != null)
            {
                T manager = existingObject.GetComponent<T>();
                if (manager != null && manager.IsInitialized)
                {
                    return true;
                }
            }

            // 해당 이름의 오브젝트가 존재하지 않으면 Resource를 통해 로드하여 생성
            T resourceManager = Resources.Load<T>(RESOURCE_PATH);
            if (resourceManager != null)
            {
                GameObject newManagerObject = GameObject.Instantiate(resourceManager.gameObject);
                newManagerObject.name = objectName;

                T managerInstance = newManagerObject.GetComponent<T>();
                if (managerInstance != null && managerInstance.IsInitialized)
                {
                    return true;
                }
            }

            // 실패한 경우 false 반환
            return false;
        }

    }

}