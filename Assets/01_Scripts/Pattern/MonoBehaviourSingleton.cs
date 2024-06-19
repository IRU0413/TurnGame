using UnityEngine;

namespace Scripts.Pattern
{
    [DisallowMultipleComponent]
    public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool _isDontDestroyOnLoad = true;
        private static T _instance;
        private bool _isInitialized = false;
        public bool IsInitialized => _isInitialized;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<T>();
                        singletonObject.name = $"@{typeof(T).Name}";
                    }
                }
                return _instance;
            }
        }


        private void Awake()
        {
            if (_instance != null && _instance != this as T)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T;

                if (_isDontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }

            Initialize();
            if (!_isInitialized)
            {
                // T 타입의 게임 오브젝트는 초기화가 안되었기에 해당 게임 오브젝트는 삭제됩니다.
                Debug.LogError($"Since the game object of that {_instance.GetType()} has not been initialized, the game object is deleted.");
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void Initialize()
        {
            _isInitialized = true;
        }

        protected virtual void OnUpdate() { }
    }
}
