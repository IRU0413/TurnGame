using Scripts.Enums;
using System;
using UnityEngine;

namespace Scripts.Manager.ManagerGroup
{
    public class InputManager
    {
        private const float DOUBLE_CLICK_CHECK_TIME = 0.5f;

        // 액션
        public Action<KeyboardInputType> KeyAction = null;
        public Action<MouseInputType, Vector3> MouseAction = null;
        // 마우스
        private bool _isClicked = false;
        private float _currentClickedTIme = 0f;
        private MouseInputType _mouseInputedType;
        private Vector3 _mousePos;
        // 키보드
        private KeyboardInputType _keyInputedType;

        public Vector3 MousePos => _mousePos;
        public MouseInputType MouseInputedType => _mouseInputedType;
        public KeyboardInputType KeyboardInputedType => _keyInputedType;

        public void OnUpdate()
        {
            Mouse();
            KeyBoard();
        }

        private void KeyBoard()
        {
            if (KeyAction == null) return;

            KeyboardInputType nowInputType;
            if (Input.anyKeyDown && _keyInputedType == KeyboardInputType.None)
            {
                nowInputType = KeyboardInputType.Down;
            }
            else if (Input.anyKey && (_keyInputedType == KeyboardInputType.Down || _keyInputedType == KeyboardInputType.Preesed))
            {
                nowInputType = KeyboardInputType.Preesed;
            }
            else if (!Input.anyKey && _keyInputedType != KeyboardInputType.None && _keyInputedType != KeyboardInputType.Up)
            {
                nowInputType = KeyboardInputType.Up;
            }
            else
            {
                nowInputType = KeyboardInputType.None;
            }

            KeyAction.Invoke(nowInputType);
            _keyInputedType = nowInputType;
        }
        private void Mouse()
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane;
            _mousePos = Camera.main.ScreenToWorldPoint(mousePosition);

            if (_isClicked)
            {
                _currentClickedTIme += Time.deltaTime;
                if (DOUBLE_CLICK_CHECK_TIME <= _currentClickedTIme)
                {
                    _currentClickedTIme = 0;
                    _isClicked = false;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_mouseInputedType == MouseInputType.None)
                    _mouseInputedType = MouseInputType.Click;
                else
                    _mouseInputedType = MouseInputType.Preesed;
            }
            else if (_mouseInputedType == MouseInputType.Preesed)
            {
                if (_isClicked)
                {
                    _mouseInputedType = MouseInputType.DoubleClicked;
                    _isClicked = false;
                    return;
                }
                else
                {
                    _mouseInputedType = MouseInputType.Clicked;
                    _currentClickedTIme = 0.0f;
                    _isClicked = true;
                }
            }
            else
            {
                _mouseInputedType = MouseInputType.None;
            }

            if (MouseAction != null)
                MouseAction.Invoke(_mouseInputedType, _mousePos);
        }

        public override string ToString()
        {
            string result = $"MouseInputType => {_mouseInputedType} \n" +
                $"MousePos => {_mousePos} \n" +
                $"MousePos Int Casting => {(int)_mousePos.x}, {(int)_mousePos.y}, {(int)_mousePos.z} \n" +
                $"MousePos Round => {Mathf.Round(_mousePos.x)}, {Mathf.Round(_mousePos.y)}, {Mathf.Round(_mousePos.z)}";


            return result;
        }
    }
}

