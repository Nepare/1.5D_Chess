using UnityEngine;
using System.Collections.Generic;

public class LanguageController : MonoBehaviour
{
    public static int LANG_ID = 1;
    public static int SKYBOX_ID = 1;
    public static Dictionary<string, Dictionary<string, string>> langDict = new Dictionary<string, Dictionary<string, string>>();
    private static string[] lines;

    public static void ReadTable()
    {
        var fileData = Resources.Load<TextAsset>("Menu").ToString();
        lines = fileData.Split("\n");
        
        for (int i = 1; i < lines.Length; i++)
        {
            var current_line = lines[i].Split(",");
            langDict[current_line[0]] = new Dictionary<string, string>();
            for (int j = 1; j < current_line.Length; j++)
            {
                langDict[current_line[0]][lines[0].Split(",")[j]] = current_line[j];
            }
        }
    }

    public static string GetWord(string key)
    {
        return langDict[key][lines[0].Split(",")[LANG_ID]];
    }
}
