using Scripts.Data;
using Scripts.Enums;
using Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Manager
{
    public class PlayerInfor
    {
        private const string TEAM_DATA_FILE = "PlayerTeamInformation.json";
        private bool _isNew = false;

        private string _teamName;
        private int _teamLevel;
        private int _teamHaveExp;
        private int _teamNextLevelExp;

        public bool IsNew => _isNew;

        public void Init()
        {
            DataLoad();
            if (_isNew)
            {
                _teamName = "NoName";
                DataSave();
            }
        }

        #region Save Load
        public void DataSave()
        {
            var data = new PlayerData(_teamName, _teamLevel, _teamHaveExp, _teamNextLevelExp);
            JsonController.SaveDataToJson(data, TEAM_DATA_FILE);
            Debug.Log($"SAVE - 플레이어 데이터 저장");
        }
        private void DataLoad()
        {
            var loadTeamData = JsonController.LoadDataFromJson<PlayerData>(TEAM_DATA_FILE);
            if (loadTeamData._teamLevel == 0)
            {
                LevelSettinng(1);
                _teamHaveExp = 0;

                _isNew = true;
            }
            else
            {
                _teamName = loadTeamData._teamName;
                _teamLevel = loadTeamData._teamLevel;
                _teamHaveExp = loadTeamData._teamHaveExp;
                _teamNextLevelExp = loadTeamData._teamNextLevelExp;
            }
            Debug.Log($"LOAD - 플레이어 데이터 로드");
        }
        #endregion
        public void SetExp(int exp)
        {
            _teamHaveExp += exp;
            if (_teamHaveExp >= _teamNextLevelExp)
            {
                int findLevel = _teamLevel + 1;
                LevelSettinng(findLevel);
            }
        }
        public void LevelSettinng(int findLevel)
        {
            List<Dictionary<string, object>> teamLvDatas = CSVReader.Read($"CSV/TeamLv/TeamLv_{0}");
            Dictionary<string, object> lvData = null;
            foreach (var data in teamLvDatas)
            {
                int level = ExtraFunction.GetValueToKeyEnum<int>(data, TeamCSVProperty.TeamLevel);

                if (level == findLevel)
                {
                    lvData = data;

                    _teamLevel = findLevel;
                    _teamNextLevelExp = ExtraFunction.GetValueToKeyEnum<int>(lvData, TeamCSVProperty.TeamNextLevelExp);
                    break;
                }
            }
        }
    }
}