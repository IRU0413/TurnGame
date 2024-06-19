using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyEvent : UnityEvent<PlayerObj.PlayerState>
{

}

[ExecuteInEditMode]
public class PlayerObj : MonoBehaviour
{
    public SPUM_Prefabs _prefabs; // 캐릭터 애니메이션 변경을 위해서 필요
    public float _charMS; // 캐릭터 이동 속도
    public enum PlayerState
    {
        idle,
        run,
        attack,
        death,
    }
    // 현재 상태.
    private PlayerState _currentState;
    // 외부에서 상태 변경하기 위한 함수(해당 값에 맞는 이벤트가 있다면 실행 시키고 상태 변경)
    public PlayerState CurrentState
    {
        get => _currentState;
        set
        {
            _stateChanged.Invoke(value);
            _currentState = value;
        }
    }

    private MyEvent _stateChanged = new MyEvent();

    public Vector3 _goalPos;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        if (_prefabs == null)
        {
            _prefabs = transform.GetChild(0).GetComponent<SPUM_Prefabs>();
        }

        _stateChanged.AddListener(PlayStateAnimation);


    }
    private void PlayStateAnimation(PlayerState state)
    {
        _prefabs.PlayAnimation(state.ToString());
    }
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.localPosition.y * 0.01f);
        switch (_currentState)
        {
            case PlayerState.idle:
                break;

            case PlayerState.run:
                DoMove();
                break;
        }


    }

    void DoMove()
    {
        // 이동 계산
        Vector3 _dirVec = _goalPos - transform.position;
        Vector3 _disVec = (Vector2)_goalPos - (Vector2)transform.position;
        // 보정
        if (_disVec.sqrMagnitude < 0.1f)
        {
            _currentState = PlayerState.idle;
            PlayStateAnimation(_currentState);
            return;
        }
        Vector3 _dirMVec = _dirVec.normalized;
        transform.position += (_dirMVec * _charMS * Time.deltaTime);

        // 플립
        if (_dirMVec.x > 0) _prefabs.transform.localScale = new Vector3(-1, 1, 1);
        else if (_dirMVec.x < 0) _prefabs.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SetMovePos(Vector2 pos)
    {
        _goalPos = pos;
        _currentState = PlayerState.run;
        PlayStateAnimation(_currentState);
    }
}
