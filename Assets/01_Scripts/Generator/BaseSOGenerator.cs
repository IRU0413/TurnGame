using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Generator
{
    public class BaseSOGenerator : MonoBehaviour
    {
        private enum SOGeeneratorType
        {
            None = 0,
            Item,
        }

        [Header("±âº»")]
        private const string BASE_PATH = "Assets/Resources/";
        private const string BASE_CSV_PATH = "CSV/";
        private const string BASE_SO_PATH = "SO/";
        protected const string BASE_FILE_NAME = "_CSV_Data_";

        [SerializeField] private SOGeeneratorType _generatorType = SOGeeneratorType.None;
        [SerializeField] private int _csvVersion = 0;

        protected List<Dictionary<string, object>> _datas = null;

        public virtual void CreateSOAssets() { }
        private string GetCSVFilePath(string midPath, string fileFullName)
        {
            return $"{BASE_CSV_PATH}{_generatorType}/{midPath}/{fileFullName}";
        }
        protected string GetCSVFullFileName(string fileName)
        {
            return $"{fileName}{BASE_FILE_NAME}{_csvVersion}";
        }
        protected string GetSOFilePath(string midPath, string soName)
        {
            return $"{BASE_PATH}{BASE_SO_PATH}{_generatorType}/{midPath}/{soName}";
        }

        protected List<Dictionary<string, object>> GetDatas(string midPath)
        {
            string fullName = GetCSVFullFileName(midPath);
            string path = GetCSVFilePath(midPath, fullName);
            Debug.Log(path);
            List<Dictionary<string, object>> datas = CSVReader.Read(path);
            return datas;
        }
        protected List<Dictionary<string, object>> GetDatas(string midPath, string fileName)
        {
            string fullName = GetCSVFullFileName(fileName);
            string path = GetCSVFilePath(midPath, fullName);
            Debug.Log(path);
            List<Dictionary<string, object>> datas = CSVReader.Read(path);
            return datas;
        }
    }
}
