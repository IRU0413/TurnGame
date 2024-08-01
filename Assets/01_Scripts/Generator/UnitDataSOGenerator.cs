using Scripts.SO;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Generator
{
    public class UnitDataSOGenerator : SOGenerator
    {
        [SerializeField] protected string _csvFileName;
        [SerializeField] private int _csvVersion; // ������ ����

        [SerializeField] protected string _unitSOFileName;
        [SerializeField] protected string _abilitySOFileName;
        [SerializeField] protected string _soundSOFileName;

        [HideInInspector][SerializeField] private string _abilityResultSavePath;
        [HideInInspector][SerializeField] private string _abilityResultDataPath;

        /*[HideInInspector][SerializeField] private string _abilityResultSavePath;
        [HideInInspector][SerializeField] private string _abilityResultSavePath;*/

        [HideInInspector][SerializeField] private string _unitResultSavePath;
        [HideInInspector][SerializeField] private string _unitResultDataPath;

        private string AbilitySavePath
        {
            get
            {
                if (_abilityResultSavePath == string.Empty)
                {
                    if (_abilitySOFileName == string.Empty)
                    {
                        Debug.Log("SO File Name is Empty");
                        return null;
                    }

                    List<string> AbilityTypeNames = GetUpperSlideStringArray(typeof(UnitAbilitySO), true);

                    // Ability ���� ���
                    _abilityResultSavePath = SO_SAVE_PATH;
                    for (int i = 0; i < AbilityTypeNames.Count - 1; i++)
                        _abilityResultSavePath += $"{AbilityTypeNames[i]}/";

                    _abilityResultSavePath += _abilitySOFileName;
                }
                return _abilityResultSavePath;
            }
        }
        private string AbilityDataPath
        {
            get
            {
                if (_abilityResultSavePath == string.Empty)
                {
                    if (_abilitySOFileName == string.Empty)
                    {
                        Debug.Log("DATA File Name is Empty");
                        return null;
                    }
                    List<string> AbilityTypeNames = GetUpperSlideStringArray(typeof(UnitAbilitySO), true);

                    // Ability ������ ���
                    _abilityResultDataPath = CSV_SAVE_PATH;
                    for (int i = 0; i < AbilityTypeNames.Count - 1; i++)
                        _abilityResultDataPath += $"{AbilityTypeNames[i]}/";

                    _abilityResultDataPath += $"{_csvFileName}_{_csvVersion}"; // ���� ��ȣ �־��־�ߵ�.
                }
                return _abilityResultDataPath;
            }
        }

        private string UnitSavePath
        {
            get
            {
                if (_unitResultSavePath == string.Empty)
                {
                    if (_unitSOFileName == string.Empty)
                    {
                        Debug.Log("SO File Name is Empty");
                        return null;
                    }

                    // Unity
                    List<string> UnitTypeNames = GetUpperSlideStringArray(typeof(UnitDataSO), true);

                    // Unit ���� ���
                    _unitResultSavePath = SO_SAVE_PATH;
                    for (int i = 0; i < UnitTypeNames.Count - 1; i++)
                        _unitResultSavePath += $"{UnitTypeNames[i]}/";

                    // ���� �̸� ����
                    _unitResultSavePath += _unitSOFileName;
                }
                return _unitResultSavePath;
            }
        }

        public override void CreateEditBtn()
        {
            base.CreateEditBtn();
            _abilityResultSavePath = "";
            _abilityResultDataPath = "";
            _unitResultSavePath = "";
            _unitResultDataPath = "";

            SettingAbilitySO();
            SettingSoundSO();
            SettingUnitDataSO();
        }

        private void SettingAbilitySO()
        {
            // ���� �̸� ����
            var dataes = CSVReader.Read(AbilityDataPath); // ������ ����

            foreach (var data in dataes)
            {
                // ���� �̸�
                string saveFindName = $"{AbilitySavePath}_{data["Id"]}";
                CreateAndSetSO<UnitAbilitySO>(saveFindName, data);
            }
        }

        private void SettingSoundSO()
        {

        }

        private void SettingUnitDataSO()
        {
            var dataes = CSVReader.Read(AbilityDataPath); // ������ ����
            foreach (var data in dataes)
            {
                // ���� �̸�
                string saveFindName = $"{UnitSavePath}_{data["Id"]}";

                // abilitySO
                string abilitySOPath = GetLoadPahtString($"{AbilitySavePath}_{data["Id"]}");
                var abilitySO = Resources.Load<UnitAbilitySO>(abilitySOPath);
                data.Add(abilitySO.GetType().Name, abilitySO);
                CreateAndSetSO<UnitDataSO>(saveFindName, data);
            }
        }
    }
}
