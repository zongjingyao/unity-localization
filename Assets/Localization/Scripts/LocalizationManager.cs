using UnityEngine;
using System.Collections.Generic;
using MiniJSON;

namespace Localization
{
    [ExecuteInEditMode]
    public class LocalizationManager
    {
        public delegate void OnLauguageChangedDelegate();

        private static LocalizationManager _instance;
        public static LocalizationManager Instance
        {
            get 
            {
                TextAsset config = Resources.Load<TextAsset>("LocalizationConfig");
                if (config == null) return null;

                return _instance ?? (_instance = new LocalizationManager()); 
            }
        }

        private SystemLanguage m_currentLanguage;
        private Dictionary<string, string> m_keyValueMap { get; set; }

        public SystemLanguage DefaultLanguage { get; set; }
        public List<SystemLanguage> Languages { get; set; }
        public OnLauguageChangedDelegate OnLauguageChanged { get; set; }

        public SystemLanguage CurrentLanguage
        {
            get { return m_currentLanguage; }
            set 
            {
                if (m_currentLanguage == SystemLanguage.Unknown || m_currentLanguage != value)
                {
                    m_currentLanguage = value;
                    loadLanguageJson();
                    if (OnLauguageChanged != null)
                    {
                        OnLauguageChanged();
                    }
                }
            }
        }

        public string[] LanguagesStr
        {
            get
            {
                string[] strLanguages = new string[Languages.Count];
                int i = 0;
                foreach (SystemLanguage key in Languages)
                {
                    strLanguages[i++] = key.ToString();
                }

                return strLanguages;
            }
        }

        public LocalizationManager()
        {
            m_keyValueMap = new Dictionary<string, string>();
            Languages = new List<SystemLanguage>();
            Init();
        }

        public void Init()
        {
            m_keyValueMap.Clear();
            m_currentLanguage = SystemLanguage.Unknown;
            DefaultLanguage = SystemLanguage.Unknown;
            Languages.Clear();
            LoadConfig();
            if (!Languages.Contains(Application.systemLanguage))
            {
                CurrentLanguage = DefaultLanguage;
            }
            else
            {
                CurrentLanguage = Application.systemLanguage;
            }
        }

        void LoadConfig()
        {
            TextAsset config = Resources.Load<TextAsset>("LocalizationConfig");
            Dictionary<string, object> dic = null;
            if (string.IsNullOrEmpty(config.text))
            {
                dic = new Dictionary<string, object>();
            }
            else
            {
                dic = Json.Deserialize(config.text) as Dictionary<string, object>;
            }
            List<object> languageList = JsonHelper.OptList(dic, "languages");
            for (int i = 0; i < languageList.Count; ++i)
            {
                SystemLanguage tmp = (SystemLanguage)((long)languageList[i]);
                Languages.Add(tmp);
            }
            DefaultLanguage = (SystemLanguage)(JsonHelper.OptInt(dic, "default", (int)SystemLanguage.Unknown));
        }

        void loadLanguageJson()
        {
            string path = m_currentLanguage.ToString() + "/" + m_currentLanguage.ToString();
            TextAsset languageJson = Resources.Load<TextAsset>(path);
            if (languageJson == null)
            {
                return;
            }

            m_keyValueMap.Clear();
            if (string.IsNullOrEmpty(languageJson.text))
            {
                return;
            }

            Dictionary<string, object> dic = Json.Deserialize(languageJson.text) as Dictionary<string, object>;
            foreach (string key in dic.Keys)
            {
                string value = JsonHelper.OptString(dic, key);
                m_keyValueMap.Add(key, value);
            }
        }

        public string GetString(string key, string fallback)
        {
            if (!m_keyValueMap.ContainsKey(key))
            {
                DebugUtils.LogWarning(string.Format("Localization Key Not Found {0}", key));
                return fallback;
            }
            return m_keyValueMap[key];
        }

        public Sprite GetSprite(string key)
        {
            string value = GetString(key, string.Empty);
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            string path = CurrentLanguage.ToString() + "/" + value;
            return Resources.Load<Sprite>(path);
        }
    }
}
