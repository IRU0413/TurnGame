namespace Scripts.SO
{
    using Scripts.Enums;
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "Stage_DefaultNum_SO", menuName = "ScriptableObjects/Stage/Data")]
    public class StageSO : ScriptableObject
    {
        [Header("= 스테이지 정보 =")]
        [SerializeField][Range(0, 100)] private int _distanceBetweenSafeBoss;
        [SerializeField][Range(0, 100)] private int _addCreateRoomCount;

        [Header("= 방 생성 데이터 =")]
        [SerializeField] private RoomDataSO _eliteRoomData;
        [SerializeField] private EventRoomDataSO _eventRoomData;

        [Space(40)]
        [Header("아이템 등급 최대치")]
        [SerializeField] private ItemGradeType _itemLimitGrade;

        [Header("등급 별 등장 확률")]
        [SerializeField] private float[] _appearGradePercent = new float[5];

        [Space(40)]
        [Header("= 스폰될 수 있는 몬스터 =")]
        [SerializeField] private List<GameObject> _spawnableNormalMonster;
        [SerializeField] private List<GameObject> _spawnableEliteMonster;

        [Header("= 일반 방 몬스터 스폰 수 =")]
        [SerializeField][Range(1, 100)] private int _normalRoomMonsterMinCount = 1;
        [SerializeField][Range(1, 100)] private int _normalRoomMonsterMaxCount = 2;

        [Header("= 엘리트 방 몬스터 스폰 수 =")]
        [SerializeField][Range(1, 100)] private int _eliteRoomMonsterMinCount = 1;
        [SerializeField][Range(1, 100)] private int _eliteRoomMonsterMaxCount = 2;

        [Header("강화된 몬스터 등장 확룰")]
        [SerializeField][Range(1, 100)] private float _upgradeMonsterAppearPercent = 1;


        // StageData
        public int Distacne => _distanceBetweenSafeBoss;
        public int AddRoomCount => _addCreateRoomCount;

        public RoomDataSO EliteRoomData => _eliteRoomData;
        public EventRoomDataSO EventRoomData => _eventRoomData;

        public List<GameObject> SpawnableNormalMonster => _spawnableNormalMonster;
        public List<GameObject> SpawnableEliteMonster => _spawnableEliteMonster;
        public int NormalRoomMonsterMinCount => _normalRoomMonsterMinCount;
        public int NormalRoomMonsterMaxCount => _normalRoomMonsterMaxCount;
        public int EliteRoomMonsterMinCount => _eliteRoomMonsterMinCount;
        public int EliteRoomMonsterMaxCount => _eliteRoomMonsterMaxCount;
        public float UpgradeMonsterAppearPercent => _upgradeMonsterAppearPercent;
        public ItemGradeType ItemLimitGrade => _itemLimitGrade;
        public float[] AppearGradePercent => _appearGradePercent;

        #region Util
        // normal, elite, event
        /*public List<float> AppearRoomPercentCatLien()
        {
            List<float> catLine = new List<float>();
            float total = 0;

            total += Mathf.Round(NR_Percent * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(ELR_Percent * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(EVR_Percent * 100) / 100;
            catLine.Add(total);

            return catLine;
        }
        // merchant, skill, weapon
        public List<float> AppearEventRoomPercentCatLien()
        {
            List<float> catLine = new List<float>();
            float total = 0;

            total += Mathf.Round(EVR_M_Percent * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(EVR_S_Percent * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(EVR_W_Percent * 100) / 100;
            catLine.Add(total);

            return catLine;
        }
        // Room UnitType Percent

        // GetRoomType
        public RoomType GetRandomNumByCatLine(List<float> catLine)
        {
            float luckNum = Random.Range(1, catLine[catLine.Count - 1] + 1);
            RoomType rType = RoomType.SafeRoom;

            for (int i = 0; i < catLine.Count; i++)
            {
                if (luckNum <= catLine[i])
                {
                    switch (i)
                    {
                        case 0:
                            rType = RoomType.NormalRoom;
                            break;
                        case 1:
                            rType = RoomType.EliteRoom;
                            break;
                        case 2:
                            rType = RoomType.EventRoom;
                            break;
                    }
                    break;
                }
            }

            return rType;
        }
        public RoomType GetEvRoomType(List<float> catLine)
        {
            float total = catLine[catLine.Count - 1];
            float luckNum = Random.Range(1, total + 1);
            RoomType rType = RoomType.SafeRoom;
            for (int i = 0; i < catLine.Count; i++)
            {
                if (luckNum <= catLine[i])
                {
                    rType = RoomType.MerchantRoom + i;
                    break;
                }
            }

            return rType;
        }

        public float GetPct(RoomType _roomType)
        {
            float percent = 0;
            switch (_roomType)
            {
                // Room
                case RoomType.NormalRoom:
                    percent = NR_Percent;
                    break;
                case RoomType.EliteRoom:
                    percent = _eliteRoomCreatePercent;
                    break;
                case RoomType.EventRoom:
                    percent = _eventRoomCreatePercent;
                    break;

                // Ev UnitType
                case RoomType.MerchantRoom:
                    percent = _merchantEvCreatePercent;
                    break;
                case RoomType.SkillRoom:
                    percent = _merchantEvCreatePercent;
                    break;
                case RoomType.WeaponRoom:
                    percent = _merchantEvCreatePercent;
                    break;

                // Ev up UnitType
                case RoomType.MerchantUpRoom:
                    percent = _merchantEvUpgradePercent;
                    break;
                case RoomType.SkillUpRoom:
                    percent = _skillEvUpgradePercent;
                    break;
                case RoomType.WeaponUpRoom:
                    percent = _weaponEvUpgradePercent;
                    break;
            }

            return percent;
        }
        public int GetMax(RoomType _roomType)
        {
            int maxCount = 0;
            switch (_roomType)
            {
                // Room
                case RoomType.NormalRoom:
                    maxCount = 999;
                    break;
                case RoomType.EliteRoom:
                    maxCount = _eliteRoomCreateMaxCount;
                    break;
                case RoomType.EventRoom:
                    maxCount = _eventRoomCreateMaxCount;
                    break;
                default:
                    maxCount = 0;
                    break;
            }

            return maxCount;
        }
        public int GetConstant(RoomType _roomType)
        {
            int constant = 0;
            switch (_roomType)
            {
                // Room
                case RoomType.NormalRoom:
                    break;
                case RoomType.EliteRoom:
                    constant = _eliteRoomCreateConstantCount;
                    break;
                case RoomType.EventRoom:
                    constant = _eventRoomCreateConstantCount;
                    break;

                // Ev UnitType
                case RoomType.MerchantRoom:
                    constant = _merchantEvCreateConstantCount;
                    break;
                case RoomType.SkillRoom:
                    constant = _skillEvCreateConstantCount;
                    break;
                case RoomType.WeaponRoom:
                    constant = _weaponEvCreateConstantCount;
                    break;
                default:
                    constant = 0;
                    break;
            }

            return constant;
        }
        public float GetEvUpgradePercent(RoomType _roomType)
        {
            float upgradePercent = 0;
            switch (_roomType)
            {
                case RoomType.MerchantRoom:
                    upgradePercent = _merchantEvUpgradePercent;
                    break;
                case RoomType.SkillRoom:
                    upgradePercent = _skillEvUpgradePercent;
                    break;
                case RoomType.WeaponRoom:
                    upgradePercent = _weaponEvUpgradePercent;
                    break;
                default:
                    upgradePercent = 0;
                    break;
            }

            return upgradePercent;
        }*/
        #endregion
    }
}