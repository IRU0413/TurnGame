using Scripts.Cores.Unit;
using Scripts.Module;
using Unity.VisualScripting;
using UnityEngine;

namespace Scripts.Cmp
{
    public class MoveCmp : UnitAssembly
    {
        [SerializeField]
        private Vector2 _inputVector;

        private MoveModule _moveModule = new();

        private Transform _thisTr;

        public float _moveSpeed = 1.0f;

        public bool _stepMove = false;


        // Ready is called before the first frame update
        void Start()
        {
            if (_moveModule == null)
                _moveModule = new MoveModule();

            _thisTr = this.GetOrAddComponent<Transform>();

            _moveModule.Init(_thisTr, _thisTr.position, _moveSpeed);
        }

        // Update is called once per frame
        void Update()
        {
            _inputVector.x = Input.GetAxisRaw("Horizontal");
            _inputVector.y = Input.GetAxisRaw("Vertical");

            if (_stepMove)
            {
                if (_inputVector != Vector2.zero
                    && !_moveModule.IsMoving)
                {
                    _moveModule.RunSetting((Vector2)_thisTr.position + _inputVector);
                }
                _moveModule.Move();
            }
            else
            {
                if (_inputVector != Vector2.zero)
                {
                    _moveModule.RunSetting((Vector2)_thisTr.position + _inputVector);
                    _moveModule.Move();
                }
            }

        }
    }
}