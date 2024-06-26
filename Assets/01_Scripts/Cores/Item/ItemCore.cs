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
        [SerializeField] private bool _isInInventory = false;

        // ������ ����
        private bool _isSettingData = false;
        private bool _isSettingVisual = false;
        private bool _isSettingFieldPosition = false;

        [SerializeField] protected int _id = -1;

        [SerializeField] protected string _itemName; // �̸�
        [SerializeField] protected ItemType _itemType; // ������ Ÿ��
        [SerializeField] protected ItemGradeType _itemGradeType; // ������ ���
        private Vector2Int _fieldPosition; // �ʵ� ��ġ

        // Visual
        private Sprite _applyIcon;
        private GameObject _visualGo;
        private SortingGroup _sortG;
        private SpriteRenderer _iconRenderer;

        // �Ҹ� ���� �ؼ� ���߿� �۾�
        private AudioClip _InSound; // �κ��� �־��� ��(�� �������� �Ҹ��� �ٸ� �� ������)
        private AudioClip _outSound; // �κ����� ������ ��(�� �������� �Ҹ��� �ٸ� �� ������)

        public bool IsInInventory => _isInInventory;

        #region Setting
        protected override void Initialized()
        {
            _isSettingData = SettingData();
            _isSettingVisual = SettingVisual();
            _isSettingFieldPosition = SettingFieldPosition();

            base.Initialized();

            bool isUsable = ((_isSettingData && _isSettingVisual
            && _isSettingFieldPosition) || GameManager.GameType == GameType.Test);

            this.gameObject.SetActive(isUsable);
        }
        protected virtual bool SettingData()
        {
            return true;
        }
        protected virtual bool SettingFieldPosition()
        {
            transform.position = GetChangeWorldPosition(_fieldPosition);
            // ���߿� CoreManager or UnitManager or ItemManager �����ؼ�
            // ���� ������ ���� �� �� ����
            return false;
        }
        protected virtual bool SettingVisual()
        {
            var visual = transform.Find("Item_Visual");
            if (visual == null)
            {
                var visualPrefab = GameManager.Resource.Load<GameObject>("Prefab/Item/Item_Visual");
                if (visualPrefab == null)
                {
                    Debug.LogError("���� Item_Visual.Prefab �� �������� �ʽ��ϴ�.");
                    return false;
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

            _iconRenderer.sprite = _applyIcon;

            return true;
        }

        #endregion

        // �ΰ��� ���� �� ����� �༮
        public void Spawn(int id, Vector2Int fieldPosition)
        {
            this.gameObject.SetActive(false);

            _id = id;
            _fieldPosition = fieldPosition;

            Initialized();
        }
        private void Awake()
        {
            Spawn(10000, Vector2Int.right);
        }

        // �ʵ� ��ġ ���� ��ġ�� ����
        private Vector3 GetChangeWorldPosition(Vector2Int pos)
        {
            return new(pos.x, pos.y, 0.0f);
        }

        // SO
        private ItemType GetItemTypeByID(int id)
        {
            var resultType = ItemType.None;

            int count = 10000 / id;

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


        // �ֿ����� �� ����
        public virtual void InInventroy()
        {
            _isInInventory = true;
            _visualGo.SetActive(!_isInInventory);
        }
        // �������� �� ����
        public virtual void OutInventory()
        {
            _isInInventory = false;
            _visualGo.SetActive(!_isInInventory);
        }
    }
}
