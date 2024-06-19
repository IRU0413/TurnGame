using Scripts.Cores.Skills.Skill.SkillMain;
using Scripts.Enums;
using Scripts.Pattern;
using Scripts.SO;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Scripts.Manager
{
    public class SkillManager : MonoBehaviourSingleton<SkillManager>
    {
        protected override void Initialize()
        {
            base.Initialize();
            InitSkillSpawn();
            InitSearchRange();
        }

        /*public SkillInfo GetInfo(int id)
        {
            SkillSO _stageSo = Managers.Resource.DataLoad<SkillSO>($"SO/Skill/Skill_{id}");
            if (_stageSo == null)
            {
                Managers.DebugPrint.WarningPrint(this, "This number is a skill ID that does not exist.");
                return null;
            }

            return new SkillInfo(id, _stageSo);
        }*/

        public SkillSO GetSkillSO(int id)
        {
            SkillSO so = GameManager.Resource.Load<SkillSO>($"SO/Skill/Skill_{id}");
            if (so == null)
            {
                this.ErrorPrint("This number is a skill ID that does not exist.");
                return null;
            }

            return so;
        }

        #region 생성

        const string DEFAULT_SKILL_GAMEOBJECT_PATH = "Prefab/Skill/Skill_None_Default_0";
        private GameObject _skillDefaultGo;

        private void InitSkillSpawn()
        {
            _skillDefaultGo = Resources.Load<GameObject>($"{DEFAULT_SKILL_GAMEOBJECT_PATH}");
        }

        // 스폰
        public SkillMainCore Spawn(int spawnSkillID)
        {
            GameObject skillGo = Instantiate(_skillDefaultGo);
            SkillMainCore skill = skillGo.GetOrAddComponent<SkillMainCore>();

            // skill.InitOut(spawnSkillID);
            return skill;
        }

        // etc...

        #endregion

        #region 예상 범위

        private const string SEARCH_TILE_BASE_PATH = "Image/CustomTile/TilesCursors_25";
        private const string ACTIVE_TILE_BASE_PATH = "Image/CustomTile/TilesCursors_26";
        private const string MOUSE_TILE_BASE_PATH = "Image/CustomTile/TilesCursors_27";

        private GameObject _searchRangeGo;
        private Grid _searchRangeGrid;
        private Tilemap _searchRangeTilemap; // 탐색 가능 범위
        private Tilemap _activeRangeTilemap; // 적용 가능 범위
        [SerializeField] private Vector3 _tileAncherOffset = new Vector3(0.5f, 0.5f, 0.0f); // 타일 앵커

        [Header("DrawRange")]
        [SerializeField] private TileBase _baseTile;
        [SerializeField] private TileBase _activeTile;
        [SerializeField] private TileBase _mouseTile;

        [SerializeField] private bool _isSearchDrew;
        [SerializeField] private bool _isActiveDrew;

        [SerializeField] private Vector2 _basePoint;
        [SerializeField] private Vector2 _actionPoint;

        public Vector3 TilemapOffset => _tileAncherOffset;

        private void InitSearchRange()
        {
            _searchRangeGo = ExtraFunction.CreateChildGameObject(this.transform, "SearchRangeGroup");
            GameObject gridGo = ExtraFunction.CreateChildGameObject(_searchRangeGo.transform, "SearchRangeGrid");
            GameObject searchTilemapGo = ExtraFunction.CreateChildGameObject(gridGo.transform, "SearchRangeTilemap");
            GameObject activeTilemapGo = ExtraFunction.CreateChildGameObject(gridGo.transform, "ActiveRangeTilemap");

            _searchRangeGrid = gridGo.GetOrAddComponent<Grid>();
            _searchRangeTilemap = searchTilemapGo.GetOrAddComponent<Tilemap>();
            _activeRangeTilemap = activeTilemapGo.GetOrAddComponent<Tilemap>();
            TilemapRenderer sTileMR = searchTilemapGo.GetOrAddComponent<TilemapRenderer>();
            TilemapRenderer aTileMR = activeTilemapGo.GetOrAddComponent<TilemapRenderer>();

            _searchRangeTilemap.tileAnchor = _tileAncherOffset;
            _activeRangeTilemap.tileAnchor = _tileAncherOffset;

            sTileMR.sortingOrder = (int)LayoutOrder.SkillSearchTilemap;
            aTileMR.sortingOrder = (int)LayoutOrder.SkillActiveTilemap;

            _baseTile = Resources.Load<TileBase>(SEARCH_TILE_BASE_PATH);
            _activeTile = Resources.Load<TileBase>(ACTIVE_TILE_BASE_PATH);
            _mouseTile = Resources.Load<TileBase>(MOUSE_TILE_BASE_PATH);

            _isSearchDrew = false;
            _isActiveDrew = false;
        }

        public void DrawRange(DrawLayerType type, List<Vector2> tilesPos)
        {
            Tilemap applyTilemap = null;
            TileBase applyTileBase = null;

            // ClearRange(type);
            switch (type)
            {
                case DrawLayerType.None:
                    return;
                case DrawLayerType.Base:
                    _isSearchDrew = true;
                    applyTilemap = _searchRangeTilemap;
                    applyTileBase = _baseTile;
                    break;
                case DrawLayerType.Action:
                    _isActiveDrew = true;
                    applyTilemap = _activeRangeTilemap;
                    applyTileBase = _activeTile;
                    break;
            }

            DrawTilemap(applyTilemap, applyTileBase, tilesPos);
        }
        public void DrawRange(DrawLayerType type, List<Vector2> tilesPos, Vector2 point)
        {
            Tilemap applyTilemap = null;
            TileBase applyTileBase = null;
            Vector2 applyPoint = Vector2.zero;
            bool reDraw = false;

            // ClearRange(type);
            switch (type)
            {
                case DrawLayerType.None:
                    return;
                case DrawLayerType.Base:
                    applyTilemap = _searchRangeTilemap;
                    applyTileBase = _baseTile;
                    if (_basePoint != point || !_isSearchDrew)
                    {
                        _basePoint = point;
                        applyPoint = _basePoint;
                        reDraw = true;
                        ClearRange(type);
                    }
                    _isSearchDrew = true;
                    break;
                case DrawLayerType.Action:
                    applyTilemap = _activeRangeTilemap;
                    applyTileBase = _activeTile;
                    if (_actionPoint != point || !_isActiveDrew)
                    {
                        _actionPoint = point;
                        applyPoint = _actionPoint;
                        reDraw = true;
                        ClearRange(type);
                    }
                    _isActiveDrew = true;
                    break;
            }

            if (reDraw)
            {
                DrawTilemap(applyTilemap, applyTileBase, tilesPos, applyPoint);
            }
        }

        public void DrawTilemap(Tilemap applyTilemap, TileBase applyTileBase, List<Vector2> tilesPos)
        {
            foreach (var tilePos in tilesPos)
            {
                Vector3Int pos = new Vector3Int((int)tilePos.x, (int)tilePos.y, 0);
                applyTilemap.SetTile(pos, applyTileBase);
            }
        }
        public void DrawTilemap(Tilemap applyTilemap, TileBase applyTileBase, List<Vector2> tilesPos, Vector2 point)
        {
            foreach (var tilePos in tilesPos)
            {
                Vector3Int pos = new Vector3Int((int)(tilePos.x + point.x), (int)(tilePos.y + point.y), 0);

                if (tilePos == Vector2.zero)
                    applyTilemap.SetTile(pos, _mouseTile);
                else
                    applyTilemap.SetTile(pos, applyTileBase);
            }
        }
        public void ClearRange(DrawLayerType type)
        {
            switch (type)
            {
                case DrawLayerType.Base:
                    if (_isSearchDrew)
                    {
                        _searchRangeTilemap.ClearAllTiles();
                        _isSearchDrew = false;
                    }
                    break;
                case DrawLayerType.Action:
                    if (_isActiveDrew)
                    {
                        _activeRangeTilemap.ClearAllTiles();
                        _isActiveDrew = false;
                    }
                    break;
            }
        }

        // 마우스 위치를 가지고 타일 위치로 보정
        public Vector2 GetTilePointToPosition(Vector2 pos)
        {
            Vector2 mousePosition = pos;

            // 타일 셋에 맞겠끔 옵셋만큼 빼줌
            mousePosition -= (Vector2)_tileAncherOffset;

            // 반올림을 사용하여 정수 값으로 변환.
            mousePosition.x = Mathf.Round(mousePosition.x);
            mousePosition.y = Mathf.Round(mousePosition.y);

            return mousePosition;
        }

        // 타일 위치 탐색 범위 안에 묶어놓기
        public Vector2 GetBindTileLocationWithinRange(List<Vector2> rangePos, Vector2 centerTilePos)
        {
            // 제일 가까운 좌표를 찾기
            List<Vector2> tilesPos = rangePos;
            Vector2 resultPos = new();

            float resultDistance = 9999.0f;
            for (int i = 0; i < tilesPos.Count; i++)
            {
                float distance = (tilesPos[i] - centerTilePos).magnitude;
                if (resultDistance > distance)
                {
                    resultPos = tilesPos[i];
                    resultDistance = distance;
                }
            }

            return resultPos;
        }

        #endregion
    }
}