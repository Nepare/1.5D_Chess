using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine.Networking;

namespace Assets.SimpleLocalization.Scripts.Editor
{
    public class LocalizationEditor
    {
        public Dictionary<string, SortedDictionary<string, string>> SheetDictionary = new Dictionary<string, SortedDictionary<string, string>>();
        public List<long> SheetIds = new List<long>();
        public List<string> SheetNames = new List<string>();

        public readonly Dictionary<string, ActionType> KeysActions = new Dictionary<string, ActionType>();
        public readonly Dictionary<string, string> Keys = new Dictionary<string, string>();

        public string CurrentKey;
        public string PrevKey = "";

        private static LocalizationSettings Settings => LocalizationSettings.Instance;
        
        private static string SheetFileName(string sheetName) => AssetDatabase.GetAssetPath(Settings.SaveFolder) + "/" + sheetName + ".csv";
        
        public bool ReadSorted(string sheetName)
        {
            SheetDictionary.Clear();

            var fileName = SheetFileName(sheetName);

            if (!File.Exists(fileName))
            {
                if (EditorUtility.DisplayDialog("Error", $"File not found: {fileName}!\nPlease check your Settings and download sheets.", "Ok"))
                {
                    return false;
                }
            }

            var lines = LocalizationManager.GetLines(File.ReadAllText(fileName));
            var languages = lines[0].Split(',').Select(i => i.Trim()).ToList();

            for (var i = 1; i < languages.Count; i++)
            {
                if (!SheetDictionary.ContainsKey(languages[i]))
                {
                    SheetDictionary.Add(languages[i], new SortedDictionary<string, string>());
                }
            }

            for (var i = 1; i < lines.Count; i++)
            {
                var columns = LocalizationManager.GetColumns(lines[i]);
                var key = columns[0];

                if (key == "") continue;

                for (var j = 1; j < languages.Count; j++)
                {
                    SheetDictionary[languages[j]].Add(key, columns[j]);
                }
            }

            return true;
        }

        public bool IsNewKey(string key)
        {
            return SheetDictionary.FirstOrDefault().Value.ContainsKey(key) && KeysActions.ContainsKey(key) && KeysActions[key] == ActionType.Add;
        }

        public string DeleteKey(string key)
        {
            if (IsNewKey(key))
            {
                KeysActions.Remove(key);
                Keys.Remove(key);
            }
            else
            {
                if (KeysActions.ContainsKey(key))
                {
                    KeysActions[key] = ActionType.Delete;
                }
                else
                {
                    KeysActions.Add(key, ActionType.Delete);
                }
            }

            foreach (var language in SheetDictionary.Keys)
            {
                SheetDictionary[language].Remove(key);
            }

            return "";
        }

        public string AddKey()
        {
            var key = $"New_key_{Guid.NewGuid().ToString().Substring(0, 8)}";

            Keys.Add(key, key);

            foreach (var language in SheetDictionary.Keys)
            {
                SheetDictionary[language].Add(key, "");
            }

            KeysActions.Add(key, ActionType.Add);

            return key;
        }
        
        public void ResetSheet()
        {
            CurrentKey = PrevKey != "" ? Keys[PrevKey] : PrevKey;
            SheetDictionary.Clear();
            Keys.Clear();
            KeysActions.Clear();
            PrevKey = "";
        }

        public void GetAllKeys()
        {
            if (Keys.Count != 0) return;

            var first = SheetDictionary.First();

            foreach (var keys in first.Value.Keys)
            {
                Keys.Add(keys, keys);
            }
        }

        public IEnumerator Translate()
        {
            if (string.IsNullOrEmpty(CurrentKey))
            {
                EditorUtility.DisplayDialog("Error", "Assign the key please!", "Ok");
                yield break;
            }

            var emptyLanguages = SheetDictionary.Keys.Where(i => string.IsNullOrEmpty(SheetDictionary[i][CurrentKey])).ToArray();
            var translatedLanguages = SheetDictionary.Keys.Where(i => !string.IsNullOrEmpty(SheetDictionary[i][CurrentKey])).ToArray();

            if (!emptyLanguages.Any())
            {
                emptyLanguages = SheetDictionary.Keys.ToArray();
            }

            var sourceLanguage = translatedLanguages.Contains("English") ? "English" : translatedLanguages.FirstOrDefault();

            if (translatedLanguages.Length == 0 || sourceLanguage == null)
            {
                EditorUtility.DisplayDialog("Error", "Assign at least one text please!", "Ok");
                yield break;
            }

            var error = "";
            var translatedText = "";

            for (var i = 0; i < emptyLanguages.Length; i++)
            {
                var progress = (float)(i + 1) / emptyLanguages.Length;

                if (EditorUtility.DisplayCancelableProgressBar("Submitting data...", $"[{(int)(100 * progress)}%] [{i + 1}/{emptyLanguages.Length}] ...", progress))
                {
                    yield break;
                }

                yield return TranslateText(GetLangCode(emptyLanguages[i]), SheetDictionary[sourceLanguage][CurrentKey], sourceLanguage,
                    (message, translated) =>
                    {
                        translatedText = translated;
                        error = message;
                    });


                if (!string.IsNullOrEmpty(error))
                {
                    EditorUtility.ClearProgressBar();
                    EditorUtility.DisplayDialog("Error", $"Can't translate data: {error}", "Ok");
                    break;
                }

                if (string.IsNullOrEmpty(CurrentKey) || string.IsNullOrEmpty(translatedText)) continue;

                SheetDictionary[emptyLanguages[i]][CurrentKey] = translatedText;
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Message", "Auto Translate completed!", "Ok");
        }

        public static IEnumerator TranslateText(string targetLang, string sourceText, string sourceLanguage, Action<string, string> callback)
        {
            if (string.IsNullOrEmpty(sourceText)) yield break;

            var url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=" + GetLangCode(sourceLanguage) + "&tl=" + targetLang + "&dt=t&q=" + UnityWebRequest.EscapeURL(sourceText);
            var request = UnityWebRequest.Get(url);

            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            var error = request.error;
            var result = "";

            if (request.error == null)
            {
                var parsedTexts = request.downloadHandler.text;
                var start = parsedTexts.IndexOf('"') + 1;
                var length = parsedTexts.IndexOf('"', start + 1) - start;

                result = parsedTexts.Substring(start, length);
            }

            request.Dispose();
            callback(error, result);
        }

        public static string GetLangCode(string language)
        {
            return language switch
            {
                "Afrikaans" => "af",
                "Albanian" => "sq",
                "Amharic" => "am",
                "Arabic" => "ar",
                "Armenian" => "hy",
                "Azerbaijani" => "az",
                "Basque" => "eu",
                "Belarusian" => "be",
                "Bengali" => "bn",
                "Bosnian" => "bs",
                "Bulgarian" => "bg",
                "Catalan" => "ca",
                "Cebuano" => "ceb",
                "Chinese" => "zh",
                "Corsican" => "co",
                "Croatian" => "hr",
                "Czech" => "cs",
                "Danish" => "da",
                "Dutch" => "nl",
                "English" => "en",
                "Esperanto" => "eo",
                "Estonian" => "et",
                "Finnish" => "fi",
                "French" => "fr",
                "Frisian" => "fy",
                "Galician" => "gl",
                "Georgian" => "ka",
                "German" => "de",
                "Greek" => "el",
                "Gujarati" => "gu",
                "Haitian Creole" => "ht",
                "Hausa" => "ha",
                "Hawaiian" => "haw",
                "Hebrew" => "he",
                "Hindi" => "hi",
                "Hmong" => "hmn",
                "Hungarian" => "hu",
                "Icelandic" => "is",
                "Igbo" => "ig",
                "Indonesian" => "id",
                "Irish" => "ga",
                "Italian" => "it",
                "Japanese" => "ja",
                "Javanese" => "jw",
                "Kannada" => "kn",
                "Kazakh" => "kk",
                "Khmer" => "km",
                "Korean" => "ko",
                "Kurdish" => "ku",
                "Kyrgyz" => "ky",
                "Lao" => "lo",
                "Latin" => "la",
                "Latvian" => "lv",
                "Lithuanian" => "lt",
                "Luxembourgish" => "lb",
                "Macedonian" => "mk",
                "Malagasy" => "mg",
                "Malay" => "ms",
                "Malayalam" => "ml",
                "Maltese" => "mt",
                "Maori" => "mi",
                "Marathi" => "mr",
                "Mongolian" => "mn",
                "Myanmar" => "my",
                "Nepali" => "ne",
                "Norwegian" => "no",
                "Nyanja" => "ny",
                "Pashto" => "ps",
                "Persian" => "fa",
                "Polish" => "pl",
                "Portuguese" => "pt",
                "Punjabi" => "pa",
                "Romanian" => "ro",
                "Russian" => "ru",
                "Samoan" => "sm",
                "Scots Gaelic" => "gd",
                "Serbian" => "sr",
                "Sesotho" => "st",
                "Shona" => "sn",
                "Sindhi" => "sd",
                "Sinhala" => "si",
                "Slovak" => "sk",
                "Slovenian" => "sl",
                "Somali" => "so",
                "Spanish" => "es",
                "Sundanese" => "su",
                "Swahili" => "sw",
                "Swedish" => "sv",
                "Tagalog" => "tl",
                "Tajik" => "tg",
                "Tamil" => "ta",
                "Telugu" => "te",
                "Thai" => "th",
                "Turkish" => "tr",
                "Ukrainian" => "uk",
                "Urdu" => "ur",
                "Uzbek" => "uz",
                "Vietnamese" => "vi",
                "Welsh" => "cy",
                "Xhosa" => "xh",
                "Yiddish" => "yi",
                "Yoruba" => "yo",
                "Zulu" => "zu",
                _ => "en"
            };
        }
    }
}
