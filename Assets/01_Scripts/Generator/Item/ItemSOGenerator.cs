using Scripts.Enums;
using Scripts.SO;
using UnityEngine;

namespace Scripts.Generator
{
    public class ItemSOGenerator : SOGenerator
    {
        // base
        private const string DATA_PATH = "CSV/Item/Gear/Gear_CSV_Data"; // ������ ���
        private const string SAVE_PATH = "Assets/Resources/SO/Item"; // ���� ���

        [SerializeField] private int _dataVersion; // ������ ����
        [SerializeField] private ItemType _saveItemType; // ������ Ÿ��

        // custom
        private const string SPRITE_DATA_PATH = "Sprite/Item/Gear";
        public override void CreateEditBtn()
        {
            base.CreateEditBtn();

            string resultDataPath = $"{DATA_PATH}_{_dataVersion}"; // ���� ������ ���
            string resultSavePath = $"{SAVE_PATH}/{_saveItemType}"; // ������ ���
            string fileName = $"{_saveItemType}_Item"; // ���� ���� �̸�

            var datas = CSVReader.Read(resultDataPath); // ������ ����

            foreach (var data in datas)
            {
                // ���� �̸�
                string saveFindName = $"{fileName}_{data["Id"]}";

                // ������ Ÿ��
                data.Add(typeof(ItemType).Name, _saveItemType);

                // ������ �̹���
                Sprite[] itemSprite = Resources.LoadAll<Sprite>($"{SPRITE_DATA_PATH}/{saveFindName}");
                data.Add("ItemSprite", itemSprite);

                CreateAndSetSO<GearItemSO>($"{resultSavePath}/{saveFindName}", data);
            }
        }
    }
}
