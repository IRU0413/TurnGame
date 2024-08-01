namespace Scripts.SO
{
    using Scripts.Enums;
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitDataSO", menuName = "ScriptableObjects/Unit/AllData")]
    public class UnitDataSO : ScriptableObject
    {
        public int Id;

        public string Name;
        public JobType JobType;

        public UnitAbilitySO UnitAbilitySO;
        public UnitSoundSO UnitSoundSO;
    }
}