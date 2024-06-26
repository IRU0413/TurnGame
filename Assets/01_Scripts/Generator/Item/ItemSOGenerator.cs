using Scripts.Enums;
using UnityEngine;

namespace Scripts.Generator
{
    public class ItemSOGenerator : BaseSOGenerator
    {
        [Header("������")]
        [SerializeField] protected ItemType _itemType = ItemType.None;

        public override void CreateSOAssets()
        {
            _datas = GetDatas(_itemType.ToString());
            Debug.Log($"Get Data:{_datas != null}");
        }
    }
}
