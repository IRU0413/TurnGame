using Scripts.Enums;
using UnityEngine;

namespace Scripts.Generator
{
    public class ItemSOGenerator : BaseSOGenerator
    {
        [Header("æ∆¿Ã≈€")]
        [SerializeField] protected ItemType _itemType = ItemType.None;

        public override void CreateSOAssets()
        {
            _datas = GetDatas(_itemType.ToString());
            Debug.Log($"Get Data:{_datas != null}");
        }

        protected string GetPathItemImageInResources(ItemType itemType, int id)
        {
            return $"Sprite/Item/{itemType}/{itemType}_Item_{id}";
        }
    }
}
