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
                // ���� �����͸� ������ �Դµ� ���ٸ�
                if (room == null)
                    continue;

                // _�� �������� ��
                // [0] => "Room"�̶�� ���ڿ�,
                // [1] => RoomType Ÿ��,
                // [2] => Index�� ��Ÿ���� ����,
                string roomName = room.name;
                string[] roomNameDatas = roomName.Split("_");

                // �̻��ϰ� ���� ���ų� �ڸ� ���ڰ� ���� ��
                bool useable = roomNameDatas == null || roomNameDatas.Length <= 0 || roomNameDatas.Length > 3;
                if (useable)
                    continue;

                // [0] �˻�
                if (roomNameDatas[0] != ROOM_FIRST_NAME && roomNameDatas[0] != TEST_ROOM_FIRST_NAME)
                    continue;

                RoomType keyType = RoomType.None;

                // [1] �̸����� RoomType ��ȯ ���� üũ
                if (Enum.TryParse(roomNameDatas[1], out keyType))
                {
                    // �̻��� ����ٴ� �� Ȯ�� �Ǿ����ϱ� �߰�!
                    AddRoomObj(keyType, room);
                }

                // [2] �ε����ε� �ʿ� ���� ������ ��
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
            // Ű���ٸ� ����Ʈ ����
            if (!_roomPrefabDictionary.ContainsKey(type))
                _roomPrefabDictionary.Add(RoomType.None, new List<GameObject>());

            if (_roomPrefabDictionary[type].Contains(roomObj))
            {
                Debug.LogWarning("This Dictionary Contains Room Game Object.");
                return;
            }

            // �� �߰�
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