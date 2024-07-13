namespace Scripts.SO
{
    using Scripts.Enums;
    using UnityEngine;

    public abstract class ItemSO : ScriptableObject
    {
        [SerializeField][HideInInspector] protected int _id;
        [SerializeField] protected string _itemName;
        [SerializeField] protected ItemType _itemType = ItemType.None;
        [SerializeField] protected ItemGradeType _itemGradeType = ItemGradeType.None;

        [SerializeField] protected Sprite[] _itemSprite;
        [SerializeField] protected float _spriteRotationCalibration;

        public int ID => _id;
        public string ItemName => _itemName;
        public ItemType ItemType => _itemType;
        public ItemGradeType ItemGradeType => _itemGradeType;
        public Sprite[] ItemSprite => _itemSprite;
        public float ItemSpriteRotation => _spriteRotationCalibration;
    }
}