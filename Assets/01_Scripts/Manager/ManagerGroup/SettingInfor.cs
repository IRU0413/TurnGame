using Scripts.Data;
using Scripts.Util;
using UnityEngine;

namespace Scripts.Manager
{
    public class SettingInfor
    {
        private const string SETTING_DATA_FILE = "SettingInformation.json";
        private SettingData settingData = new SettingData();

        public void Init()
        {
            DataLoad();
        }
        public void DataSave()
        {
            JsonController.SaveDataToJson(settingData, SETTING_DATA_FILE);
            Debug.Log($"SAVE - 세팅 데이터 저장");
        }
        private void DataLoad()
        {
            settingData = JsonController.LoadDataFromJson<SettingData>(SETTING_DATA_FILE);
            Debug.Log($"LOAD - 세팅 데이터 로드");
        }

        public void SetVolume(float background, float voice, float effect)
        {
            settingData.backgroundVolume = background;
            settingData.voiceVolume = voice;
            settingData.effectVolume = effect;
        }
        public void SetUl(float uiScalePercent)
        {
            settingData.uiScalePercent = uiScalePercent;
        }
        public void SetEffect(bool screenVibration, bool viewActionEffet, bool viewDamageEffect)
        {
            settingData.screenVibration = screenVibration;
            settingData.viewActionEffect = viewActionEffet;
            settingData.viewDamageEffect = viewDamageEffect;
        }
    }
}