﻿using Scripts.Util;
using UnityEngine;

namespace Scripts.Cores
{
    /// <summary>
    /// 유닛의 조립 부품
    /// </summary>
    public abstract class Assembly : MonoBehaviour
    {
        private Core _core;
        protected bool _isInitialized;

        public Core BaseCore => _core;
        public bool IsInitialized => _isInitialized;

        public void Initialized(Core core)
        {
            if (_isInitialized) return;
            if (!core.IsInitialized)
            {
                this.WarningPrint("코어가 초기화 되지 않았습니다");
                return;
            }

            _core = core;
            OnInitialized();
        }
        public void Removed()
        {
            OnDisabled();
            _core = null;
            _isInitialized = false;
        }

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

