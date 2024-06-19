using Scripts.Cores.Unit.ActinoUnit.Assemblies;
using Scripts.Util;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbilityActionCmp))]
public class AbilityControlCmpEditor : Editor
{
    private const string ACTION_SLOT_NAME = "ActionSkillSlot";
    private const char SPLIT_POINT = '_';


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 인스펙터 GUI를 그립니다.

        var cmp = (AbilityActionCmp)target;
        if (!cmp.IsInitialized)
            return;

        if (GUILayout.Button("Have Skills LogPrint"))
        {
            PrintHaveAbilitys(cmp);
        }
    }

    public void PrintHaveAbilitys(AbilityActionCmp cmp)
    {
        cmp.LogPrint(cmp.GetAbility(Scripts.Enums.AbilityType.Weapon, 0)?.ToString());

        cmp.LogPrint(cmp.GetAbility(Scripts.Enums.AbilityType.Passive, 0)?.ToString());

        cmp.LogPrint(cmp.GetAbility(Scripts.Enums.AbilityType.Action, 0)?.ToString());
        cmp.LogPrint(cmp.GetAbility(Scripts.Enums.AbilityType.Action, 1)?.ToString());
    }

}
