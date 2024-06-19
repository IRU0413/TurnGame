using Scripts.Cores.Unit;
using Scripts.Enums;
using Scripts.Pattern;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class UnitManager : MonoBehaviourSingleton<UnitManager>
    {
        private List<UnitType> _PlayerFriendlyType = new List<UnitType>(); // 플레이어에게 우호적인 유닛 타입
        private List<UnitType> _MonsterFriendlyType = new List<UnitType>(); // 몬스터에게 우호적인 유닛 타입
        private List<UnitType> _NPCFriendlyType = new List<UnitType>(); // NPC에게 우호적인 유닛 타입

        private List<UnitCore> _units = new List<UnitCore>();

        public void Add(UnitCore unit)
        {
            if (!unit.IsInitialized) return;
            if (_units.Contains(unit)) return;

            _units.Add(unit);
            this.LogPrint($"[Unit >> Add Name]: {unit.gameObject.name}");
        }
        public void Remove(UnitCore unit)
        {
            if (!_units.Contains(unit))
                return;

            _units.Remove(unit);
            this.LogPrint($"[Unit >> Remove Name]: {unit.gameObject.name}");
        }
        public bool ContainsUnit(UnitCore unit)
        {
            return _units.Contains(unit);
        }

        public List<UnitCore> GetUnitsInRange(List<Vector2> nodesPos, Vector2 point)
        {
            List<UnitCore> nowUnits = new List<UnitCore>(_units);
            List<UnitCore> unitsInRange = new();

            // 범위 안에 있는 유닛들(플레이어도 당연히 탐색됨 > 버프 관련된 것 때문에) 
            foreach (var nodePos in nodesPos)
            {
                for (int i = 0; i < nowUnits.Count; i++)
                {
                    if ((nodePos + point) == (Vector2)nowUnits[i].Tr.position)
                    {
                        if (!unitsInRange.Contains(nowUnits[i]))
                        {
                            unitsInRange.Add(nowUnits[i]);
                            nowUnits.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }

            return unitsInRange;
        }
    }
}