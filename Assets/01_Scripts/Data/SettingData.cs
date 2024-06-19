using Scripts.Enums;
using Scripts.Pattern;
using UnityEngine;

namespace Scripts.Data
{
    [System.Serializable]
    public class SettingData
    {
        // SOUND
        public float backgroundVolume; // ���
        public float voiceVolume; // ��Ҹ�
        public float effectVolume; // ����Ʈ

        // VIBRATION
        public bool screenVibration; // ����

        // UI
        public float uiScalePercent; // UI ����

        // EFFECT
        public bool viewActionEffect; // �׼� ����Ʈ
        public bool viewDamageEffect; // ���̹� ����Ʈ

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