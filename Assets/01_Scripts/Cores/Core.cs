using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores
{
    [DisallowMultipleComponent]
    public abstract class Core : MonoBehaviour
    {
        protected bool _isInitialized;

        [SerializeField] private List<Assembly> _assamblies = new List<Assembly>();
        private Transform _tr;

        public bool IsInitialized => _isInitialized;

        public List<Assembly> BaseAssamblies => _assamblies;
        public Transform Tr => _tr;

        /// <summary>
        /// 현재 연결되어있는 부품 찾는 기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAssembly<T>() where T : Assembly
        {
            return _assamblies.Find(x => x is T) as T;
        }

        /// <summary>
        /// 부품 추가 되면서 초기화 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assembly"></param>
        public virtual void AddAssembly<T>(T assembly) where T : Assembly
        {
            if (!_assamblies.Contains(assembly))
                _assamblies.Add(assembly);

            if (_isInitialized)
                assembly.Initialized(this);
        }

        public virtual T GetOrAddAssembly<T>() where T : Assembly
        {
            T assembly = GetAssembly<T>();
            if (assembly == null)
            {
                assembly = this.gameObject.AddComponent<T>();
            }
            return assembly;
        }

        /// <summary>
        /// 연결된 부품을 제거하는 기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assambly"></param>
        public virtual void RemoveAssambly<T>(T assembly) where T : Assembly
        {
            if (!_assamblies.Contains(assembly))
                return;

            _assamblies.Remove(assembly);
            assembly.Removed();
        }

        protected virtual void Initialized()
        {
            if (_isInitialized) return;

            _tr = GetComponent<Transform>();
            _isInitialized = true;

            HaveAssembliesInitalizing();
            this.LogPrint($"Core 초기화");
        }

        // 가지고 있는 부품의 추가
        private void HaveAssembliesInitalizing()
        {
            Assembly[] getAssemblys = GetComponents<Assembly>();
            foreach (var assembly in getAssemblys)
            {
                // 활성화 되지않았다면 
                if (!assembly.enabled) continue;
                AddAssembly(assembly);
            }
        }
    }
}

