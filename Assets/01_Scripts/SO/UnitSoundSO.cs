namespace Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "UnitSoundSO", menuName = "ScriptableObjects/Unit/Sound")]
    public class UnitSoundSO : ScriptableObject
    {
        [SerializeField][HideInInspector] private int _id;
        [SerializeField] private AudioClip _unitIdle;
        [SerializeField] private AudioClip _unitHit;
        [SerializeField] private AudioClip _unitCC;
        [SerializeField] private AudioClip _unitDie;

        public void SaveData(int id, AudioClip idle, AudioClip hit, AudioClip cc, AudioClip die)
        {
            _id = id;

            _unitIdle = idle;
            _unitHit = hit;
            _unitCC = cc;
            _unitDie = die;
        }
    }
}