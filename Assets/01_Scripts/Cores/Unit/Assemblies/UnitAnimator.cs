using Scripts.Enums;
using Scripts.Manager;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitAnimator : UnitAssembly
    {
        const string FIND_VISUAL_OBJECT_NAME = "UnitPrefab";

        const string PARAMETER_ATTACK_NAME = "AttackType";
        const string PARAMETER_STATE_NAME = "StateType";

        const float ATTACK_TYPE_END_NUM = (int)UnitAttackType.BowAttack - 1;


        private GameObject _visualObject;
        private Animator _animator = null;
        private SortingGroup _layerGroup = null;

        private int _attackHash;
        private int _stateHash;

        private UnitStateType _playStateType;
        private UnitAttackType _playAttackType;

        protected override void OnBeforeInitialization()
        {
            base.OnBeforeInitialization();
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            SetVisualObject();

            // 레이어 정렬
            _layerGroup = _visualObject.GetOrAddComponent<SortingGroup>();
            _layerGroup.sortingOrder = (int)LayerType.Unit;

            // 애니메이터 추가 및 가져오기
            _animator = _visualObject.GetOrAddComponent<Animator>();
            if (_animator.runtimeAnimatorController == null)
                _animator.runtimeAnimatorController = GameManager.Resource.Load<AnimatorController>("Animation/Controller/ActionUnitController");

            // 애니메이터 해싱
            foreach (var parameter in _animator.parameters)
            {
                string pName = parameter.name;
                int hash = Animator.StringToHash(pName);

                if (PARAMETER_ATTACK_NAME.Equals(pName))
                    _attackHash = hash;
                else if (PARAMETER_STATE_NAME.Equals(pName))
                    _stateHash = hash;
            }
        }

        private void SetVisualObject()
        {
            _visualObject = Unit.Tr.Find(FIND_VISUAL_OBJECT_NAME)?.gameObject;
            if (_visualObject == null)
            {
                GameObject loadUnitPrefab = GameManager.Resource.Load<GameObject>("Prefab/Unit/Unit_Visual");
                GameObject spawnPrefab = GameObject.Instantiate(loadUnitPrefab);
                spawnPrefab.transform.parent = null;
                spawnPrefab.transform.parent = Unit.Tr;
                _visualObject = spawnPrefab;
            }
        }

        public void PlayAnimation(UnitStateType state, UnitAttackType attack = UnitAttackType.None)
        {
            if (state == UnitStateType.Attack && attack == UnitAttackType.None)
                attack = UnitAttackType.NormalAttack;

            _playAttackType = attack;
            _playStateType = state;

            _animator.SetFloat(_attackHash, ((int)_playAttackType - 1) / ATTACK_TYPE_END_NUM);
            _animator.SetInteger(_stateHash, (int)_playStateType);
        }

        public void FlipX(bool isFlipX)
        {
            if (isFlipX)
                _visualObject.transform.eulerAngles = Vector3.right * 188;
            else
                _visualObject.transform.eulerAngles = Vector3.zero;
        }
        public void FlipY(bool isFlipY)
        {
            if (isFlipY)
                _visualObject.transform.eulerAngles = Vector3.up * 188;
            else
                _visualObject.transform.eulerAngles = Vector3.zero;
        }
    }
}