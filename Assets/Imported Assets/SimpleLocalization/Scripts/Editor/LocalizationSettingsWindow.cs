using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    public class LocalizationSettingsWindow : EditorWindow
    {
        private static SerializedObject _serializedObject;

        private static LocalizationSettings Settings => LocalizationSettings.Instance;

        [MenuItem("Window/◆ Simple Localization/Settings")]
        public static void ShowWindow()
        {
            GetWindow<LocalizationSettingsWindow>("Localization Settings");
        }

        [MenuItem("Window/◆ Simple Localization/Reset")]
        public static void ResetSettings()
        {
            if (EditorUtility.DisplayDialog("Simple Localization", "Do you want to reset settings?", "Yes", "No"))
            {
                LocalizationSettings.Instance.Reset();
            }
        }

        [MenuItem("Window/◆ Simple Localization/Help")]
        public static void Help()
        {
            Application.OpenURL("https://github.com/hippogamesunity/SimpleLocalization/wiki");
        }

        public void OnGUI()
        {
            MakeSettingsWindow();
        }
        
        private void MakeSettingsWindow()
        {
            minSize = new Vector2(300, 500);
            Settings.TableId = EditorGUILayout.TextField("Table Id", Settings.TableId, GUILayout.MinWidth(200));
            Settings.SaveFolder = EditorGUILayout.ObjectField("Save Folder", Settings.SaveFolder, typeof(Object), false);
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            DrawList();

            EditorGUILayout.Space();

            var buttonStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold, fixedHeight = 30 };

            if (GUILayout.Button("↺ Resolve Sheets", buttonStyle))
            {
                Settings.ResolveGoogleSheets();
            }

            if (GUILayout.Button("▼ Download Sheets", buttonStyle))
            {
                if (string.IsNullOrEmpty(Settings.TableId))
                {
                    Debug.Log("LocalizationSyncWindow: Can't make sync while Table Id is empty!");
                }
                else
                {
                    Settings.DownloadGoogleSheets();
                }
            }

            if (GUILayout.Button("❖ Open Google Sheets", buttonStyle))
            {
                Settings.OpenGoogleSheets();
            }

            if (GUILayout.Button("★ Leave Review", buttonStyle))
            {
                Settings.LeaveReview();
            }
        }

        private static void DrawList()
        {
            if (_serializedObject == null || _serializedObject.targetObject == null)
            {
                _serializedObject = new SerializedObject(Settings);
            }
            else
            {
                _serializedObject.Update();
            }

            var property = _serializedObject.FindProperty("Sheets");

            EditorGUILayout.PropertyField(property, new GUIContent("Sheets"), true);

            if (property.isArray)
            {
                property.Next(true);
                property.Next(true);

                var length = property.intValue;

                property.Next(true);
                
                Settings.Sheets.Clear();

                var lastIndex = length - 1;

                for (var i = 0; i < length; i++)
                {
                    Settings.Sheets.Add(new Sheet { Name = property.FindPropertyRelative("Name").stringValue, Id = property.FindPropertyRelative("Id").longValue});

                    if (i < lastIndex)
                    {
                        property.Next(false);
                    }
                }
            }

            _serializedObject.ApplyModifiedProperties();
        }
    }
}