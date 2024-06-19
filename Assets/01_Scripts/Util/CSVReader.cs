using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string filePath)
    {
        var list = new List<Dictionary<string, object>>();
        // 필요 데이터 불러오기
        TextAsset data = Resources.Load(filePath) as TextAsset;

        // 출 바꾸는 무자 확인하고 라인짤라서 저장
        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        // 라인이 1줄 이라는거는 Header만 존재하는 것임
        if (lines.Length <= 1) return list;

        // Header 저장
        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {
            // 데이터 라인에 있는 데이터를 Header처럼 데이터 조각 조각 짜르기
            var values = Regex.Split(lines[i], SPLIT_RE);
            // 짜른데이터가 없거다면 다음줄 확인하기
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                object finalvalue = value;
                int n;
                float f;
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;
            }
            list.Add(entry);
        }
        return list;
    }
}
