using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameTools.AnimatorParameter
{
    [InitializeOnLoad]
    public static class LocalizationManager
    {
        private const string Localization = "Localization";
        private static Dictionary<string, string> localizedTable;

        static LocalizationManager()
        {
            OnLoad();
        }

        public static void Reload()
        {
            localizedTable = null;
            OnLoad();
        }

        private static void OnLoad()
        {
            if (localizedTable != null)
            {
                return;
            }

            if (!TryGetJsonPath(out string jsonPath))
            {
                return;
            }

            try
            {
                var jsonText = File.ReadAllText(jsonPath);
                var allTexts = JsonUtility.FromJson<LanguageContainer>(jsonText);
                localizedTable = allTexts.GetLanguage(Application.systemLanguage);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{nameof(LocalizationManager)}] Failed to parse {Localization} file: {e.Message}");
                localizedTable = null;
            }
        }

        private static bool TryGetJsonPath(out string jsonPath)
        {
            jsonPath = string.Empty;
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(LocalizationManager).Assembly);
            if (packageInfo == null)
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] Assembly not found.");
                return false;
            }

            string packageRoot = packageInfo.resolvedPath;
            jsonPath = Path.Combine(packageRoot, "Editor", "Localization", "Localization.json");
            if (!File.Exists(jsonPath))
            {
                Debug.LogWarning($"[{nameof(LocalizationManager)}] {Localization} file not found.");
                return false;
            }

            return true;
        }

        public static string Get(string key)
            => localizedTable.TryGetValue(key, out var str)
                ? str
                : key;

        [Serializable]
        public class LanguageContainer
        {
            public List<LanguageEntry> English;
            public List<LanguageEntry> ChineseTraditional;

            public Dictionary<string, string> GetLanguage(SystemLanguage language)
            {
                var result = language switch
                {
                    SystemLanguage.ChineseTraditional => ChineseTraditional,
                    SystemLanguage.English => English,
                    _ => English,
                };
                return result.ToDictionary(kv => kv.key, kv => kv.value);
            }
        }

        [Serializable]
        public class LanguageEntry
        {
            public string key;
            public string value;
        }
    }
}