using Scripts.Cores.Unit;
using Scripts.Cores.Unit.Assemblies;
using Unity.VisualScripting;
using UnityEditor;

namespace Scripts.Edit
{
    [CustomEditor(typeof(UnitCore))]
    public class UnitCoreEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // 기본 인스펙터 GUI를 그립니다.

            var unit = (UnitCore)target;
            if (unit != null)
            {
                switch (unit.UnitType)
                {
                    case Enums.UnitType.Player:
                        var aCtrl = unit.GetComponent<PlayerAttackController>();
                        unit.GetOrAddComponent<PlayerAttackController>();

                        var mCtrl = unit.GetComponent<PlayerMoveController>();
                        unit.GetOrAddComponent<PlayerMoveController>();
                        break;
                    case Enums.UnitType.Monster:

                        break;
                    case Enums.UnitType.NPC:
                        break;
                    case Enums.UnitType.Object:
                        break;
                }
            }
        }
    }
}