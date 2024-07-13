using Scripts.Cores;
using Scripts.Pattern;
using System.Collections.Generic;

namespace Scripts.Manager
{
    public class AssemblyManager : MonoBehaviourSingleton<AssemblyManager>
    {
        private Dictionary<Assembly, List<Assembly>> _needAssemblies = new();

        public void Add(Assembly applicant, List<Assembly> applicationList)
        {

        }

        public void ApplicationProcessing(Assembly applicant)
        {

        }
    }
}