using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Manager
{
    public class ResourceManager
    {
        private const string RESOURCE_PATH = "Prefab/Managers/";
        private const string CSV_DATA_PATH = "CSV/";
        private const string SO_DATA_PATH = "SO/";

        public T Load<T>(string path) where T : Object
        {
            return Resources.Load<T>(path);
        }

        public T[] LoadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>($"Prefab/{path}");

            if (prefab == null)
            {
                Debug.Log($"Failed to load prefab : {path}");
                return null;
            }

            GameObject go = Object.Instantiate(prefab, parent);
            int index = go.name.IndexOf("(Clone)");
            if (index > 0)
                go.name = go.name.Substring(0, index);

            return go;
        }

        public void Destroy(GameObject gameObject)
        {
            if (gameObject == null)
            {
                Debug.Log("Faild to Destroy GameObject -> value is Null");
                return;
            }

            Object.Destroy(gameObject);
        }

        public T LoadManager<T>() where T : MonoBehaviourSingleton<T>
        {
            // T Ÿ���� �̸����� ��ȯ�Ͽ� ���� ������Ʈ ã��
            string objectName = $"@{typeof(T).Name}";
            GameObject existingObject = GameObject.Find(objectName);

            if (existingObject != null)
            {
                T manager = existingObject.GetComponent<T>();
                if (manager != null && manager.IsInitialized)
                {
                    return manager;
                }
            }

            // �ش� �̸��� ������Ʈ�� �������� ������ Resource�� ���� �ε��Ͽ� ����
            T resourceManager = Load<T>(RESOURCE_PATH + objectName);
            if (resourceManager != null)
            {
                GameObject newManagerObject = GameObject.Instantiate(resourceManager.gameObject);
                newManagerObject.name = objectName;

                T managerInstance = newManagerObject.GetComponent<T>();
                if (managerInstance != null && managerInstance.IsInitialized)
                {
                    return managerInstance;
                }
            }

            // ������ ��� false ��ȯ
            return null;
        }
    }
}

