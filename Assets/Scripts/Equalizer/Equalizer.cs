using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equalizer : MonoBehaviour
{
    public GameObject linePrefab;
    public int lineCount = 13;
    private GameObject[] lines;
    public float minHeight = 1.0f;
    public float maxHeight = 5.0f;
    private GameObject redLine;

    private MicrophoneCaptureManager micCapture;

    void Start()
    {
        micCapture = MicrophoneCaptureManager.instance;

        CreateLines();

        //micCapture.SetActiveMicrophone(0);
        //micCapture.BeginCapture();
    }

    private void CreateLines()
    {
        lines = new GameObject[lineCount];
        for (int i = 0; i < lines.Length; i++)
        {
            GameObject line = Instantiate(linePrefab);
            line.transform.SetParent(this.transform, false);
            Vector2 pos = new Vector2(i * 50, 0);
            line.GetComponent<RectTransform>().anchoredPosition = pos;
            lines[i] = line;
        }

        redLine = Instantiate(linePrefab);
        redLine.transform.SetParent(this.transform, false);
        Vector2 pos1 = new Vector2(13 * 50, 0);
        redLine.GetComponent<RectTransform>().anchoredPosition = pos1;
        redLine.GetComponent<Image>().color = Color.red;
    }

    void Update()
    {
        UpdateLines();
    }

    private void UpdateLines()
    {
        if (micCapture.captureStarted)
        {
            float[] samples = micCapture.GetSamples();

            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 v = lines[i].GetComponent<RectTransform>().rect.size;
                v.y = Mathf.Lerp(v.y, samples[i] * 1000.0f, 0.5f);
                lines[i].GetComponent<RectTransform>().sizeDelta = v;
            }

            Vector2 v1 = redLine.GetComponent<RectTransform>().rect.size;
            float f = micCapture.GetArithmeticMean();
            v1.y = Mathf.Lerp(v1.y, f, 0.5f);
            redLine.GetComponent<RectTransform>().sizeDelta = v1;
        }
    }
}
