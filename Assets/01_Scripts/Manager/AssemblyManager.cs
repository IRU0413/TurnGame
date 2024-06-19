using Scripts.Cores;
using Scripts.Pattern;
using System.Collections.Generic;

namespace Scripts.Manager
{
    public class AssemblyManager : MonoBehaviourSingleton<AssemblyManager>
    {
        private Dictionary<Core, List<Assembly>> _notInitUnit = new Dictionary<Core, List<Assembly>>();

        public void AddLateInitialize(Core core, Assembly assembly)
        {
            if (core == null || assembly == null) return;
            if (!_notInitUnit.ContainsKey(core))
            {
                _notInitUnit.Add(core, new());
            }

            List<Assembly> coreAssemblies = _notInitUnit[core];
            if (!coreAssemblies.Contains(assembly))
                coreAssemblies.Add(assembly);
        }
        public void InitializeCoreAssemblies(Core core)
        {
            if (!_notInitUnit.ContainsKey(core))
                return;

            List<Assembly> coreAssemblies = _notInitUnit[core];
            foreach (var assembly in coreAssemblies)
            {
                assembly.Initialized(core);

                if (!assembly.IsInitialized)
                    core.RemoveAssambly(assembly);
            }

            _notInitUnit.Remove(core);
        }
    }
}