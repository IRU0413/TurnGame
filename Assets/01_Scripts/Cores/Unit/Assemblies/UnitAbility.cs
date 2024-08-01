using Scripts.Enums;
using System.Collections.Generic;
using System.Text;

namespace Scripts.Cores.Unit.Assemblies
{
    public class UnitAbility : UnitAssembly
    {
        private Dictionary<UnitAbilityType, float> _currentAbility = new();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _currentAbility = Unit.BaseAbility.GetCurrentAbility();
            UnityEngine.Debug.Log(ToString());
        }

        public float GetAbility(UnitAbilityType abilityType)
        {
            return _currentAbility[abilityType];
        }
        public void SetAbility(UnitAbilityType abilityType, float value)
        {
            _currentAbility[abilityType] = value;
        }
        public void SetAbility(UnitAbilityType abilityType, float point, float percent)
        {
            var baseValue = Unit.BaseAbility.GetAbility(abilityType);
            _currentAbility[abilityType] = baseValue + (baseValue * percent) + point;
        }

        public override string ToString()
        {
            var resultStr = new StringBuilder();
            foreach (var ability in _currentAbility)
                resultStr.AppendLine($"[{ability.Key}]: {ability.Value}");

            return resultStr.ToString();
        }
    }
}