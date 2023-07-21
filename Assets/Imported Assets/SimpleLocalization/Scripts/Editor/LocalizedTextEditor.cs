using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    /// <summary>
    /// Adds "Sync" button to LocalizationSync script.
    /// </summary>
    [CustomEditor(typeof(LocalizedText))]
    public class LocalizedTextEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var component = (LocalizedText) target;

            if (GUILayout.Button("Localization Editor"))
            {
                LocalizationEditorWindow.OpenEditor();
            }
        }
    }
}