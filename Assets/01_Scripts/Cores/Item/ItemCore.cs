using Scripts.Enums;
using Scripts.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Cores.Item
{
    public abstract class ItemCore : Core
    {
        [SerializeField] private bool _isTest = false;
        [SerializeField] private bool _isInInventory = false;

        [SerializeField] private int _id;

        [SerializeField] private string _itemName; // 이름
        [SerializeField] private ItemType _itemType; // 아이템 타입
        private Vector2Int _fieldPosition; // 필드 위치

        [SerializeField] private Sprite _icon;

        // Visual
        private GameObject _visualGo;
        private SortingGroup _sortG;
        private SpriteRenderer _iconRenderer;

        // 소리 관련 해서 나중에 작업
        private AudioClip _spawnSound;
        private AudioClip _InSound;
        private AudioClip _outSound;

        public bool IsInInventory => _isInInventory;

        protected override void Initialized()
        {
            SettingData();
            SettingVisual();
            SettingFieldPosition();

            base.Initialized();

            bool isActive = (_id > 0 || _isTest);
            this.gameObject.SetActive(isActive);
        }
        protected virtual void SettingData()
        {
            if (_id < 0)
            {
                _id = -1;
                _itemName = "NoName (NotSpawn)";
                return;
            }
        }
        protected virtual void SettingFieldPosition()
        {
            // 나중에 CoreManager or UnitManager or ItemManager 관련해서
            // 만들어서 아이템 관리 될 때 수정
        }
        protected virtual void SettingVisual()
        {
            var visual = transform.Find("Item_Visual");
            if (visual == null)
            {
                var visualPrefab = GameManager.Resource.Load<GameObject>("Prefab/Item/Item_Visual");
                if (visualPrefab == null)
                {
                    Debug.LogError("현재 Item_Visual.Prefab 이 존재하지 않습니다.");
                    return;
                }

                visual = GameObject.Instantiate(visualPrefab).transform;
            }

            this.gameObject.name = _itemName;

            _visualGo = visual.gameObject;
            _sortG = visual.GetOrAddComponent<SortingGroup>();
            _sortG.sortingOrder = (int)LayerType.Item;

            visual.transform.parent = this.transform;
            var findIconObj = visual.CreateChildGameObject("Icon");
            _iconRenderer = findIconObj.GetOrAddComponent<SpriteRenderer>();

            _iconRenderer.sprite = _icon;
        }

        // 생성
        public virtual void Spawn(int id, Vector2Int fieldPosition)
        {
            if (_isInitialized)
                return;
            _id = id;
            _fieldPosition = fieldPosition;

            Initialized();
        }

        private void Awake()
        {
            this.gameObject.SetActive(false);
            if (_isTest)
            {
                Spawn(-1, new Vector2Int(0, 0));
            }
        }

        // 줍기(획득)
        public virtual void InInventroy()
        {
            _isInInventory = true;
            _visualGo.SetActive(!_isInInventory);
        }
        // 꺼내기(버리기)
        public virtual void OutInventory(Vector2Int playerPosition)
        {
            // 플레이 포지션으로 자신 주위에 떨어지게 설정. 이것도 SettingFieldPosition 관련해서 수정
            _isInInventory = false;
            _visualGo.SetActive(!_isInInventory);
        }
    }
}
