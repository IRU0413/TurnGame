using Scripts.Enums;
using Scripts.Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Scene
{
    public class TestScene : BaseScene
    {
        public override void Init()
        {
            SceneType = SceneType.Test;

            // init
            InitStage();
            InitRoom();

            // start
            StartStage();
            StartRoom();
        }
        private void InitStage()
        {
            GameManager.Resource.LoadManager<StageManager>();
        }
        private void InitRoom()
        {
            GameManager.Resource.LoadManager<RoomManager>();
        }

        private void StartStage()
        {

        }
        private void StartRoom()
        {
            int roomTypeEndNum = (int)RoomType.WeaponRoom;
            List<Vector2Int> typeRoomList = null;

            for (int i = 1; i <= roomTypeEndNum; i++)
            {
                RoomType roomType = (RoomType)i;
                if (roomType == RoomType.EventRoom)
                    continue;
                typeRoomList = StageManager.Instance.GetRoomTypeList(roomType);
                // 리스트 있는지 확인
                if (typeRoomList.Count <= 0)
                    continue;

                foreach (var pos in typeRoomList)
                    RoomManager.Instance.Add(roomType, pos);
            }

            // RoomManager.Instance.Play(Vector2Int.zero);
        }

        private void UpdateStage()
        {

        }

        private void Update()
        {
            UpdateStage();
        }
    }
}