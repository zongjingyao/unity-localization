using UnityEditor;

namespace Localization
{
    [CustomEditor(typeof(LocalizedText), true)]
    public class LocalizedTextEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocalizedText Target = target as LocalizedText;

            string key = EditorGUILayout.TextField("Key", Target.Key);
            var manager = LocalizationManager.Instance;
            if (manager == null)
            {
                if (!key.Equals(Target.Key))
                {
                    Target.Key = key;
                }
                EditorGUILayout.LabelField("Error ", "LocalizationManager Not Found");
            }
            else
            {
                if (!key.Equals(Target.Key))
                {
                    Target.Key = key;
                    Target.OnLanguageChanged();
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }

    [CustomEditor(typeof(LocalizedImage), true)]
    public class LocalizedImageEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LocalizedImage Target = target as LocalizedImage;

            string key = EditorGUILayout.TextField("Key", Target.Key);
            var manager = LocalizationManager.Instance;
            if (manager == null)
            {
                if (!key.Equals(Target.Key))
                {
                    Target.Key = key;
                }
                EditorGUILayout.LabelField("Error ", "LocalizationManager Not Found");
            }
            else
            {
                if (!key.Equals(Target.Key))
                {
                    Target.Key = key;
                    Target.OnLanguageChanged();
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
