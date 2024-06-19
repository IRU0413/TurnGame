using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Manager
{
    public class DataLoader
    {
        private const string RESOURCE_PATH = "Prefab/Managers";
        public bool LoadManager<T>() where T : MonoBehaviourSingleton<T>
        {
            // T Ÿ���� �̸����� ��ȯ�Ͽ� ���� ������Ʈ ã��
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

            // �ش� �̸��� ������Ʈ�� �������� ������ Resource�� ���� �ε��Ͽ� ����
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

            // ������ ��� false ��ȯ
            return false;
        }

    }

}