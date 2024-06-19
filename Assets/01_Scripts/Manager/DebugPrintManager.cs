using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Manager
{
    public class DebugPrintManager : MonoBehaviourSingleton<DebugPrintManager>
    {
        private string _managerName;
        [SerializeField] private bool _isDebug = true;
        protected override void Initialize()
        {
            base.Initialize();
            _managerName = GetType().Name;
        }

        private void PrintLog(string gameObjectName, string csTypeName, string logMessage, LogType logType = LogType.Log)
        {
            if (!_isDebug) return;
            string log = $"[{_managerName}]:[{gameObjectName}] > [{csTypeName}] * {logMessage}";
            switch (logType)
            {
                case LogType.Log:
                    Debug.Log(log);
                    break;
                case LogType.Warning:
                    Debug.LogWarning(log);
                    break;
                case LogType.Error:
                    Debug.LogError(log);
                    break;
            }
        }

        public void Log(MonoBehaviour monoBehaviour, string logMessage)
        {
            PrintLog(monoBehaviour.gameObject.name, monoBehaviour.GetType().Name, logMessage, LogType.Log);
        }
        public void Warning(MonoBehaviour monoBehaviour, string logMessage)
        {
            PrintLog(monoBehaviour.gameObject.name, monoBehaviour.GetType().Name, logMessage, LogType.Warning);
        }
        public void Error(MonoBehaviour monoBehaviour, string logMessage)
        {
            PrintLog(monoBehaviour.gameObject.name, monoBehaviour.GetType().Name, logMessage, LogType.Error);
        }
    }
}
