using Scripts.Enums;
using Scripts.SO;
using UnityEngine;

namespace Scripts.Generator
{
    public class ItemSOGenerator : SOGenerator
    {
        // base
        private const string DATA_PATH = "CSV/Item/Gear/Gear_CSV_Data"; // 데이터 경로
        private const string SAVE_PATH = "Assets/Resources/SO/Item"; // 저장 경로

        [SerializeField] private int _dataVersion; // 데이터 버전
        [SerializeField] private ItemType _saveItemType; // 아이템 타입

        // custom
        private const string SPRITE_DATA_PATH = "Sprite/Item/Gear";
        public override void CreateEditBtn()
        {
            base.CreateEditBtn();

            string resultDataPath = $"{DATA_PATH}_{_dataVersion}"; // 최종 데이터 경로
            string resultSavePath = $"{SAVE_PATH}/{_saveItemType}"; // 아이템 경로
            string fileName = $"{_saveItemType}_Item"; // 저장 파일 이름

            var datas = CSVReader.Read(resultDataPath); // 데이터 추출

            foreach (var data in datas)
            {
                // 파일 이름
                string saveFindName = $"{fileName}_{data["Id"]}";

                // 아이템 타입
                data.Add(typeof(ItemType).Name, _saveItemType);

                // 아이템 이미지
                Sprite[] itemSprite = Resources.LoadAll<Sprite>($"{SPRITE_DATA_PATH}/{saveFindName}");
                data.Add("ItemSprite", itemSprite);

                CreateAndSetSO<GearItemSO>($"{resultSavePath}/{saveFindName}", data);
            }
        }
    }
}
