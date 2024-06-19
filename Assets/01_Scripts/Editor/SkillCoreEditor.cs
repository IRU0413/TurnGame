using Scripts.Architecture;
using Scripts.Cores.Skills.Skill;
using Scripts.Cores.Skills.SkillObject;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkillCore))]
public class SkillCoreEditor : Editor
{
    private const string ACTION_SLOT_NAME = "ActionSkillSlot";
    private const char SPLIT_POINT = '_';

    private int _createdSlotCount = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // 기본 인스펙터 GUI를 그립니다.

        SkillCore core = (SkillCore)target;

        int nowSlotCount = 0;
        // core.SkillActionDataList.Count;

        if (nowSlotCount != _createdSlotCount)
        {
            CreateAndSettingSlot(core, nowSlotCount);
            _createdSlotCount = nowSlotCount;
        }

        /*if (GUILayout.Button("Generate Skill Objects"))
        {
            core.GenerateSkillObjects(core);
        }*/
    }

    private void CreateAndSettingSlot(SkillCore core, int nowSlotCount)
    {
        Transform[] childs = core.gameObject.transform.GetComponentsInChildren<Transform>();
        List<Transform> slotObjectList = new();

        // 가지고 있는 슬롯 중에 범위 안에 슬롯 들가지고 오고, 나머지 지우기
        foreach (var child in childs)
        {
            if (IsSlot(child.gameObject.name))
            {
                int slotNumber = GetSlotNumber(child.gameObject.name);
                if (slotNumber < 0 || (nowSlotCount - 1) < slotNumber)
                {
                    DestroyImmediate(child.gameObject);
                }
                else
                {
                    slotObjectList.Add(child);
                }
            }
        }

        for (int i = 0; i < nowSlotCount; i++)
        {
            string checkSlotName = $"{ACTION_SLOT_NAME}_{i}";
            bool isIn = false;
            Transform inTr = null;

            for (int j = 0; j < slotObjectList.Count; j++)
            {
                string slotName = slotObjectList[j].gameObject.name;
                if (slotName.Equals(checkSlotName))
                {
                    inTr = slotObjectList[j];
                    isIn = true;
                    break;
                }
            }

            SkillActionData stepSkills = new();
            // core.SkillActionDataList[i];
            stepSkills.ActionSkillList.Clear();
            if (isIn)
            {
                SkillObjectCore[] skillObjs = inTr.GetComponentsInChildren<SkillObjectCore>();
                stepSkills.ActionSkillList.AddRange(skillObjs.ToList());
            }
            else
            {
                GameObject newSlot = new GameObject(checkSlotName);
                newSlot.transform.SetParent(core.gameObject.transform);
            }
        }
        EditorUtility.SetDirty(target);
    }

    private bool IsSlot(string gameobjectName)
    {
        string[] names = gameobjectName.Split(SPLIT_POINT);

        if (names.Length <= 1) return false;

        bool isSlot = ACTION_SLOT_NAME.Equals(names[0]);
        if (!isSlot) return false;

        return true;
    }
    private int GetSlotNumber(string gameobjectName)
    {
        int slotNumber = -1;
        string[] names = gameobjectName.Split(SPLIT_POINT);

        if (!IsSlot(gameobjectName))
            return slotNumber;

        int.TryParse(names[1], out slotNumber);
        return slotNumber;
    }

    /*public void GenerateSkillObjects(SkillCore core)
    {
        string ACTION_SLOT_NAME = "ActionSkillSlot";
        char SPLIT_POINT = '_';

        // GameObject를 정리합니다.
        // 개수 만큼 생성하려함
        // 만약 생성되어 있다면 생성하지 않음.
        // 생성하려는 개수를 넘었자면 삭제

        Transform[] childs = core.gameObject.transform.GetComponentsInChildren<Transform>();
        int slotCount = core.SkillActionDataList.Count;
        foreach (Transform child in childs)
        {
            string[] names = child.name.Split(SPLIT_POINT);

            if (names.Length <= 1)
                continue;

            bool isSlot = ACTION_SLOT_NAME.Equals(names[0]);
            if (!isSlot)
                continue;

            int number = -1;
            if (!int.TryParse(names[1], out number))
                continue;
            if (number < 0)
                continue;

            if ((slotCount - 1) < number)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                SkillObjectCore[] skillObjs = child.GetComponentsInChildren<SkillObjectCore>();
                SkillActionData stepSkills = core.SkillActionDataList[number];

                stepSkills.ActionSkillList.Clear();
                stepSkills.ActionSkillList.AddRange(skillObjs.ToList());
            }
        }

        // 새로운 GameObject를 생성합니다.
        for (int i = 0; i < core.SkillActionDataList.Count; i++)
        {
            string slotName = $"{ACTION_SLOT_NAME}_{i}";
            bool isInName = false;
            foreach (var child in childs)
            {
                isInName = child.gameObject.name.Equals(slotName);
                if (isInName)
                    break;
            }

            if (isInName)
                continue;

            GameObject newSlot = new GameObject(slotName);
            newSlot.transform.SetParent(core.gameObject.transform);
        }
    }*/
}
