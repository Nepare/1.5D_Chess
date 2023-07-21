using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    [CreateAssetMenu(fileName = "LocalizationSettings", menuName = "◆ Simple Localization/Settings")]
    public class LocalizationSettings : ScriptableObject
    {
        /// <summary>
        /// Table Id on Google Sheets.
        /// Let's say your table has the following URL https://docs.google.com/spreadsheets/d/1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4/edit#gid=331980525
        /// In this case, Table Id is "1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4" and Sheet Id is "331980525" (the gid parameter).
        /// </summary>
        public string TableId;

        public List<Sheet> Sheets = new List<Sheet>();

        public UnityEngine.Object SaveFolder;

        public static string UrlPattern = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";

        private static DateTime _timestamp;

        public static LocalizationSettings Instance
        {
            get
            {
                if (_instance == null) _instance = LoadSettings();

                return _instance;
            }
        }

        private static LocalizationSettings _instance;

        public void Awake()
        {
            if (string.IsNullOrEmpty(TableId) && Sheets == null && SaveFolder == null)
            {
                Reset();
            }
        }

        private static LocalizationSettings LoadSettings()
        {
            const string settingsPath = @"Assets\SimpleLocalization\Settings.asset";
            var settings = AssetDatabase.LoadAssetAtPath<LocalizationSettings>(settingsPath);

            if (settings != null) return settings;

            settings = CreateInstance<LocalizationSettings>();
            AssetDatabase.CreateAsset(settings, settingsPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return settings;
        }

        public void DownloadGoogleSheets(Action callback = null)
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(DownloadGoogleSheetsCoroutine(callback));
        }
        
        public IEnumerator DownloadGoogleSheetsCoroutine(Action callback = null, bool silent = false)
        {
            if (string.IsNullOrEmpty(TableId) || Sheets.Count == 0)
            {
                EditorUtility.DisplayDialog("Error", "Please specify Table Id and Sheets!", "Ok");
                yield break;
            }

            if ((DateTime.UtcNow - _timestamp).TotalSeconds < 2)
            {
                if (EditorUtility.DisplayDialog("Message", "Too many requests! Try again later.", "Ok"))
                {
                    yield break;
                }
            }
            
            _timestamp = DateTime.UtcNow;

            if (!silent) ClearSaveFolder();

            for (var i = 0; i < Sheets.Count; i++)
            {
                var sheet = Sheets[i];
                var url = string.Format(UrlPattern, TableId, sheet.Id);

                Debug.Log($"Downloading <color=grey>{url}</color>");

                var request = UnityWebRequest.Get(url);
                var progress = (float)(i + 1) / Sheets.Count;

                if (EditorUtility.DisplayCancelableProgressBar("Downloading sheets...", $"[{(int)(100 * progress)}%] [{i + 1}/{Sheets.Count}] Downloading {sheet.Name}...", progress))
                {
                    yield break;
                }

                yield return request.SendWebRequest();

                var error = request.error ?? (request.downloadHandler.text.Contains("signin/identifier") ? "It seems that access to this document is denied." : null);

                if (string.IsNullOrEmpty(error))
                {
                    var path = Path.Combine(AssetDatabase.GetAssetPath(SaveFolder), sheet.Name + ".csv");

                    File.WriteAllBytes(path, request.downloadHandler.data);
                    Debug.LogFormat($"Sheet <color=yellow>{sheet.Name}</color> ({sheet.Id}) saved to <color=grey>{path}</color>");
                }
                else
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("Error", (error.Contains("404") ? "Table Id is wrong!" : error), "Ok");

                    yield break;
                }
            }

            yield return null;

            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();

            callback?.Invoke();

            if (!silent) EditorUtility.DisplayDialog("Message", "Localization sheets downloaded!", "Ok");

            void ClearSaveFolder()
            {
                var files = Directory.GetFiles(AssetDatabase.GetAssetPath(SaveFolder));

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }
        }

        public void OpenGoogleSheets()
        {
            if (string.IsNullOrEmpty(TableId))
            {
                Debug.LogWarning("Table ID is empty.");
            }
            else
            {
                Application.OpenURL($"https://docs.google.com/spreadsheets/d/{TableId}");
            }
        }

        public void LeaveReview()
        {
            Application.OpenURL($"{(LocalizationEditorWindow.IsPro() ? "http://u3d.as/34Zv" : "http://u3d.as/1cWr")}");
        }

        public void Reset()
        {
            TableId = "1RvKY3VE_y5FPhEECCa5dv4F7REJ7rBtGzQg9Z_B_DE4";
            Sheets = new List<Sheet>
            {
                new Sheet { Name = "Menu", Id = 0 },
                new Sheet { Name = "Settings", Id = 331980525 },
                new Sheet { Name = "Tests", Id = 1674352817 }
            };
            SaveFolder = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(@"Assets\SimpleLocalization\Resources\Localization");
        }

        public void ResolveGoogleSheets()
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(ResolveGoogleSheetsCoroutine());

            IEnumerator ResolveGoogleSheetsCoroutine()
            {
                if (string.IsNullOrEmpty(TableId))
                {
                    EditorUtility.DisplayDialog("Error", "Table Id is empty!", "Ok");

                    yield break;
                }

                using var request = UnityWebRequest.Get($"https://script.google.com/macros/s/AKfycbycW2dsGZhc2xJh2Fs8yu9KUEqdM-ssOiK1AlES3crLqQa1lkDrI4mZgP7sJhmFlGAD/exec?tableUrl={TableId}");

                if (EditorUtility.DisplayCancelableProgressBar("Resolving sheets...", "Executing Google App Script...", 1))
                {
                    yield break;
                }

                yield return request.SendWebRequest();

                EditorUtility.ClearProgressBar();

                if (request.error == null)
                {
                    var error = LocalizationUtils.GetInternalError(request);

                    if (error != null)
                    {
                        EditorUtility.DisplayDialog("Error", $"{(error.Contains("openById") ? "Table Id is wrong!" :error)}", "Ok");

                        yield break;
                    }

                    var sheetsDict = JsonConvert.DeserializeObject<Dictionary<string, long>>(request.downloadHandler.text);

                    Sheets.Clear();

                    foreach (var item in sheetsDict)
                    {
                        Sheets.Add(new Sheet { Id = item.Value, Name = item.Key });
                    }

                    EditorUtility.DisplayDialog("Message", $"{Sheets.Count} sheets resolved: {string.Join(", ", Sheets.Select(i => i.Name))}.", "Ok");
                }
                else
                {
                    throw new Exception(request.error);
                }
            }
        }
    }
}