using UnityEngine;

namespace Scripts.Module
{
    public class MoveModule
    {
        private bool _isInit = false;

        private Transform _tr;

        private bool _isMoving;

        private float _moveSpeed;
        private Vector2 _moveDir;

        private Vector2 _startPos;
        private Vector2 _goalPos;

        private Vector2 _currentPos;


        public bool IsInit => _isInit;
        public bool IsMoving => _isMoving;
        public Vector2 MoveDir => _moveDir;
        public Vector2 StartPos => _startPos;

        // 옵셋, 배율
        public void Init(Transform tr, Vector2 startPos, float speed)
        {
            _tr = tr;

            _isMoving = false;

            _moveSpeed = speed;

            _moveDir = Vector2.zero;

            _startPos = startPos;
            _goalPos = startPos;

            _currentPos = _startPos;
            _tr.localPosition = _currentPos;

            if (_tr == null || _moveSpeed <= 0)
            {
                Debug.LogError("It Error MoveModule.Init() Fuction!");
                return;
            }

            _isInit = true;
        }

        public void RunSetting(Vector2 goalPos)
        {
            _startPos = _tr.localPosition;
            _goalPos = goalPos;

            _currentPos = _startPos;
            _moveDir = (_goalPos - _currentPos).normalized;

            _isMoving = true;
        }

        public void Move()
        {
            if (!_isMoving)
                return;

            _tr.Translate(_moveDir * Time.deltaTime * _moveSpeed);
            _currentPos = _tr.position;

            Vector2 distance = _goalPos - _currentPos;
            if (distance.normalized != _moveDir || distance.magnitude <= 0.05f)
            {
                _currentPos = _goalPos;
                _tr.position = _currentPos;
                _isMoving = false;
                return;
            }
        }
    }
}