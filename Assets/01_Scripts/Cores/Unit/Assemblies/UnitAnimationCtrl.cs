using Scripts.Enums;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

namespace Scripts.Cores.Unit.Assemblies
{
    [DisallowMultipleComponent]
    public class UnitAnimationCtrl : UnitAssembly
    {
        const string FIND_GAEM_OBJECT_NAME = "UnitPrefab";

        const string PARAMETER_ATTACK_NAME = "AttackType";
        const string PARAMETER_STATE_NAME = "StateType";

        const float ATTACK_TYPE_END_NUM = (int)UnitAttackType.BowAttack - 1;

        private Animator _animator = null;
        private SortingGroup _layerGroup = null;

        private int _attackHash;
        private int _stateHash;

        private UnitStateType _playStateType;
        private UnitAttackType _playAttackType;

        protected override void OnInitialized()
        {
            base.OnInitialized();

            GameObject findNameObj = Unit.Tr.Find(FIND_GAEM_OBJECT_NAME)?.gameObject;
            if (findNameObj == null)
            {
                GameObject loadUnitPrefab = GameManager.Resource.Load<GameObject>("Prefab/Unit/Unit_Visual");
                GameObject spawnUnitPrefab = GameObject.Instantiate(loadUnitPrefab);
                spawnUnitPrefab.transform.parent = Unit.Tr;
                findNameObj = spawnUnitPrefab;
            }

            // 레이어 정렬
            _layerGroup = findNameObj.GetOrAddComponent<SortingGroup>();
            _layerGroup.sortingOrder = (int)LayerType.Unit;

            // 애니메이터 추가 및 가져오기
            _animator = findNameObj.GetOrAddComponent<Animator>();
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

        public void PlayAnimation(UnitStateType state, UnitAttackType attack = UnitAttackType.None)
        {
            if (state == UnitStateType.Attack && attack == UnitAttackType.None)
                attack = UnitAttackType.NormalAttack;

            _playAttackType = attack;
            _playStateType = state;

            _animator.SetFloat(_attackHash, ((int)_playAttackType - 1) / ATTACK_TYPE_END_NUM);
            _animator.SetInteger(_stateHash, (int)_playStateType);
        }
    }
}