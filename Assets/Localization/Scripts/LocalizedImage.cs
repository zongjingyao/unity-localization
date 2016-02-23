using UnityEngine;
using UnityEngine.UI;

namespace Localization
{
    [RequireComponent(typeof(Image))]
    [AddComponentMenu("Localization/LocalizedImage")]
    [ExecuteInEditMode]
    public class LocalizedImage : MonoBehaviour
    {
        [HideInInspector]
        public string Key;

        public void OnLanguageChanged()
        {
            GetComponent<Image>().sprite = LocalizationManager.Instance.GetSprite(Key);
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

