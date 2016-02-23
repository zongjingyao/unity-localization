using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text;
using MiniJSON;

namespace Localization
{
    public class LocalizationConfigWindow : EditorWindow
    {
        private static readonly string FOLDER_NAME = "Localization";
        private static readonly string CONFIG_FILE_NAME = "LocalizationConfig.json";
        private static readonly string RESOURCES_FOLDER_PATH = Application.dataPath + "/" + FOLDER_NAME + "/Resources";
        private static readonly string CONFIG_FILE_PATH = RESOURCES_FOLDER_PATH + "/" + CONFIG_FILE_NAME;

        [MenuItem ("Window/LocalizationConfig")]
        static void ShowConfigWindow()
        {
            LocalizationConfigWindow window = EditorWindow.GetWindow<LocalizationConfigWindow>("LocalizationConfig");
            window.Show();
        }

        private bool m_inited = false;
        private Vector2 m_scrollPostion;
        private SystemLanguage m_selectedLanguage = SystemLanguage.Unknown;
        private static int m_currentLanguageIdx;

        void Awake()
        {
            if (Directory.Exists(RESOURCES_FOLDER_PATH) && File.Exists(CONFIG_FILE_PATH))
            {
                m_inited = true;
            }
        }

        void OnGUI()
        {
            if (!m_inited)
            {
                if (GUILayout.Button("Init"))
                {
                    InitLocalization();
                }
                return;
            }
            if (LocalizationManager.Instance == null) return;

            List<SystemLanguage> languages = LocalizationManager.Instance.Languages;
            if (languages.Count > 0)
            {
                m_scrollPostion = GUILayout.BeginScrollView(m_scrollPostion);
                SystemLanguage[] lanArray = languages.ToArray();
                for (int i = 0; i < lanArray.Length; ++i)
                {
                    SystemLanguage lan = lanArray[i];
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(lan.ToString(), GUILayout.MinWidth(60));
                    if (GUILayout.Button("Delete"))
                    {
                        DeleteLanguage(lan);
                    }
                    if (GUILayout.Button("Edit"))
                    {
                        EditLanguage(lan);
                    }
                    if (GUILayout.Toggle(LocalizationManager.Instance.DefaultLanguage == lan, "Default"))
                    {
                        SetDefaultLanguage(lan);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }

            GUILayout.BeginHorizontal();
            m_selectedLanguage = (SystemLanguage)EditorGUILayout.EnumPopup(m_selectedLanguage);
            if (GUILayout.Button("Add language"))
            {
                CreateLanguage(m_selectedLanguage);
            }
            GUILayout.EndHorizontal();

            if (languages.Count > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Current language");
                string[] languagesStr = LocalizationManager.Instance.LanguagesStr;
                m_currentLanguageIdx = languages.IndexOf(LocalizationManager.Instance.CurrentLanguage);
                int tmpIdx = EditorGUILayout.Popup(m_currentLanguageIdx, languagesStr);
                if (tmpIdx != m_currentLanguageIdx)
                {
                    m_currentLanguageIdx = tmpIdx;
                    LocalizationManager.Instance.CurrentLanguage = languages[m_currentLanguageIdx];
                }

                GUILayout.EndHorizontal();
            }
        }

        private void InitLocalization()
        {
            if (!Directory.Exists(RESOURCES_FOLDER_PATH))
            {
                Directory.CreateDirectory(RESOURCES_FOLDER_PATH);
            }
            if (!File.Exists(CONFIG_FILE_PATH))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                List<string> list = new List<string>();
                dic.Add("languages", list);
                dic.Add("default", (int)SystemLanguage.Unknown);

                byte[] bytes = Encoding.UTF8.GetBytes(Json.Serialize(dic));
                FileStream fs = File.Create(CONFIG_FILE_PATH);
                fs.Write(bytes, 0, bytes.Length);
                fs.Close();
            }

            if (Directory.Exists(RESOURCES_FOLDER_PATH) && File.Exists(CONFIG_FILE_PATH))
            {
                m_inited = true;
            }
        }

        private void DeleteLanguage(SystemLanguage language)
        {
            Dictionary<string, object> configDic = GetConfigDic();
            ((List<object>)configDic["languages"]).Remove((long)language);
            
            SaveToConfigFile(configDic);

            LocalizationManager.Instance.Languages.Remove(language);

            FileUtil.DeleteFileOrDirectory(RESOURCES_FOLDER_PATH + "/" + language.ToString());
            FileUtil.DeleteFileOrDirectory(RESOURCES_FOLDER_PATH + "/" + language.ToString() + ".meta");
        }

        private void EditLanguage(SystemLanguage language)
        {
            ShowNotification(new GUIContent("Coming soon..."));
        }

        private void SetDefaultLanguage(SystemLanguage language)
        {
            if (language != LocalizationManager.Instance.DefaultLanguage)
            {
                Dictionary<string, object> configDic = GetConfigDic();
                configDic["default"] = (int)language;
                SaveToConfigFile(configDic);
                LocalizationManager.Instance.DefaultLanguage = language;
            }
        }

        private void CreateLanguage(SystemLanguage language)
        {
            if (language == SystemLanguage.Unknown)
            {
                return;
            }
            if (LocalizationManager.Instance.Languages.Contains(language))
            {
                ShowNotification(new GUIContent("Language exists !"));
                return;
            }

            string strLanguage = language.ToString();
            string folderPath = RESOURCES_FOLDER_PATH + "/" + strLanguage;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = folderPath + "/" + strLanguage + ".json";
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }

            Dictionary<string, object> configDic = GetConfigDic();
            ((List<object>)configDic["languages"]).Add((int)language);
            SaveToConfigFile(configDic);
            LocalizationManager.Instance.Languages.Add(language);

            if (LocalizationManager.Instance.Languages.Count == 1)
            {
                SetDefaultLanguage(language);
            }
        }

        private Dictionary<string, object> GetConfigDic()
        {
            string config = File.ReadAllText(CONFIG_FILE_PATH);
            DebugUtils.Log("GetConfigData:\t" + config);
            Dictionary<string, object> dic = Json.Deserialize(config) as Dictionary<string, object>;
            
            return dic;
        }

        private void SaveToConfigFile(Dictionary<string, object> dic)
        {
            string content = Json.Serialize(dic);
            DebugUtils.Log("SaveToConfigFile:\t" + content);
            File.WriteAllText(CONFIG_FILE_PATH, content);
        }

    }
}
