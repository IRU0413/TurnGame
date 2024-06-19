using Scripts.Enums;
using Scripts.Module;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Data
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class RoomData : MonoBehaviour
    {
        private Transform _tr;

        private RoomType _roomType = RoomType.None;
        private Vector2Int _roomPosition = Vector2Int.zero;

        private GridController _gridCtrl;
        private List<GameObject> _monstorList = new List<GameObject>();

        #region Editor
        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                InitializeGrid();
            }
        }
        private void Reset()
        {
            InitializeGrid();
        }
        private void InitializeGrid()
        {
            // RoomData�� GameObject�� �߰��� �� �ڵ����� Grid�� ����
            GameObject gridMain = transform.Find("Grid_Main")?.gameObject;
            if (gridMain == null)
            {
                gridMain = new GameObject("Grid_Main");
                gridMain.transform.SetParent(this.transform, false);
                _gridCtrl = gridMain.AddComponent<GridController>();
                _gridCtrl.InitGrid("Main", this.transform);
            }
            else
            {
                _gridCtrl = gridMain.GetComponent<GridController>();
                if (_gridCtrl == null)
                {
                    _gridCtrl = gridMain.AddComponent<GridController>();
                    _gridCtrl.InitGrid("Main", this.transform);
                }
            }

            _tr = this.transform;
        }
        // �����Ϳ� ��ư ����
        public void AddTilemap(LayerType layerType)
        {
            if (_gridCtrl == null)
            {
                Debug.LogWarning("GridController is not assigned.");
                return;
            }

            string tilemapName = "Tilemap_" + layerType.ToString();

            // �ߺ� üũ
            Transform existingTilemap = _gridCtrl.transform.Find(tilemapName);
            if (existingTilemap != null)
            {
                Debug.LogWarning($"{tilemapName} already exists under Grid_Main.");
                return;
            }

            // Tilemap �߰�
            _gridCtrl.CreateTilemap(layerType);
        }

        #endregion

        public void InitRoomData(RoomType roomtype, Vector2Int pos, int gapLength = 50)
        {
            _tr = this.GetComponent<Transform>();
            _tr.gameObject.name = $"{roomtype}_{pos}";

            _roomPosition = pos;
            _roomType = roomtype;

            // ��ġ ����
            _tr.position = new Vector3(_roomPosition.x * gapLength, _roomPosition.y * gapLength, 0);

            // �ش� �濡 GridController �߰� �� ������ ����
            _gridCtrl = _tr.GetComponentInChildren<GridController>();
            _gridCtrl.InitGrid(pos.ToString(), _tr);
            _gridCtrl.SetActiveGrid(false);
        }
        public void UseRoom()
        {
            _gridCtrl.SetActiveGrid(true);
            _gridCtrl.SetActiveAllTilemap(true);
        }

    }
}