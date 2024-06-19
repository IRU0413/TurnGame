namespace Scripts.Data
{
    [System.Serializable]
    public class PlayerData
    {
        public string _teamName = "DefaultTeamName";
        public int _teamLevel = 0;
        public int _teamHaveExp = 0;
        public int _teamNextLevelExp = int.MaxValue;

        public PlayerData() { }

        public PlayerData(string teamName, int teamLevel, int teamHaveExp, int teamNextLevelExp)
        {
            _teamName = teamName;
            _teamLevel = teamLevel;
            _teamHaveExp = teamHaveExp;
            _teamNextLevelExp = teamNextLevelExp;
        }
    }
}