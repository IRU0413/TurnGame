using Scripts.Enums;
using Scripts.Module;
using Scripts.Pattern;
using System.Collections.Generic;

namespace Scripts.Manager
{
    public class GridManager : MonoBehaviourSingleton<GridManager>
    {
        private List<GridController> _gridControllers = new List<GridController>();
        public List<GridController> GridCtrls => _gridControllers;

        protected override void Initialize()
        {
            base.Initialize();
        }

        // GridController �߰�
        public void AddGridController(GridController gridController)
        {
            if (!_gridControllers.Contains(gridController))
            {
                _gridControllers.Add(gridController);
            }
        }

        // GridController ����
        public void RemoveGridController(GridController gridController)
        {
            if (_gridControllers.Contains(gridController))
            {
                _gridControllers.Remove(gridController);
            }
        }

        // ���Ե� �׸�����Ʈ�ѷ�
        public bool ContainsGridController(GridController gridController)
        {
            return _gridControllers.Contains(gridController);
        }

        // ���� ���̾�
        public void SetActiveLayer(GridController gridController, LayerType layer, bool isActive)
        {
            if (gridController == null)
                return;
            gridController.SetActiveLayerTypeTilemap(layer, isActive);
        }

        // ��� ���̾�
        public void SetAllLayersActive(GridController gridController, bool isActive)
        {
            if (gridController == null)
                return;
            gridController.SetActiveAllTilemap(isActive);
        }

        // ��� �׸���, Ÿ�ϸ�
        public void SetAllGridsActive(bool isActive)
        {
            foreach (var gridController in _gridControllers)
            {
                // �׸��� ��Ʈ�ѷ� Ȱ��ȭ or ��Ȱ��ȭ
                gridController.SetActiveGrid(isActive);
            }
        }
    }
}
