using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using Unity.EditorCoroutines.Editor;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    public class LocalizationEditorWindow : EditorWindow
    {
        public string SheetName;
        public long SheetId;
        
        private static LocalizationSettings Settings => LocalizationSettings.Instance;
        private static LocalizationEditorWindow _window; 
        private static LocalizationEditor _editor;
        private static int _columnWidth = 200;

        private int _selectedSheet;
        private Vector2 _scrollPosition;
        private DateTime _timeStamp;
        private string _filter = "";

        private const int MinColumnWidth = 200;
        private const string GoogleScriptControllerUrl = "";
        
        [MenuItem("Window/◆ Simple Localization/Editor [PRO]")]
        public static void OpenEditor()
        {
            if (_window == null)
            {
                _editor = new LocalizationEditor();
                _window = CreateInstance<LocalizationEditorWindow>();
                _window.titleContent = new GUIContent("Localization Editor");
                _window.minSize = new Vector2(1150, 200);
            }

            Settings.Sheets.ForEach(i =>
            {
                _editor.SheetNames.Add(i.Name);
                _editor.SheetIds.Add(i.Id);
            });

            _window.Show();

        }

        public void OnGUI()
        {
            if (Settings.Sheets.Count == 0 || _editor.SheetNames.Count == 0 || string.IsNullOrEmpty(Settings.TableId) || Settings.SaveFolder == null)
            {
                if (EditorUtility.DisplayDialog("Error", "Wrong table settings!", "Ok"))
                {
                    Close();
                }

                return;
            }
            
            if (Directory.GetFiles(AssetDatabase.GetAssetPath(Settings.SaveFolder)).Length == 0)
            {
                if (EditorUtility.DisplayDialog("Error", "Nothing to edit. You need to download sheets in the Settings window!", "Ok"))
                {
                    Close();
                }

                return;
            }

            EditorGUILayout.Space();

            SheetName = MakeSheetsDropdown(_editor.SheetNames);

            if (_editor.SheetDictionary.Count == 0)
            {
                SheetId = _editor.SheetIds[_selectedSheet];

                if (!_editor.ReadSorted(SheetName))
                {
                    Close();
                }

                if (_editor.SheetDictionary.Count == 0)
                {
                    return;
                }
            }

            EditorGUILayout.Space();
            MakeKeysDropdown();
            EditorGUILayout.Space();
            MakeFilterRow();
            EditorGUILayout.Space();
            MakeTable();
            EditorGUILayout.Space();

            if (_editor.KeysActions.Count > 0)
            {
                MakeBottomMenu();
            }
        }

        private void MakeFilterRow()
        {
            GUILayout.BeginHorizontal();
            _filter = EditorGUILayout.TextField("Key filter:", _filter, GUILayout.MinWidth(400), GUILayout.ExpandWidth(false));
            if (_columnWidth > MinColumnWidth)
            {
                if (GUILayout.Button("−", GUILayout.MaxWidth(25)))
                {
                    _columnWidth -= 25;
                }
            }
            else
            {
                GUILayout.Space(28);
            }
            GUILayout.Label("Column Width", GUILayout.MaxWidth(85));
            if (GUILayout.Button("✚", GUILayout.MaxWidth(25)))
            {
                _columnWidth += 25;
            }
            GUILayout.EndHorizontal();
        }

        public void OnDestroy()
        {
            if (_editor.KeysActions.Count > 0)
            {
                if (EditorUtility.DisplayDialog("Message", "Do you want to submit changes before exiting?", "Yes", "No"))
                {
                    var keys = _editor.Keys.ToDictionary(entry => entry.Key, entry => entry.Value);
                    var sheetDictionary = _editor.SheetDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
                    var keysActions = _editor.KeysActions.ToDictionary(entry => entry.Key, entry => entry.Value);

                    EditorCoroutineUtility.StartCoroutine(SaveSheet(keys, keysActions, sheetDictionary), this);
                }
            }

            _editor.ResetSheet();
        }

        private void MakeTable()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Width(position.width), GUILayout.ExpandWidth(true));
            MakeLanguagesRow();

            if (_editor.CurrentKey != "")
            {
                MakeSheetRow(_editor.CurrentKey);
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.EndScrollView();
        }

        private static void MakeLanguagesRow()
        {
            var style = new GUIStyle { fontStyle = FontStyle.Bold, normal = { textColor = Color.white }, alignment = TextAnchor.MiddleCenter };

            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal("box", GUILayout.MinWidth(MinColumnWidth)); 
            GUILayout.Label("Key" + (_editor.CurrentKey == "" ? "" : _editor.Keys[_editor.CurrentKey] != _editor.CurrentKey ? " *" : ""), style, GUILayout.MinWidth(200));
            GUILayout.EndHorizontal();

            foreach (var language in _editor.SheetDictionary.Keys)
            {
                GUILayout.BeginHorizontal("box", GUILayout.MinWidth(_columnWidth));
                GUILayout.Label(language, style, GUILayout.MinWidth(_columnWidth));
                GUILayout.EndHorizontal();
            }

            GUILayout.EndHorizontal();
        }

        private static void MakeSheetRow(string key)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginHorizontal("box", GUILayout.MinWidth(MinColumnWidth));

            var newValue = EditorGUILayout.TextField(_editor.Keys[key], GUILayout.MinHeight(50), GUILayout.MaxWidth(200), GUILayout.MinWidth(200)); 

            GUILayout.EndHorizontal();
            
            if (_editor.Keys[key] != newValue)
            {
                var duplicateKey = _editor.Keys.ContainsKey(newValue) ? _editor.Keys[newValue] : _editor.Keys.Values.Contains(newValue) ? _editor.Keys.First(i => i.Value == newValue).Key : "";

                if (duplicateKey != "" && _editor.KeysActions[duplicateKey] != ActionType.Delete || _editor.Keys.Count(i => i.Value == newValue) > 1)
                {
                    EditorUtility.DisplayDialog("Error", "A key with the same name already exists!", "Ok");
                }
                else
                {
                    _editor.Keys[key] = newValue;

                    if (!_editor.IsNewKey(key) && !_editor.KeysActions.ContainsKey(key))
                    {
                        _editor.KeysActions.Add(key, ActionType.Edit);
                    }
                }
            }

            foreach (var language in _editor.SheetDictionary.Keys)
            {
                GUILayout.BeginHorizontal("box", GUILayout.MinWidth(_columnWidth));

                var value = EditorGUILayout.TextArea(_editor.SheetDictionary[language][key], GUILayout.MinHeight(50), GUILayout.MaxWidth(_columnWidth), GUILayout.MinWidth(_columnWidth));

                GUILayout.EndHorizontal();

                if (value == _editor.SheetDictionary[language][key]) continue;

                _editor.SheetDictionary[language][key] = value;

                if (!_editor.IsNewKey(key) && !_editor.KeysActions.ContainsKey(key))
                {
                    _editor.KeysActions.Add(key, ActionType.Edit);
                }
            }

            GUILayout.EndHorizontal();
        }

        private void MakeKeysDropdown()
        {
            GUILayout.BeginHorizontal();
            _editor.GetAllKeys();

            var filter = _filter.ToLower();
            var keysArray = filter.Length > 2
                ? _editor.Keys.Keys.Where(i => !IsDeleted(i) && i.ToLower().Contains(filter) || i == _editor.CurrentKey).ToArray()
                : _editor.Keys.Keys.Where(i => !IsDeleted(i)).ToArray();
            var valuesArray = _editor.Keys.Where(i => keysArray.Contains(i.Key)).Select(i => i.Value).ToArray();
            var selectedKeyArrayIndex = string.IsNullOrEmpty(_editor.CurrentKey) ? 0 : Array.IndexOf(keysArray, _editor.CurrentKey);

            selectedKeyArrayIndex = EditorGUILayout.Popup("Key:", selectedKeyArrayIndex, valuesArray, GUILayout.MinWidth(400), GUILayout.ExpandWidth(false));
            _editor.CurrentKey = keysArray.Length == 0 ? "" : keysArray[selectedKeyArrayIndex];

            if (GUILayout.Button("Add Key", GUILayout.Width(110)))
            {
                _editor.CurrentKey = _editor.AddKey();
            }

            if (GUILayout.Button("Delete Key", GUILayout.Width(110)))
            {
                if (EditorUtility.DisplayDialog("Message", "Do you want to delete this key?", "Yes", "No"))
                {
                    _editor.CurrentKey = _editor.DeleteKey(_editor.CurrentKey);
                }
            }

            if (GUILayout.Button("Auto Translate", GUILayout.Width(110)))
            {
                EditorCoroutineUtility.StartCoroutine(_editor.Translate(), this);

                if (!_editor.IsNewKey(_editor.CurrentKey))
                {
                    if (!_editor.KeysActions.ContainsKey(_editor.CurrentKey))
                    {
                        _editor.KeysActions.Add(_editor.CurrentKey, ActionType.Edit);
                    }
                }
            }

            GUILayout.EndHorizontal();

            bool IsDeleted(string key)
            {
                return _editor.KeysActions.ContainsKey(key) && _editor.KeysActions[key] == ActionType.Delete;
            }
        }

        private string MakeSheetsDropdown(List<string> sheetNames)
        {
            GUILayout.BeginHorizontal();

            var sheetNamesArray = sheetNames.ToArray();
            var newSheet = EditorGUILayout.Popup("Sheet:", _selectedSheet, sheetNamesArray, GUILayout.MinWidth(400), GUILayout.ExpandWidth(false));
            
            if (GUILayout.Button("▼ Download Sheets", GUILayout.Width(167)))
            {
                Settings.DownloadGoogleSheets(_editor.ResetSheet);
            }

            if (GUILayout.Button("❖ Open Sheets", GUILayout.Width(167)))
            {
                Settings.OpenGoogleSheets();
            }

            if (GUILayout.Button("★ Leave Review", GUILayout.Width(167)))
            {
                Settings.LeaveReview();
            }

            GUILayout.EndHorizontal();

            if (_selectedSheet == newSheet) return sheetNamesArray[_selectedSheet];

            if (_editor.KeysActions.Count > 0)
            {
                if (EditorUtility.DisplayDialog("Message", "Changes will be lost when switching sheet!\nDo you want to submit changes?", "Yes", "No"))
                {
                    EditorCoroutineUtility.StartCoroutine(SaveSheet(_editor.Keys, _editor.KeysActions, _editor.SheetDictionary), this);
                }
            }

            _editor.ResetSheet();
            _selectedSheet = newSheet;

            return sheetNamesArray[_selectedSheet];
        }

        public void MakeBottomMenu()
        {
            var style = new GUIStyle {normal = {textColor = Color.gray}};

            GUILayout.BeginHorizontal("box");
            
            if (GUILayout.Button("Submit"))
            {
                EditorCoroutineUtility.StartCoroutine(SaveSheet(_editor.Keys, _editor.KeysActions, _editor.SheetDictionary), this);
            }

            if (GUILayout.Button("Revert") && EditorUtility.DisplayDialog("Message", "Do you want to revert changes?", "Yes", "No"))
            {
                _editor.ResetSheet();
            }

            GUILayout.Label($"Pending submit of {_editor.KeysActions.Count} changes", style);
            GUILayout.EndHorizontal();
        }

        public static bool IsPro()
        {
            if (!string.IsNullOrEmpty(GoogleScriptControllerUrl)) return true;

            if (EditorUtility.DisplayDialog("Message", "This feature is available in Simple Localization PRO.\nYou can support my asset by purchasing it.\nDo you want to open the Asset Store?", "Yes", "No"))
            {
                Application.OpenURL("http://u3d.as/34Zv");
            }

            return false;
        }

        private IEnumerator SaveSheet(Dictionary<string, string> keys, Dictionary<string, ActionType> keysActions, Dictionary<string, SortedDictionary<string, string>> sheetDictionary)
        {
            if (!IsPro()) yield break;

            if ((DateTime.UtcNow - _timeStamp).TotalSeconds < 2)
            {
                if (EditorUtility.DisplayDialog("Error", "Too many requests! Try again later.", "Ok"))
                {
                    yield break;
                }

                yield break;
            }

            _timeStamp = DateTime.UtcNow;
            
            var rows = new List<Dictionary<string, string>>();

            foreach (var item in keysActions)
            {
                rows.Add(LocalizationUtils.CreateRow(item.Key, item.Value, keys, sheetDictionary));
            }

            var success = false;

            _editor.PrevKey = _editor.CurrentKey;

            yield return LocalizationUtils.SubmitChanges(rows, SheetId, Settings.TableId, GoogleScriptControllerUrl, () => success = true);

            if (!success)
            {
                yield break;
            }

            yield return Settings.DownloadGoogleSheetsCoroutine(() =>
            {
                _editor.ResetSheet();
                _editor.ReadSorted(SheetName);
                EditorUtility.DisplayDialog("Message", "Changes submitted!", "Ok");
            }, silent: true);
        }
    }
}
