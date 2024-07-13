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
        // ������ ����
        private bool _isSettingData = false;
        private bool _isSettingVisual = false;
        private bool _isSettingFieldPosition = false;

        [SerializeField] protected int _id = -1;

        [SerializeField] protected string _itemName; // �̸�
        [SerializeField] protected ItemType _itemType; // ������ Ÿ��
        [SerializeField] protected ItemGradeType _itemGradeType; // ������ ���
        [SerializeField] protected Sprite[] _itemSprite; // ������ �̹�����
        [SerializeField] protected float _itemSpriteRotation; // ������ ��������Ʈ ȸ�� ����

        private Sprite _iconSprite; // ������ ������(������ ��������Ʈ�� ������ 1�ʰ��� �� ������)

        private Vector2Int _fieldPosition; // �ʵ� ��ġ

        // Visual
        private GameObject _visualGo;
        private SortingGroup _sortG;
        private SpriteRenderer _iconRenderer;

        // �Ҹ� ���� �ؼ� ���߿� �۾�
        private AudioClip _InSound; // �κ��� �־��� ��(�� �������� �Ҹ��� �ٸ� �� ������)
        private AudioClip _outSound; // �κ����� ������ ��(�� �������� �Ҹ��� �ٸ� �� ������)

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
            // ���߿� CoreManager or UnitManager or ItemManager �����ؼ�
            // ���� ������ ���� �� �� ����
            return false;
        }
        protected virtual bool SettingVisual()
        {
            // �ʿ� ������ ���־� ������ ����
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

            // ���̾� ����
            _visualGo = visual.gameObject;
            _sortG = visual.GetOrAddComponent<SortingGroup>();
            _sortG.sortingOrder = (int)LayerType.Item;

            // �ش� ������ ������ �־��ֱ�
            visual.transform.parent = this.transform;
            visual.transform.localPosition = Vector2.zero;

            // ������ ����
            var findIconObj = visual.CreateChildGameObject("Icon");
            _iconRenderer = findIconObj.GetOrAddComponent<SpriteRenderer>();
            _iconSprite = GetIconSprite(_itemSprite);
            _iconRenderer.sprite = _iconSprite;

            return true;
        }

        #endregion

        // �ΰ��� ���� �� ����� �༮
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

        // �ʵ� ��ġ ���� ��ġ�� ����
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
