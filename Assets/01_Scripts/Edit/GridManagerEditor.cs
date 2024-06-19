using Scripts.Enums;
using Scripts.Manager;
using Scripts.Module;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scripts.Edit
{
    [CustomEditor(typeof(GridManager))]
    [CanEditMultipleObjects]
    public class GridManagerEditor : Editor
    {
        private LayerType _selectedLayer;
        private LayerType _globalLayer;
        private int _newSelectGridIndex = 0;
        private int _selectedGridIndex = -1;
        private bool _gridActiveState;
        private bool _layerActiveState;
        private List<string> typeNames = new List<string>();
        GridController _selectedGridController = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridManager gridManager = (GridManager)target;

            // 그리드 선택 드롭다운
            List<string> gridNames = new List<string>();
            foreach (var gridController in gridManager.GridCtrls)
            {
                gridNames.Add(gridController.ToString());
            }

            // 그리드 선택 드롭 다운
            if (gridNames.Count > 0)
            {
                _newSelectGridIndex = EditorGUILayout.Popup("Select Grid", _newSelectGridIndex, gridNames.ToArray());
                if (_newSelectGridIndex != _selectedGridIndex)
                {
                    _selectedGridIndex = _newSelectGridIndex;
                    _selectedGridController = gridManager.GridCtrls[_selectedGridIndex];
                    var types = _selectedGridController.GetLayerTypes();
                    if (types.Count > 0)
                    {
                        var settingType = types[0];
                        _selectedLayer = settingType;
                        _layerActiveState = _selectedGridController.ContainsLayerType(_selectedLayer);

                        typeNames.Clear();
                        foreach (var type in types)
                            typeNames.Add(type.ToString());
                    }
                    else
                        return;
                }
            }
            else
                return;
            if (GUILayout.Button($"Set Active {_selectedGridController.name} \n(Now {_gridActiveState} > Clicked {!_gridActiveState})"))
            {
                _gridActiveState = !_gridActiveState;
                _selectedGridController.gameObject.SetActive(_gridActiveState);
            }
            if (GUILayout.Button("Show All Grids"))
                SetAllActiveGrid(gridManager, true);
            if (GUILayout.Button("Hide All Grids"))
                SetAllActiveGrid(gridManager, false);

            GUILayout.Space(30); // 간격 추가



            // 레이어 선택 드롭다운
            if (typeNames.Count > 0)
            {
                int selectedTypeIndex = typeNames.IndexOf(_selectedLayer.ToString());
                selectedTypeIndex = EditorGUILayout.Popup("Select Layer", selectedTypeIndex, typeNames.ToArray());
                var nowType = (LayerType)System.Enum.Parse(typeof(LayerType), typeNames[selectedTypeIndex]);

                if (nowType != _selectedLayer)
                {
                    _selectedLayer = nowType;
                    if (_selectedGridController.ContainsLayerType(_selectedLayer))
                    {
                        _layerActiveState = _selectedGridController.ContainsLayerType(_selectedLayer);
                    }
                    else
                    {
                        Debug.LogWarning($"{_selectedLayer} 레이어를 찾을 수 없습니다.");
                        _layerActiveState = false;
                    }
                }
            }
            else
                return;
            if (GUILayout.Button($"Set Active Choose Layer \n(Now {_layerActiveState} > Clicked {!_layerActiveState})"))
            {
                _layerActiveState = !_layerActiveState;
                gridManager.SetActiveLayer(_selectedGridController, _selectedLayer, _layerActiveState);
            }
            if (GUILayout.Button($"Show All Layer In Selected Grid \n({_selectedGridController.name})"))
                SetActiveLayerInSelectedGrid(gridManager, true);
            if (GUILayout.Button($"Hide All Layer In Selected Grid \n({_selectedGridController.name})"))
                SetActiveLayerInSelectedGrid(gridManager, false);

            GUILayout.Space(30); // 간격 추가



            _globalLayer = (LayerType)EditorGUILayout.EnumPopup("Select Layer", _globalLayer);
            // Global Layer
            if (GUILayout.Button($"Show {_globalLayer}"))
            {
                int start = (int)LayerType.Background;
                int end = (int)LayerType.UIEffect;
                foreach (var ctrl in gridManager.GridCtrls)
                {
                    for (int i = start; i < end; i++)
                    {
                        LayerType type = (LayerType)i;
                        bool isActive = (type == _globalLayer);

                        ctrl.SetActiveLayerTypeTilemap(type, isActive);
                    }
                }
            }
            if (GUILayout.Button($"Show ALL"))
            {
                int start = (int)LayerType.Background;
                int end = (int)LayerType.UIEffect;
                foreach (var ctrl in gridManager.GridCtrls)
                {
                    for (int i = start; i < end; i++)
                    {
                        LayerType type = (LayerType)i;
                        ctrl.SetActiveLayerTypeTilemap(type, true);
                    }
                }
            }

        }

        private void SetActiveLayerInSelectedGrid(GridManager gridManager, bool isActive)
        {
            _layerActiveState = isActive;
            gridManager.SetAllLayersActive(_selectedGridController, _layerActiveState);
        }

        private void SetAllActiveGrid(GridManager gridManager, bool isActive)
        {
            _gridActiveState = isActive;
            gridManager.SetAllGridsActive(_gridActiveState);
        }
    }
}
