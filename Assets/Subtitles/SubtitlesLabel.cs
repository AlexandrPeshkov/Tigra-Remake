using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Audio.Subtitles
{
    public class SubtitlesLabel : MonoBehaviour
    {
        public Text Label;

        public void Construct(string text, Color color, int fontSize)
        {
            if (Label != null)
            {
                Label.text = text;
                Label.color = color;
                Label.fontSize = fontSize;
            }
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.offsetMin = new Vector2(0, 0);
                rectTransform.offsetMax = new Vector2(0, 0);
            }
        }
    }
}
