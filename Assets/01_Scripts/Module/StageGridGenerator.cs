using Scripts.Enums;
using Scripts.Interface;
using Scripts.Module.StageGridPath;
using Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Module
{
    public class StageGridGenerator
    {
        private StageSO _stageSo;
        private Dictionary<RoomType, List<Vector2Int>> _roomDictionary;

        private IPathCreator _pathCreator;

        private int _normalRoomCount;
        private int _eliteRoomCount;
        private int _eventRoomCount;

        private int _merchantEVCount;
        private int _skillEVCount;
        private int _weaponEVCount;

        public Dictionary<RoomType, List<Vector2Int>> Map => _roomDictionary;

        public StageGridGenerator(GameModeType type)
        {
            _roomDictionary = new();
            SetGridCreator(type);
        }
        public void SetGridCreator(GameModeType type)
        {
            if (type == GameModeType.None)
                type = GameModeType.Normal;

            switch (type)
            {
                case GameModeType.Test:
                    _pathCreator = new AstarPathCreator();
                    break;
                case GameModeType.None:
                case GameModeType.Normal:
                    _pathCreator = new RandomPathCreator();
                    break;
            }
        }

        public void ClearMap()
        {
            _roomDictionary.Clear();
        }
        public bool CreateStageGrid(StageSO so)
        {
            if (so == null)
                return false;

            _stageSo = so;
            CreateGrid();
            return true;
        }
        private void CreateGrid()
        {
            List<Vector2Int> allocablePos = new List<Vector2Int>();

            Vector2Int startRoomPos = Vector2Int.zero;
            Vector2Int bossRoomPos = Vector2Int.zero;

            allocablePos = _pathCreator.CreatePath(startRoomPos, _stageSo.AddRoomCount);
            bossRoomPos = GetBossRoomPosition(allocablePos, _stageSo.Distacne); // 보스 룸 위치 지정

            // 지정 생성
            AddRoom(RoomType.SafeRoom, startRoomPos); // 세이프
            allocablePos.Remove(startRoomPos);
            AddRoom(RoomType.BossRoom, bossRoomPos); // 보스
            allocablePos.Remove(bossRoomPos);

            AddConstantRooms(allocablePos, RoomType.EliteRoom); // 엘리트
            AddConstantRooms(allocablePos, RoomType.EventRoom); // 이벤트

            // 비할당된 위치 > 룸 타입 세팅
            AddBaseRooms(allocablePos);

            // 이벤트 룸 세팅
            TryEventRoomsSetting();
        }


        // 룸 추가
        private bool AddRoom(RoomType type, Vector2Int pos)
        {
            if (_stageSo == null || _roomDictionary == null) return false;

            if (!_roomDictionary.ContainsKey(type))
                _roomDictionary.Add(type, new());

            if (_roomDictionary[type].Contains(pos))
                return false;

            // 추가
            _roomDictionary[type].Add(pos);
            return true;
        }
        private bool AddRoom(List<Vector2Int> allocablePos, RoomType type = RoomType.None, Vector2Int pos = default)
        {
            if (pos == default)
                pos = GetRandomPosition(allocablePos);

            if (type == RoomType.None)
                type = GetUseableRoomType();

            bool isAdded = AddRoom(type, pos);
            if (isAdded)
                allocablePos.Remove(pos);

            return isAdded;
        }
        private void AddConstantRooms(List<Vector2Int> allocablePos, RoomType type)
        {
            int crr = GetRoomCurrentCount(type);
            int constant = GetRoomConstantCount(type);
            int max = GetRoomMaxCount(type);
            int count = (constant > max) ? max : constant;

            if (count > allocablePos.Count)
                count = allocablePos.Count;

            for (int i = 0; i < count; i++)
            {
                AddRoom(allocablePos, type);
            }
        }
        private void AddBaseRooms(List<Vector2Int> allocablePos)
        {
            List<float> catLine = GetBaseRoomCatLinePercent();
            RoomType minType = RoomType.NormalRoom;
            RoomType type = RoomType.None;
            int durationCount = allocablePos.Count;

            for (int i = 0; i < durationCount; i++)
            {
                type = RoomType.None;
                type = GetRandomRoomType(catLine, minType);
                int crr = GetRoomCurrentCount(type);
                int max = GetRoomMaxCount(type);

                if (crr >= max)
                    type = GetUseableRoomType();

                AddRoom(allocablePos, type);
                crr++;
                SetRoomCount(type, crr);
            }
        }
        private void TryEventRoomsSetting()
        {
            if (!_roomDictionary.ContainsKey(RoomType.EventRoom)) return;

            List<float> catLine = GetEventRoomCatLinePercent();
            RoomType minType = RoomType.MerchantRoom;
            RoomType type = RoomType.None;

            List<Vector2Int> eventRoomPosList = _roomDictionary[RoomType.EventRoom];
            foreach (var pos in eventRoomPosList)
            {
                type = RoomType.None;
                type = GetRandomRoomType(catLine, minType);

                int crr = GetRoomCurrentCount(type);
                int max = GetRoomMaxCount(type);

                if (crr >= max)
                    type = GetUseableRoomType();

                AddRoom(type, pos);
                crr++;
                SetRoomCount(type, crr);
            }
        }

        // 룸 위치 특정
        private Vector2Int GetBossRoomPosition(List<Vector2Int> allocablePos, int distance)
        {
            int count = allocablePos.Count;
            Vector2Int resultPos = new Vector2Int();
            if (count <= 0)
            {
                resultPos = Vector2Int.right;
                return resultPos;
            }

            List<Vector2Int> under = new List<Vector2Int>(); // 미만
            List<Vector2Int> over = new List<Vector2Int>(); // 이상

            foreach (var pos in allocablePos)
            {
                int posDistance = Mathf.Abs(pos.x) + Mathf.Abs(pos.y);
                if (posDistance < distance)
                    under.Add(pos);
                else
                    over.Add(pos);
            }

            if (over.Count > 0)
                resultPos = over[Random.Range(0, over.Count)];
            else
                resultPos = under[Random.Range(0, under.Count)];

            return resultPos;
        }

        // 해당 룸타입 현재 수
        public int GetRoomCurrentCount(RoomType type)
        {
            if (_stageSo == null || _roomDictionary == null)
                return -1;

            int result = 0;
            switch (type)
            {
                // base
                case RoomType.SafeRoom:
                case RoomType.BossRoom:
                case RoomType.NormalRoom:
                case RoomType.EliteRoom:
                case RoomType.EventRoom:
                    if (!_roomDictionary.ContainsKey(type) || _roomDictionary[type] == null)
                        break;

                    List<Vector2Int> typeRoomPosList = _roomDictionary[type];
                    result = typeRoomPosList.Count;
                    break;

                // event
                case RoomType.MerchantRoom:
                case RoomType.SkillRoom:
                case RoomType.WeaponRoom:
                    int start = (int)RoomType.MerchantRoom;
                    int end = (int)RoomType.WeaponRoom;
                    for (int i = start; i < end; i++)
                    {
                        RoomType checkType = (RoomType)i;
                        if (!_roomDictionary.ContainsKey(checkType) || _roomDictionary[checkType] == null)
                            continue;

                        result += _roomDictionary[checkType].Count;
                    }
                    break;
                // None
                default:
                    return -1;
            }

            return result;
        }
        // 해당 룸타입 고정 생성 수
        public int GetRoomConstantCount(RoomType type)
        {
            if (_stageSo == null)
                return -1;

            int result = 0;
            switch (type)
            {
                // base
                case RoomType.SafeRoom:
                case RoomType.BossRoom:
                    result = 1;
                    break;
                case RoomType.NormalRoom:
                    result = int.MaxValue;
                    break;
                case RoomType.EliteRoom:
                    result = (_stageSo.EliteRoomData != null) ? _stageSo.EliteRoomData.ConstantCount : 0;
                    break;
                case RoomType.EventRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.ConstantCount : 0;
                    break;

                // event
                case RoomType.MerchantRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.MerchantConstantCount : 0;
                    break;
                case RoomType.WeaponRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.WeaponConstantCount : 0;
                    break;
                case RoomType.SkillRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.SkillConstantCount : 0;
                    break;
                // None
                default:
                    return -1;
            }

            return result;
        }
        // 해당 룸타입 최대 생성 수
        public int GetRoomMaxCount(RoomType type)
        {
            if (_stageSo == null)
                return -1;
            int result = 0;
            switch (type)
            {
                // base
                case RoomType.SafeRoom:
                case RoomType.BossRoom:
                    result = 1;
                    break;
                case RoomType.NormalRoom:
                    result = int.MaxValue;
                    break;
                case RoomType.EliteRoom:
                    result = (_stageSo.EliteRoomData != null) ? _stageSo.EliteRoomData.MaxCount : 0;
                    break;
                case RoomType.EventRoom:
                case RoomType.MerchantRoom:
                case RoomType.WeaponRoom:
                case RoomType.SkillRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.MaxCount : 0;
                    break;
                // None
                default:
                    return -1;
            }

            return result;
        }
        // 해당 룸 생성 확률
        public float GetBaseRoomCreatePercent(RoomType type)
        {
            if (_stageSo == null)
                return -1;

            float result = 0;
            switch (type)
            {
                // base
                case RoomType.SafeRoom:
                case RoomType.BossRoom:
                    result = 100.0f;
                    break;
                case RoomType.NormalRoom:
                    result = 100.0f;
                    result -= (_stageSo.EliteRoomData != null) ? _stageSo.EliteRoomData.CreatePercent : 0;
                    result -= (_stageSo.EliteRoomData != null) ? _stageSo.EventRoomData.CreatePercent : 0;
                    break;
                case RoomType.EliteRoom:
                    result = (_stageSo.EliteRoomData != null) ? _stageSo.EliteRoomData.CreatePercent : 0;
                    break;
                case RoomType.EventRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.ConstantCount : 0;
                    break;

                // event
                case RoomType.MerchantRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.MerchantCreatePercent : 0;
                    break;
                case RoomType.WeaponRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.WeaponCreatePercent : 0;
                    break;
                case RoomType.SkillRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.SkillCreatePercent : 0;
                    break;
                // None
                default:
                    return -1;
            }

            return result;
        }
        // 해당 이벤트 룸 업그레이드 확률
        public float GetEventRoomUpgradePercent(RoomType type)
        {
            if (_stageSo == null)
                return -1;

            float result = 0;
            switch (type)
            {
                // event
                case RoomType.MerchantRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.MerchantEvUpgradePercent : 0;
                    break;
                case RoomType.WeaponRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.WeaponEvUpgradePercent : 0;
                    break;
                case RoomType.SkillRoom:
                    result = (_stageSo.EventRoomData != null) ? _stageSo.EventRoomData.SkillEvUpgradePercent : 0;
                    break;
                // None
                default:
                    return -1;
            }

            return result;
        }
        // 지정 룸 수 수정
        private void SetRoomCount(RoomType roomType, int count)
        {
            switch (roomType)
            {
                case RoomType.NormalRoom:
                    _normalRoomCount = count;
                    break;
                case RoomType.EliteRoom:
                    _eliteRoomCount = count;
                    break;
                case RoomType.MerchantRoom:
                    _merchantEVCount = count;
                    _eventRoomCount = _merchantEVCount + _skillEVCount + _weaponEVCount;
                    break;
                case RoomType.SkillRoom:
                    _skillEVCount = count;
                    _eventRoomCount = _merchantEVCount + _skillEVCount + _weaponEVCount;
                    break;
                case RoomType.WeaponRoom:
                    _weaponEVCount = count;
                    _eventRoomCount = _merchantEVCount + _skillEVCount + _weaponEVCount;
                    break;

                default: break;
            }
        }
        // 모든 룸 카운팅
        public void CountingAllRoom()
        {
            int maxCount = (int)RoomType.WeaponRoom;
            for (int i = 0; i <= maxCount; i++)
            {
                RoomType type = (RoomType)i;

                int count = 0;
                if (_roomDictionary.ContainsKey(type))
                    count = _roomDictionary[type].Count;

                SetRoomCount(type, count);
            }
        }


        // 랜덤한 타입
        private RoomType GetRandomRoomType(List<float> catLine, RoomType minType)
        {
            int catLineCount = catLine.Count;
            int roomTypeMin = (int)minType;

            RoomType resultType = RoomType.None;
            float maxluck = catLine[catLineCount - 1];

            float luckNum = Random.Range(1, catLine[catLineCount - 1] + 1);
            for (int j = 0; j < catLineCount; j++)
            {
                luckNum = Random.Range(0, maxluck);
                if (luckNum < catLine[j])
                {
                    resultType = (RoomType)(roomTypeMin + j);
                    break;
                }
            }
            return resultType;
        }
        // 사용 가능한 타입
        private RoomType GetUseableRoomType()
        {
            List<RoomType> useableRoomType = new();
            int durationCount = (int)RoomType.WeaponRoom;

            RoomType rType = RoomType.None;
            for (int i = 0; i < durationCount; i++)
            {
                rType = (RoomType)i;

                int maxCount = GetRoomMaxCount(rType);
                int currentCount = GetRoomCurrentCount(rType);
                if (currentCount < maxCount)
                    useableRoomType.Add(rType);
            }

            if (useableRoomType.Count > 0)
            {
                int idx = Random.Range(0, useableRoomType.Count);
                rType = useableRoomType[idx];
            }
            else
                rType = RoomType.NormalRoom;

            return rType;
        }


        // 커트라인
        private List<float> GetBaseRoomCatLinePercent()
        {
            // 커트 라인
            List<float> catLine = new List<float>();
            float total = 0;

            float eliteP = GetBaseRoomCreatePercent(RoomType.EliteRoom);
            float eventP = GetBaseRoomCreatePercent(RoomType.EventRoom);
            float normalP = 100.0f - (eliteP + eventP);
            normalP = (normalP < 0) ? 0 : normalP;

            total += Mathf.Round(normalP * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(eliteP * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(eventP * 100) / 100;
            catLine.Add(total);

            return catLine;
        }
        private List<float> GetEventRoomCatLinePercent()
        {
            // 커트 라인
            List<float> catLine = new List<float>();
            float total = 0;

            float merchant = GetBaseRoomCreatePercent(RoomType.MerchantRoom);
            float skill = GetBaseRoomCreatePercent(RoomType.SkillRoom);
            float weapon = GetBaseRoomCreatePercent(RoomType.WeaponRoom);

            total += Mathf.Round(merchant * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(skill * 100) / 100;
            catLine.Add(total);

            total += Mathf.Round(weapon * 100) / 100;
            catLine.Add(total);

            return catLine;
        }

        // 랜덤 위치
        private Vector2Int GetRandomPosition(List<Vector2Int> allocablePos)
        {
            if (allocablePos.Count == 0 || allocablePos == null)
                return Vector2Int.zero;

            Vector2Int roomPos = allocablePos[Random.Range(0, allocablePos.Count)];
            return roomPos;
        }
    }
}