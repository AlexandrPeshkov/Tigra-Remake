using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Audio.Subtitles
{
    /// <summary>
    /// Сервис субтитров
    /// </summary>
    public class SubtitlesService : MonoBehaviour
    {
        public static SubtitlesService Instance;

        private SubtitlesLabel subtitlesLabelPrefab;

        private readonly string prefabName = "SubtitlesPrefab";

        private Dictionary<PhraseCategory, Color> phraseColors;

        private Dictionary<PhraseCategory, string[]> phraseCategories;

        private string[] Subtitles;

        private SubtitlesLabel currentSubtitles;

        private void Awake()
        {
            Instance = this;
            Construct();
        }

        private void Construct()
        {
            string assetPath = "Assets" + "/" + "Subtitles" + "/" + prefabName + ".prefab";
            subtitlesLabelPrefab = AssetDatabase.LoadAssetAtPath(assetPath, typeof(SubtitlesLabel)) as SubtitlesLabel;

            phraseColors = new Dictionary<PhraseCategory, Color>()
            {
                { PhraseCategory.Info, new Color(0.9549953f, 1f, 0.1462264f)},
                { PhraseCategory.Bad, new Color(0.745283f, 0.054341f, 0)},
                { PhraseCategory.Success, new Color(0, 0.7264151f, 0.04891533f)}
            };

            string[] goods = new string[]
            {
                    "уминца",
                    "молодец",
                    "здорово",
                    "отллично",
                    "замечательно",
                    "у тебя получилось"
            };

            string[] bads = new string[]
            {
                "нет",
                "попробуй еще раз",
                "не унывай",
                "неверно",
                "неправильно"
            };

            phraseCategories = new Dictionary<PhraseCategory, string[]>();

            phraseCategories.Add(PhraseCategory.Success, goods);
            phraseCategories.Add(PhraseCategory.Bad, bads);
        }

        /// <summary>
        /// Показать субтитры c задержкой. 
        /// Общие фразы - ShowSubtitles("молодец.wav")
        /// Для конкретной игры ShowSubtitles("вступление 1.wav", Canvas, new string[] { "меню фонематика", "игра животные"});
        /// </summary>
        /// <param name="audioFileName">Имя аудиофайла + имя игры</param>
        /// <param name="labelParent">Контейнер для label</param>
        /// <param name="breadCrumbs">Путь из каталогов меню\игра\подкаталог</param>
        /// <returns></returns>
        public void ShowSubtitles(string audioFileName, Transform labelParent = null, string[] breadCrumbs = null)
        {
            Clear();
            if (subtitlesLabelPrefab != null)
            {
                audioFileName = audioFileName.Replace(".wav", string.Empty);

                string file = FindSubtitleFile(audioFileName, breadCrumbs);

                if (file != null)
                {
                    string subtitles = ReadFile(file);

                    if (!string.IsNullOrEmpty(subtitles))
                    {
                        SubtitlesLabel label = GameObject.Instantiate(subtitlesLabelPrefab);

                        currentSubtitles = label;

                        if (labelParent != null)
                        {
                            label.transform.SetParent(labelParent);
                        }

                        if (label != null)
                        {
                            Color color = ResolveColor(audioFileName);
                            int fontSize = CalcFontSize();
                            label.Construct(subtitles, color, fontSize);
                        }

                        StartCoroutine(LongDelete(subtitles.Length));

                    }
                }
            }
        }

        IEnumerator LongDelete(int textLength)
        {
            float delay = textLength / 7f;
            float minDelay = 4.5f;
            yield return new WaitForSeconds(Mathf.Max(delay, minDelay));
            Clear();
        }

        private void Clear()
        {
            if (currentSubtitles != null)
            {
                Destroy(currentSubtitles.gameObject);
            }
        }

        private string FindSubtitleFile(string audioFileName, string[] breadCrumbs = null)
        {
            string sourcePath = Application.dataPath + "/Subtitles/Source/";
            string additionalpath = string.Empty;

            string subtitleFile = null;

            List<string> files = new List<string>();

            //если не передали путь игры
            if (breadCrumbs == null)
            {
                additionalpath = "Common/";
            }
            else
            {
                additionalpath = string.Join("/", breadCrumbs);
            }

            sourcePath += additionalpath;

            if (!Directory.Exists(sourcePath)) return null;

            files = Directory.GetFiles(sourcePath, "*.txt").ToList();

            foreach (var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);

                if (name.Trim().Contains(audioFileName.Trim()))
                {
                    subtitleFile = file;
                    break;
                }
            }

            return subtitleFile;
        }

        private string ReadFile(string path)
        {
            string subtitles = null;
            if (File.Exists(path))
            {
                subtitles = File.ReadAllText(path);
            }

            return subtitles;
        }

        private Color ResolveColor(string phrase)
        {
            PhraseCategory category = phraseCategories.Where(d => d.Value.Contains(phrase)).Select(d => d.Key).FirstOrDefault();
            Color color = Color.white;
            phraseColors.TryGetValue(category, out color);
            return color;
        }

        private int CalcFontSize()
        {
            return Screen.height / 27;
        }
    }

    public enum PhraseCategory
    {
        Info,
        Bad,
        Success
    }
}
