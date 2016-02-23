using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Text))]
    [AddComponentMenu("Localization/LocalizedText")]
    [ExecuteInEditMode]
    public class LocalizedText : MonoBehaviour
    {
        [HideInInspector]
        public string Key;

        public void OnLanguageChanged()
        {
            Text label = GetComponent<Text>();
            label.text = LocalizationManager.Instance.GetString(Key, string.Empty);
        }

        void Awake()
        {
            LocalizationManager.Instance.OnLauguageChanged += OnLanguageChanged;
            OnLanguageChanged();
        }

        public void OnDestroy()
        {
            LocalizationManager.Instance.OnLauguageChanged -= OnLanguageChanged;
        }
    }
}
