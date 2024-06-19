using Scripts.Enums;
using Scripts.Manager;
using Scripts.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.Module
{
    public class GridController : MonoBehaviour
    {
        private Grid _grid;
        private Dictionary<LayerType, Tilemap> _tilemapDictionary = new Dictionary<LayerType, Tilemap>();

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _grid = gameObject.GetOrAddComponent<Grid>();
            var tilemaps = gameObject.GetComponentsInChildren<Tilemap>();

            int startNum = (int)LayerType.Background;
            int endNum = (int)LayerType.UIEffect;
            foreach (var tilemap in tilemaps)
            {
                // 현 타일맵 이름
                var goName = tilemap.gameObject.name;

                // 현 타입맵 이름이 LyaerType중에 있다면 추가
                for (int i = startNum; i <= endNum; i++)
                {
                    LayerType type = (LayerType)i;
                    string findStr = type.ToString();

                    if (goName.Contains(findStr))
                    {
                        _tilemapDictionary.Add(type, tilemap);
                        break;
                    }
                }
            }
        }

        public void InitGrid(string gridName, Transform parentTransform = null)
        {
            var createName = "Grid_" + gridName;
            gameObject.name = createName;
            if (parentTransform != null)
            {
                transform.parent = parentTransform;
            }
            GridManager.Instance.AddGridController(this);
            SetActiveGrid(false);
        }

        public void SetActiveGrid(bool isActive)
        {
            if (_grid == null)
                return;
            if (_grid.gameObject.activeSelf == isActive)
                return;

            _grid.gameObject.SetActive(isActive);
        }

        public void SetActiveLayerTypeTilemap(LayerType type, bool isActive)
        {
            // 타일맵이 없던가? || 그리드가 활성화 안됨?
            if (!_tilemapDictionary.ContainsKey(type) || !_grid.gameObject.activeSelf)
            {
                Debug.LogWarning($"{type} 레이어를 찾을 수 없습니다.");
                return;
            }

            var tilemap = _tilemapDictionary[type];
            tilemap.gameObject.SetActive(isActive);
        }
        public void SetActiveAllTilemap(bool isActive)
        {
            int startNum = (int)LayerType.Background;
            int endNum = (int)LayerType.UIEffect;

            for (int i = startNum; i <= endNum; i++)
                SetActiveLayerTypeTilemap((LayerType)i, isActive);
        }
        public bool ContainsLayerType(LayerType type)
        {
            var typeTilemap = _tilemapDictionary[type];
            if (typeTilemap == null)
                return false;
            return typeTilemap.gameObject.activeSelf;
        }
        public List<LayerType> GetLayerTypes()
        {
            var result = _tilemapDictionary.Keys.ToList();
            return result;
        }

        public bool DeleteGrid()
        {
            if (_grid != null)
            {
                foreach (var tilemap in _tilemapDictionary.Values)
                {
                    DestroyImmediate(tilemap.gameObject);
                }
                _tilemapDictionary.Clear();
                DestroyImmediate(_grid.gameObject);
                _grid = null;
                return true;
            }
            return false;
        }

        public void CreateTilemap(LayerType type)
        {
            if (_tilemapDictionary.ContainsKey(type))
            {
                Debug.LogWarning("이미 해당 타입의 타일맵이 존재합니다.");
                return;
            }

            string tilemapName = "Tilemap_" + type.ToString();
            Tilemap tilemap = CreateTilemap(transform, tilemapName, (int)type);
            _tilemapDictionary[type] = tilemap;
            CreateTilemapSetting(type, tilemap);
        }

        private void CreateTilemapSetting(LayerType type, Tilemap tilemap)
        {
            switch (type)
            {
                case LayerType.Wall:
                    // 추가
                    tilemap.gameObject.GetOrAddComponent<TilemapCollider2D>();
                    tilemap.gameObject.GetOrAddComponent<CompositeCollider2D>();
                    var rigd2D = tilemap.gameObject.GetOrAddComponent<Rigidbody2D>();
                    rigd2D.gravityScale = 0;
                    break;

                default:
                    break;
            }

        }

        private Tilemap CreateTilemap(Transform parentTransform, string tilemapName, int sortOrder)
        {
            GameObject tilemapObject = new GameObject(tilemapName);
            tilemapObject.transform.parent = parentTransform;

            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            TilemapRenderer tilemapRenderer = tilemapObject.AddComponent<TilemapRenderer>();
            tilemapRenderer.sortingOrder = sortOrder;

            return tilemap;
        }

        public bool DeleteTilemap(LayerType layer)
        {
            if (_tilemapDictionary.TryGetValue(layer, out Tilemap tilemapToDelete))
            {
                _tilemapDictionary.Remove(layer);
                DestroyImmediate(tilemapToDelete.gameObject);
                return true;
            }
            return false;
        }

        public void DrawTilemap(LayerType layer, Vector3Int position, TileBase tile)
        {
            if (_tilemapDictionary.TryGetValue(layer, out Tilemap tilemap))
            {
                tilemap.SetTile(position, tile);
            }
            else
            {
                Debug.LogWarning("해당 레이어의 타일맵을 찾을 수 없습니다.");
            }
        }

        public void ClearLayerTilemap(LayerType layer)
        {
            if (_tilemapDictionary.TryGetValue(layer, out Tilemap tilemap))
            {
                tilemap.ClearAllTiles();
            }
            else
            {
                Debug.LogWarning("해당 레이어의 타일맵을 찾을 수 없습니다.");
            }
        }

        public void ClearAllLayerTilemap()
        {
            foreach (var key in _tilemapDictionary.Keys)
            {
                ClearLayerTilemap(key);
            }
        }
    }
}
