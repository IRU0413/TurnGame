namespace Scripts.SO
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "EventRoomData_Default", menuName = "ScriptableObjects/EventRoom/Data")]
    public class EventRoomDataSO : RoomDataSO
    {
        [Header("> 상점 관련(상인)")]
        [SerializeField][Range(0, 10)] private int _merchantEvCreateConstantCount;
        [SerializeField][Range(0, 100)] private float _merchantEvCreatePercent = 34;
        [SerializeField][Range(0, 100)] private float _merchantEvUpgradePercent = 10;

        [Header("> 스킬 관련(엘프)")]
        [SerializeField][Range(0, 10)] private int _skillEvCreateConstantCount;
        [SerializeField][Range(0, 100)] private float _skillEvCreatePercent = 33;
        [SerializeField][Range(0, 100)] private float _skillEvUpgradePercent = 10;

        [Header("> 무기 관련(드워프)")]
        [SerializeField][Range(0, 10)] private int _weaponEvCreateConstantCount;
        [SerializeField][Range(0, 100)] private float _weaponEvCreatePercent = 33;
        [SerializeField][Range(0, 100)] private float _weaponEvUpgradePercent = 10;

        public int MerchantConstantCount => _merchantEvCreateConstantCount;
        public float MerchantCreatePercent => _merchantEvCreatePercent;
        public float MerchantEvUpgradePercent => _merchantEvUpgradePercent;
        public int SkillConstantCount => _skillEvCreateConstantCount;
        public float SkillCreatePercent => _skillEvCreatePercent;
        public float SkillEvUpgradePercent => _skillEvUpgradePercent;
        public int WeaponConstantCount => _weaponEvCreateConstantCount;
        public float WeaponCreatePercent => _weaponEvCreatePercent;
        public float WeaponEvUpgradePercent => _weaponEvUpgradePercent;
    }
}