using Scripts.Enums;
using Scripts.Manager;
using Scripts.Manager.ManagerGroup;
using Scripts.Pattern;
using UnityEngine;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField] private GameModeType _gameMode = GameModeType.None;

    // �⺻ ��ɵ�
    private InputManager _input = new InputManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private ResourceManager _resource = new ResourceManager();
    private DataManager _dataManager = new DataManager();

    // ȯ�� ����, �÷��̾� ���� �����͵�
    private PlayerInfor _playerInfo = new PlayerInfor();
    private SettingInfor _settingInfo = new SettingInfor();


    public static GameModeType GameMode { get { return Instance._gameMode; } }
    // ���
    public static InputManager Input { get { return Instance._input; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static DataManager Data { get { return Instance._dataManager; } }

    // ����
    public static PlayerInfor PlayerInfo { get { return Instance._playerInfo; } }
    public static SettingInfor SettingInfo { get { return Instance._settingInfo; } }


    protected override void Initialize()
    {
        base.Initialize();
        _playerInfo.Init();
        _settingInfo.Init();
    }

    private void Update()
    {
        Input.OnUpdate();
    }

    private void OnDisable()
    {
        _playerInfo.DataSave();
        _settingInfo.DataSave();
    }
}