using Scripts.Data;
using Scripts.Enums;
using Scripts.Module;
using Scripts.Pattern;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class RoomManager : MonoBehaviourSingleton<RoomManager>
    {
        private RoomGenerator _roomGenerator = null;
        private Dictionary<Vector2Int, RoomData> _rooms;
        private GameObject _roomGroup = null;
        private RoomData _crrentRoom;
        protected override void Initialize()
        {
            _roomGenerator = new RoomGenerator();
            _rooms = new Dictionary<Vector2Int, RoomData>();
            _roomGroup = new("RoomGroup");

            base.Initialize();
        }

        public void Add(RoomType type, Vector2Int pos)
        {
            // �ߺ� Ű üũ
            if (_rooms.ContainsKey(pos))
            {
                Debug.LogWarning($"Room at position {pos} already exists.");
                return;
            }

            // �� ������Ʈ
            var roomCmp = _roomGenerator.GetRoom(type);
            if (roomCmp == null)
            {
                Debug.LogError($"{type}_{pos}�� �ش��ϴ� ���� �������� ���߽��ϴ�.");
                return;
            }

            // �� �ʱ�ȭ
            roomCmp.InitRoomData(type, pos);

            // �߰�
            _rooms.Add(pos, roomCmp);
            roomCmp.gameObject.transform.parent = _roomGroup.transform;
        }

        public void Use()
        {
            // Implement the use logic here if needed.
        }
    }
}
