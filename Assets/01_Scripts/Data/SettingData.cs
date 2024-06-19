using Scripts.Enums;
using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Data
{
    [System.Serializable]
    public class SettingData
    {
        // SOUND
        public float backgroundVolume; // 배경
        public float voiceVolume; // 목소리
        public float effectVolume; // 이펙트

        // VIBRATION
        public bool screenVibration; // 진동

        // UI
        public float uiScalePercent; // UI 배율

        // EFFECT
        public bool viewActionEffect; // 액션 이펙트
        public bool viewDamageEffect; // 데이미 이펙트

        // KEY
        public SerializableDictionary<UseKeyType, KeyCode> useKeys = new SerializableDictionary<UseKeyType, KeyCode>();

        public SettingData()
        {
            backgroundVolume = 0.5f;
            voiceVolume = 0.5f;
            effectVolume = 0.5f;

            screenVibration = true;
            uiScalePercent = 1.0f;

            viewActionEffect = true;
            viewDamageEffect = true;

            int count = (int)UseKeyType.Setting;
            for (int i = 0; i < count; i++)
                useKeys.Add((UseKeyType)i, KeyCode.None);
            useKeys.OnBeforeSerialize();
        }
    }
}