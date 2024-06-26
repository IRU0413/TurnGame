namespace Scripts.SO
{
    using Scripts.Enums;
    using UnityEngine;

    [CreateAssetMenu(fileName = "ActionUnitDataSO", menuName = "ScriptableObjects/Unit/ActionUnit/AllData")]
    public class ActionUnitDataSO : ScriptableObject
    {
        [SerializeField][HideInInspector] private int _id;

        [SerializeField] private string _name;
        [SerializeField] private JobType _jobType;

        [SerializeField] private ActionUnitStatusSO _statusSO;
        [SerializeField] private UnitSoundSO _soundSO;

        public int ID => _id;

        public string Name => _name;
        public JobType JobType => _jobType;

        public ActionUnitStatusSO Status => _statusSO;
        public UnitSoundSO Sound => _soundSO;


        public void SaveData(int id, string name, JobType job, ActionUnitStatusSO statusSO, UnitSoundSO soundSO)
        {
            _id = id;

            _name = name;
            _jobType = job;

            _statusSO = statusSO;
            _soundSO = soundSO;

        }
    }
}