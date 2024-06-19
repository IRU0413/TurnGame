using Scripts.Enums;
using Scripts.Module;
using Scripts.Pattern;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class StageManager : MonoBehaviourSingleton<StageManager>
    {
        private StageSO _so = null;
        private StageGridGenerator _gridGenerator = null;

        [SerializeField] private int _stageNum = 0;

        public StageSO SO => _so;

        protected override void Initialize()
        {
            Setting();

            _so = GetStageData(_stageNum);
            if (GridSetting())
                base.Initialize();
        }

        private void Setting()
        {
            var gameType = GameManager.GameType;

            _gridGenerator = new StageGridGenerator(gameType);

            switch (gameType)
            {
                case GameType.Test:
                    _stageNum = 0;
                    break;
                case GameType.Normal:
                    _stageNum = 1;
                    break;
            }
        }
        private bool GridSetting()
        {
            bool result = false;

            _gridGenerator.ClearMap();
            result = _gridGenerator.CreateStageGrid(_so);
            _gridGenerator.CountingAllRoom();

            return result;
        }
        private StageSO GetStageData(int stageNum)
        {
            string fileName = $"SO/Stage/Stage_{stageNum}_SO";
            return GameManager.Resource.Load<StageSO>(fileName);
        }

        // Util
        public List<Vector2Int> GetRoomTypeList(RoomType type)
        {
            if (_gridGenerator.Map.TryGetValue(type, out List<Vector2Int> haveList))
            {
                return haveList;
            }

            this.WarningPrint($"해당 룸 타입({type})은 생성된 룸들 중 존재 하지 않습니다.");
            return new List<Vector2Int>();
        }
    }
}