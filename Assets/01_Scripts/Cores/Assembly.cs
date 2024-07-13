using Scripts.Util;
using UnityEngine;

namespace Scripts.Cores
{
    /// <summary>
    /// 유닛의 조립 부품
    /// </summary>
    public abstract class Assembly : MonoBehaviour
    {
        private Core _core;
        private bool _isInitialized;

        public Core Base => _core;
        public bool IsInitialized => _isInitialized;

        public void Initialized(Core core)
        {
            if (_isInitialized) return;

            _core = core;

            // 오브젝트 로드 및 파츠 추가
            OnBeforeInitialization();
            OnInitialized();
        }
        public void Removed()
        {
            OnDisabled();
            _core = null;
            _isInitialized = false;
        }

        protected virtual void OnBeforeInitialization() { }
        protected virtual void OnInitialized()
        {
            this.LogPrint("초기화 및 Core와 연결이 되었습니다.");
            _isInitialized = true;
        }
        protected virtual void OnDisabled()
        {
            _isInitialized = false;
            this?.WarningPrint($"해당 Core와의 연결이 해제되었습니다.");
        }

        protected virtual void OnEnable()
        {
            var core = GetComponent<Core>();
            core?.AddAssembly(this);
        }
        protected virtual void OnDisable()
        {
            _core?.RemoveAssambly(this);
        }
    }
}

