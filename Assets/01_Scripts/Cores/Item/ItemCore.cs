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

        [SerializeField] private string _itemName; // �̸�
        [SerializeField] private ItemType _itemType; // ������ Ÿ��
        private Vector2Int _fieldPosition; // �ʵ� ��ġ

        [SerializeField] private Sprite _icon;

        // Visual
        private GameObject _visualGo;
        private SortingGroup _sortG;
        private SpriteRenderer _iconRenderer;

        // �Ҹ� ���� �ؼ� ���߿� �۾�
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
            // ���߿� CoreManager or UnitManager or ItemManager �����ؼ�
            // ���� ������ ���� �� �� ����
        }
        protected virtual void SettingVisual()
        {
            var visual = transform.Find("Item_Visual");
            if (visual == null)
            {
                var visualPrefab = GameManager.Resource.Load<GameObject>("Prefab/Item/Item_Visual");
                if (visualPrefab == null)
                {
                    Debug.LogError("���� Item_Visual.Prefab �� �������� �ʽ��ϴ�.");
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

        // ����
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

        // �ݱ�(ȹ��)
        public virtual void InInventroy()
        {
            _isInInventory = true;
            _visualGo.SetActive(!_isInInventory);
        }
        // ������(������)
        public virtual void OutInventory(Vector2Int playerPosition)
        {
            // �÷��� ���������� �ڽ� ������ �������� ����. �̰͵� SettingFieldPosition �����ؼ� ����
            _isInInventory = false;
            _visualGo.SetActive(!_isInInventory);
        }
    }
}
