namespace Scripts.SO
{
    using Scripts.Enums;
    using UnityEngine;

    public abstract class ItemSO : ScriptableObject
    {
        // Base Data
        [HideInInspector] public int Id;
        public string ItemName;
        public ItemType ItemType = ItemType.None;
        public ItemGradeType ItemGradeType = ItemGradeType.None;

        public Sprite[] ItemSprite;
        public float SpriteRotationCalibration;
    }
}