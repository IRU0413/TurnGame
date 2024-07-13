using Scripts.Cores.Item.Gear;
using Scripts.Enums;
using Scripts.Pattern;
using Scripts.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitEquipment : UnitAssembly
    {
        [Serializable]
        private class PocketGear
        {
            [SerializeField] private GearItemCore _gearItemCore;
            public List<SpriteRenderer> GearRenderers;

            public bool IsHaveRenderer => (GearRenderers != null);
            public GearItemCore GearItem
            {
                get => _gearItemCore;
                set
                {
                    if (value == null)
                    {
                        _gearItemCore = null;
                        int rCount = GearRenderers.Count;
                        for (int i = 0; i < rCount; i++)
                        {
                            GearRenderers[i].transform.Rotate(Vector3.zero);
                            GearRenderers[i].sprite = null;
                        }
                    }
                    else
                    {
                        _gearItemCore = value;

                        int sCount = _gearItemCore.ItemSprite.Length;
                        int rCount = GearRenderers.Count;
                        int minCount = Math.Min(sCount, rCount);

                        for (int i = 0; i < minCount; i++)
                        {
                            GearRenderers[i].transform.Rotate(new Vector3(0, 0, _gearItemCore.GearSpriteRotation));
                            GearRenderers[i].sprite = _gearItemCore.ItemSprite[i];
                        }
                    }
                }
            }
            public SerializableDictionary<UnitAbilityType, Ability> Abilities => _gearItemCore?.Abillities;
        }

        private SerializableDictionary<GearType, PocketGear> _gears;

        protected override void OnBeforeInitialization()
        {
            Unit.GetOrAddAssembly<UnitAnimator>();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            // Setting
            SetGearDictionary();
        }

        private void SetGearDictionary()
        {
            _gears = new();

            int startNum = (int)GearType.Helmet;
            int endNum = (int)GearType.Shield;
            for (int i = startNum; i <= endNum; i++)
            {
                GearType type = (GearType)i;
                List<SpriteRenderer> list = new();
                switch (type)
                {
                    case GearType.Helmet:
                        AddGearPartRenderer(list, "11_Helmet1");
                        break;
                    case GearType.InnerTop:
                        AddGearPartRenderer(list, "ClothBody");
                        AddGearPartRenderer(list, "21_LCArm");
                        AddGearPartRenderer(list, "-19_RCArm");
                        break;
                    case GearType.OuterTop:
                        AddGearPartRenderer(list, "BodyArmor");
                        AddGearPartRenderer(list, "25_L_Shoulder");
                        AddGearPartRenderer(list, "-15_R_Shoulder");
                        break;
                    case GearType.Bottom:
                        AddGearPartRenderer(list, "_2L_Cloth");
                        AddGearPartRenderer(list, "_11R_Cloth");
                        break;
                    case GearType.Back:
                        AddGearPartRenderer(list, "Back");
                        break;
                    case GearType.Weapon:
                        AddGearPartRenderer(list, "R_Weapon");
                        break;
                    case GearType.Shield:
                        AddGearPartRenderer(list, "L_Shield");
                        break;
                }
                var gear = new PocketGear();
                gear.GearRenderers = list;

                _gears.Add(type, gear);
            }
            _gears.OnBeforeSerialize();
        }
        // 부분 파츠 랜더러 위치 추가
        private void AddGearPartRenderer(List<SpriteRenderer> gearPartRendererList, string gearPartName)
        {
            var gear = Unit.Tr.GetChild(gearPartName);
            if (gear == null) return;

            var renderer = gear.GetComponent<SpriteRenderer>();
            if (renderer == null) return;

            gearPartRendererList.Add(renderer);
        }
        // 장착
        public void Equip(GearItemCore gearItem)
        {
            if (gearItem == null) return;

            // 필요한 포켓 가지고 오기
            PocketGear pocket = _gears.GetValue(gearItem.GearType);

            // 포켓 사용 여부
            if (pocket == null)
                return;

            // 포켓에 지금 가지고 있는 장비가 있는지.
            if (pocket.GearItem != null)
                Unequip(pocket);

            // 장비 장착
            pocket.GearItem = gearItem;

            var inven = Unit.GetAssembly<UnitInventory>();
            if (inven != null)
                inven.RemoveItem(gearItem);
            pocket.GearItem.VisualGO.SetActive(false);
            return;
        }
        // 해제
        public void Unequip(GearType gearType)
        {
            if (gearType == GearType.None)
                return;
            PocketGear pocket = _gears.GetValue(gearType);
            Unequip(pocket);
        }
        private void Unequip(PocketGear pocket)
        {
            if (pocket == null)
                return;

            // 인벤에 추가
            var inven = Unit.GetAssembly<UnitInventory>();
            if (inven != null)
                inven.AddItem(pocket.GearItem);
            else
            {
                pocket.GearItem.VisualGO.SetActive(true);
            }
            // 해제
            pocket.GearItem = null;
        }
    }
}