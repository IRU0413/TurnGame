using Scripts.Enums;
using Scripts.SO;
using Scripts.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Cores.Item
{
    public abstract class ItemCore : Core
    {
        // 아이템 상태
        private bool _isSettingData = false;
        private bool _isSettingVisual = false;
        private bool _isSettingFieldPosition = false;

        [SerializeField] protected int _id = -1;

        [SerializeField] protected string _itemName; // 이름
        [SerializeField] protected ItemType _itemType; // 아이템 타입
        [SerializeField] protected ItemGradeType _itemGradeType; // 아이템 등급
        [SerializeField] protected Sprite[] _itemSprite; // 아이템 이미지들
        [SerializeField] protected float _itemSpriteRotation; // 아이템 스프라이트 회전 보정

        private Sprite _iconSprite; // 아이콘 이지미(아이템 스프라이트의 갯수가 1초과일 때 생성됨)

        private Vector2Int _fieldPosition; // 필드 위치

        // Visual
        private GameObject _visualGo;
        private SortingGroup _sortG;
        private SpriteRenderer _iconRenderer;

        // 소리 관련 해서 나중에 작업
        private AudioClip _InSound; // 인벤에 넣었을 때(각 아이템의 소리가 다를 수 있으니)
        private AudioClip _outSound; // 인벤에서 나왔을 때(각 아이템의 소리가 다를 수 있으니)

        public Sprite[] ItemSprite => _itemSprite;
        public GameObject VisualGO => _visualGo;

        #region Setting
        protected override void Initialized()
        {
            _isSettingData = SettingData();
            _isSettingVisual = SettingVisual();
            _isSettingFieldPosition = SettingFieldPosition();

            _isSettingFieldPosition = true;

            base.Initialized();

            bool isUsable = ((_isSettingData && _isSettingVisual
            && _isSettingFieldPosition) || GameManager.GameMode == GameModeType.Test);

            this.gameObject.SetActive(isUsable);
        }
        protected virtual bool SettingData()
        {
            return true;
        }
        protected virtual bool SettingFieldPosition()
        {
            // transform.position = GetChangeWorldPosition(_fieldPosition);
            // 나중에 CoreManager or UnitManager or ItemManager 관련해서
            // 만들어서 아이템 관리 될 때 수정
            return false;
        }
        protected virtual bool SettingVisual()
        {
            // 필요 아이템 비주얼 가지고 오기
            var visual = transform.Find("Item_Visual");
            if (visual == null)
            {
                var visualPrefab = GameManager.Resource.Load<GameObject>("Prefab/Item/Item_Visual");
                if (visualPrefab == null)
                {
                    Debug.LogError("현재 Item_Visual.Prefab 이 존재하지 않습니다.");
                    return false;
                }

                visual = GameObject.Instantiate(visualPrefab).transform;
            }

            // 레이어 세팅
            _visualGo = visual.gameObject;
            _sortG = visual.GetOrAddComponent<SortingGroup>();
            _sortG.sortingOrder = (int)LayerType.Item;

            // 해당 옵젝의 하위로 넣어주기
            visual.transform.parent = this.transform;
            visual.transform.localPosition = Vector2.zero;

            // 아이콘 세팅
            var findIconObj = visual.CreateChildGameObject("Icon");
            _iconRenderer = findIconObj.GetOrAddComponent<SpriteRenderer>();
            _iconSprite = GetIconSprite(_itemSprite);
            _iconRenderer.sprite = _iconSprite;

            return true;
        }

        #endregion

        // 인게임 생성 시 실행될 녀석
        public void Spawn(int id, Vector2Int fieldPosition)
        {
            if (_isInitialized)
                return;
            this.gameObject.SetActive(false);

            _id = id;
            _fieldPosition = fieldPosition;

            Initialized();
        }

        private void OnEnable()
        {
            Spawn(_id, Vector2Int.right);
        }

        // 필드 위치 월드 워치로 변경
        private Vector3 GetChangeWorldPosition(Vector2Int pos)
        {
            return new(pos.x, pos.y, 0.0f);
        }

        // SO
        private ItemType GetItemTypeByID(int id)
        {
            var resultType = ItemType.None;

            int count = id / 10000;

            switch (count)
            {
                case 1:
                    resultType = ItemType.Gear;
                    break;
                case 2:
                    resultType = ItemType.Potion;
                    break;
            }

            return resultType;
        }
        protected T GetSO<T>() where T : ItemSO
        {
            var type = GetItemTypeByID(_id);
            string typeStr = type.ToString();

            var so = GameManager.Resource.Load<T>($"SO/Item/{typeStr}/{typeStr}_Item_{_id}");

            return so;
        }

        private Sprite GetIconSprite(Sprite[] itemSprites)
        {
            Texture2D spriteParentTexture2D = itemSprites[0].texture;
            int pixelsPerUnit = 32;

            float preCount = (pixelsPerUnit / 2.0f) / spriteParentTexture2D.height;

            return Sprite.Create(spriteParentTexture2D, new Rect(0, 0, spriteParentTexture2D.width, spriteParentTexture2D.height), new Vector2(0.5f, preCount), 32);

        }
    }
}
