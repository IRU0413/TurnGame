using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SPUM_Prefabs : MonoBehaviour
{
    public float _version; // 버전
    public SPUM_SpriteList _spriteOBj; // 스프라이트 옵젝(스프라이트 덩어리 찾는거임.)
    public bool EditChk; // 에디터 확인(다른쪽에서 확인)
    public string _code; // 코드(프리팹의 이름격되는 아이디)
    public Animator _anim; // 애니메터
    public bool _horse;
    // 말 여부
    public bool isRideHorse
    {
        get => _horse;
        set
        {
            _horse = value;
            UnitTypeChanged?.Invoke();
        }
    }
    // 말 저장 경로
    public string _horseString;

    // 유니티 이벤트(유닛 타입 변경)
    public UnityEvent UnitTypeChanged = new UnityEvent();
    private AnimationClip[] _animationClips;
    public AnimationClip[] AnimationClips => _animationClips;
    private Dictionary<string, int> _nameToHashPair = new Dictionary<string, int>();
    private void InitAnimPair()
    {
        _nameToHashPair.Clear();
        _animationClips = _anim.runtimeAnimatorController.animationClips;
        foreach (var clip in _animationClips)
        {
            int hash = Animator.StringToHash(clip.name);
            _nameToHashPair.Add(clip.name, hash);
        }
    }
    private void Awake()
    {
        InitAnimPair();
    }
    private void Start()
    {
        UnitTypeChanged.AddListener(InitAnimPair);
    }
    // 이름으로 애니메이션 실행
    public void PlayAnimation(string name)
    {

        Debug.Log(name);

        foreach (var animationName in _nameToHashPair)
        {
            if (animationName.Key.ToLower().Contains(name.ToLower()))
            {
                _anim.Play(animationName.Value, 0);
                break;
            }
        }
    }
}
