using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmSystem : MonoBehaviour
{
    public GameObject samplePrefab;
    public List<RhythmSample> samplesTemplate;
    public GameObject samplesParent;
    public GameObject centerSensor;
    public float speed = 0.02f;
    public RectTransform startPoint;
    public RectTransform finishPoint;
    [HideInInspector]
    public Action onBeepIntersect;

    private bool isStarted;
    private int activeSample = 0;
    private float lastTime;
    private List<GameObject> samples = new List<GameObject>();
    private List<GameObject> beepSamples = new List<GameObject>();
    private bool isBeeped = false;
    private bool isStillBeeped = false;
    private bool beepReleased = false;

    void Start ()
    {
        beepReleased = true;
    }

    void FixedUpdate()
    {
        if (isStarted)
        {
            CheckForAddSample();
            CheckForRemoveSample();
        }
    }

    void Update ()
    {
        if (isStarted)
        {
            float offset = Time.deltaTime * speed;
            for (int i = 0; i < samples.Count; i++)
            {
                Vector2 v = samples[i].GetComponent<RectTransform>().anchoredPosition;
                v.x -= offset;
                samples[i].GetComponent<RectTransform>().anchoredPosition = v;
            }
            for (int i = 0; i < beepSamples.Count; i++)
            {
                Vector2 v = beepSamples[i].GetComponent<RectTransform>().anchoredPosition;
                v.x -= offset;
                beepSamples[i].GetComponent<RectTransform>().anchoredPosition = v;
            }
        }
        isStillBeeped = false;
        CheckMicropnoneInput();
        CheckForIntersect();
        isBeeped = false;
    }

    private void CheckForAddSample()
    {
        if (Time.time - lastTime >= samplesTemplate[activeSample].offsetTime)
        {
            CreateSample();
        }
    }

    private void CheckForRemoveSample()
    {
        for (int i = samples.Count - 1; i >= 0; i--)
        {
            if (samples[i].transform.position.x < finishPoint.transform.position.x)
            {
                Destroy(samples[i]);
                samples.RemoveAt(i);
            }
        }
        for (int i = beepSamples.Count - 1; i >= 0; i--)
        {
            if (beepSamples[i].transform.position.x < finishPoint.transform.position.x)
            {
                Destroy(beepSamples[i]);
                beepSamples.RemoveAt(i);
            }
        }
    }

    private void CheckForIntersect()
    {
        for (int i = 0; i < samples.Count; i++)
        {
            if (!samples[i].GetComponent<RhythmSampleInfo>().beeped)
            {
                Vector2 v = samples[i].GetComponent<RectTransform>().anchoredPosition;
                RectTransform rt = samples[i].GetComponent<RectTransform>();
                if ((rt.anchoredPosition.x - ((rt.rect.width / 2.0f) * rt.localScale.x) < 0) &&
                    (rt.anchoredPosition.x + ((rt.rect.width / 2.0f) * rt.localScale.x) > 0))
                {

                    Color color = new Color();
                    if (isBeeped)
                    {
                        ColorUtility.TryParseHtmlString("#FFFB58", out color);
                        samples[i].GetComponent<RhythmSampleInfo>().beeped = true;

                        DoBeepIntersect();
                    }
                    else
                    {
                        ColorUtility.TryParseHtmlString("#FF6060", out color);
                    }
                    samples[i].GetComponent<Image>().color = color;
                }
                else
                {
                    samples[i].GetComponent<Image>().color = Color.white;
                }
            }
        }
    }

    private void DoBeepIntersect()
    {
        onBeepIntersect();
    }

    private void CreateSample()
    {
        lastTime = Time.time;
        activeSample = (activeSample + 1 > samplesTemplate.Count - 1) ? 0 : activeSample + 1;
        GameObject sample = Instantiate(samplePrefab);
        sample.transform.SetParent(samplesParent.transform, true);
        sample.transform.position = startPoint.position;
        Vector2 sc = sample.GetComponent<RectTransform>().localScale;
        sc = sc * 2.5f;
        sample.GetComponent<RectTransform>().localScale = sc;
        samples.Add(sample);
    }

    private void CreateBeepSample()
    {
        if (isStarted)
        {
            GameObject sample = Instantiate(samplePrefab);
            sample.transform.SetParent(samplesParent.transform, true);
            sample.transform.position = samplesParent.gameObject.transform.position;
            Color color = new Color();
            ColorUtility.TryParseHtmlString("#6EFF73", out color);
            sample.GetComponent<Image>().color = color;
            beepSamples.Add(sample);
        }
    }

    public void BeginRhythm()
    {
        isStarted = !isStarted;

        MicrophoneCaptureManager.instance.BeginCapture();
    }

    public void StopRhythm()
    {
        isStarted = false;

        MicrophoneCaptureManager.instance.StopCapture();
    }

    public void Beep()
    {
        isStillBeeped = true;
        if (beepReleased)
        {
            isBeeped = true;
            beepReleased = false;

            CreateBeepSample();

            StartCoroutine(ResetkeyPressed());
        }
    }

    private void CheckMicropnoneInput()
    {
        if (isStarted)
        {
            float f = MicrophoneCaptureManager.instance.GetArithmeticMean();
            if (f > 3.0f)
            {
                Beep();
            }
        }
    }

    [System.Serializable]
    public struct RhythmSample
    {
        public float offsetTime;
    }

    IEnumerator ResetkeyPressed()
    {
        yield return new WaitForSeconds(0.3f);

        if (!isStillBeeped)
        {
            beepReleased = true;
        }
        else
        {
            StartCoroutine(ResetkeyPressed());
        }

    }
}