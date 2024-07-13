using UnityEngine;

namespace Scripts.Module
{
    public class MoveModule
    {
        private bool _isInit = false;

        private Transform _tr;

        private bool _isMoving;

        private Vector2 _moveDir;

        private Vector2 _startPos;
        private Vector2 _goalPos;

        private Vector2 _currentPos;


        public bool IsInit => _isInit;
        public bool IsMoving => _isMoving;
        public Vector2 MoveDir => _moveDir;
        public Vector2 StartPos => _startPos;

        // 옵셋, 배율
        public void Init(Transform tr, Vector2 startPos)
        {
            _tr = tr;

            _isMoving = false;


            _moveDir = Vector2.zero;

            _startPos = startPos;
            _goalPos = startPos;

            _currentPos = _startPos;
            _tr.localPosition = _currentPos;

            if (_tr == null)
            {
                Debug.LogError("It Error MoveModule.Init() Fuction!");
                return;
            }

            _isInit = true;
        }

        public void GoalPosRunSetting(Vector2 goalPos)
        {
            _startPos = _tr.localPosition;
            _goalPos = goalPos;

            _currentPos = _startPos;
            _moveDir = (_goalPos - _currentPos).normalized;

            _isMoving = true;
        }

        public void DirectionRunSetting(Vector2 dir)
        {
            _startPos = _tr.localPosition;
            _goalPos = new Vector2(_tr.position.x, _tr.position.y) + dir;

            _currentPos = _startPos;
            _moveDir = dir.normalized;

            _isMoving = true;
        }

        public void Move(float moveSpeed)
        {
            if (!_isMoving)
                return;

            _tr.Translate(_moveDir * Time.deltaTime * moveSpeed);
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