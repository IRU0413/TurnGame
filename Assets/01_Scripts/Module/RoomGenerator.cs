using Scripts.Data;
using Scripts.Enums;
using Scripts.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Module
{
    public class RoomGenerator
    {
        private const string ROOM_PREFAB_PATH = "Prefab/Room/";
        private const string ROOM_FIRST_NAME = "Room";
        private const string TEST_ROOM_FIRST_NAME = "TestRoom";

        private Dictionary<RoomType, List<GameObject>> _roomPrefabDictionary = new();

        public RoomGenerator()
        {
            if (_roomPrefabDictionary == null)
                _roomPrefabDictionary = new();

            SettingRoomPrefabs();
            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
        }
        private void SettingRoomPrefabs()
        {
            var roomObjs = GameManager.Resource.LoadAll<GameObject>($"{ROOM_PREFAB_PATH}");
            if (roomObjs == null)
            {
                Debug.LogError("Not Find Room Objects.");
                return;
            }

            foreach (var room in roomObjs)
            {
                // 만약 데이터를 가지고 왔는데 없다면
                if (room == null)
                    continue;

                // _로 나누었을 때
                // [0] => "Room"이라는 문자열,
                // [1] => RoomType 타입,
                // [2] => Index를 나타내는 정수,
                string roomName = room.name;
                string[] roomNameDatas = roomName.Split("_");

                // 이상하게 값이 없거나 자를 문자가 없을 때
                bool useable = roomNameDatas == null || roomNameDatas.Length <= 0 || roomNameDatas.Length > 3;
                if (useable)
                    continue;

                // [0] 검사
                if (roomNameDatas[0] != ROOM_FIRST_NAME && roomNameDatas[0] != TEST_ROOM_FIRST_NAME)
                    continue;

                RoomType keyType = RoomType.None;

                // [1] 이름상의 RoomType 변환 가능 체크
                if (Enum.TryParse(roomNameDatas[1], out keyType))
                {
                    // 이상이 없기다는 거 확인 되었은니까 추가!
                    AddRoomObj(keyType, room);
                }

                // [2] 인덱스인데 필요 없는 정보라 컷
            }
        }
        private void ClearRoomObject(RoomType type)
        {
            _roomPrefabDictionary?.Remove(type);
        }
        public void AllClearRoomObject()
        {
            int startNum = (int)RoomType.None + 1;
            int endNum = (int)RoomType.WeaponRoom;
            for (int i = startNum; i <= endNum; i++)
                ClearRoomObject((RoomType)i);
        }
        private void AddRoomObj(RoomType type, GameObject roomObj)
        {
            // 키없다면 리스트 생성
            if (!_roomPrefabDictionary.ContainsKey(type))
                _roomPrefabDictionary.Add(RoomType.None, new List<GameObject>());

            if (_roomPrefabDictionary[type].Contains(roomObj))
            {
                Debug.LogWarning("This Dictionary Contains Room Game Object.");
                return;
            }

            // 룸 추가
            _roomPrefabDictionary[type].Add(roomObj);
        }

        public RoomData GetRoom(RoomType type)
        {
            if (!_roomPrefabDictionary.ContainsKey(type))
                type = RoomType.None;

            var roomList = _roomPrefabDictionary[type];
            if (roomList.Count <= 0)
                roomList = _roomPrefabDictionary[RoomType.None];

            int luckNumber = UnityEngine.Random.Range(0, roomList.Count);

            var roomPrefab = roomList[luckNumber];
            if (roomPrefab == null)
            {
                return null;
            }

            var roomCmp = roomPrefab.GetComponent<RoomData>();
            if (roomCmp == null)
            {
                return null;
            }

            var newRoom = GameObject.Instantiate(roomPrefab);
            return newRoom.GetComponent<RoomData>();
        }
    }
}