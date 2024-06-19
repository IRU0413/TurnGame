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

        // GridController 추가
        public void AddGridController(GridController gridController)
        {
            if (!_gridControllers.Contains(gridController))
            {
                _gridControllers.Add(gridController);
            }
        }

        // GridController 제거
        public void RemoveGridController(GridController gridController)
        {
            if (_gridControllers.Contains(gridController))
            {
                _gridControllers.Remove(gridController);
            }
        }

        // 포함된 그리드컨트롤러
        public bool ContainsGridController(GridController gridController)
        {
            return _gridControllers.Contains(gridController);
        }

        // 지정 레이어
        public void SetActiveLayer(GridController gridController, LayerType layer, bool isActive)
        {
            if (gridController == null)
                return;
            gridController.SetActiveLayerTypeTilemap(layer, isActive);
        }

        // 모든 레이어
        public void SetAllLayersActive(GridController gridController, bool isActive)
        {
            if (gridController == null)
                return;
            gridController.SetActiveAllTilemap(isActive);
        }

        // 모든 그리드, 타일맵
        public void SetAllGridsActive(bool isActive)
        {
            foreach (var gridController in _gridControllers)
            {
                // 그리드 컨트롤러 활성화 or 비활성화
                gridController.SetActiveGrid(isActive);
            }
        }
    }
}
