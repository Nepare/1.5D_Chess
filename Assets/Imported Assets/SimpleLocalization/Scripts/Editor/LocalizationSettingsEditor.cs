using UnityEditor;
using UnityEngine;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    [CustomEditor(typeof(LocalizationSettings))]
    public class LocalizationSettingsEditor : UnityEditor.Editor 
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();

            var settings = (LocalizationSettings) target;
            var buttonStyle = new GUIStyle(GUI.skin.button) {fontStyle = FontStyle.Bold, fixedHeight = 30};

            if (GUILayout.Button("↺ Resolve Sheets", buttonStyle))
            {
                settings.ResolveGoogleSheets();
            }

            if (GUILayout.Button("▼ Download Sheets", buttonStyle))
            {
                settings.DownloadGoogleSheets();
            }

            if (GUILayout.Button("❖ Open Google Sheets", buttonStyle))
            {
                settings.OpenGoogleSheets();
            }

            if (GUILayout.Button("★ Leave Review", buttonStyle))
            {
                settings.LeaveReview();
            }
        }
    }
}
