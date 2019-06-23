using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicrophoneSettingsMenu : MonoBehaviour {
    public Dropdown micDropdown;
    public RectTransform microphoneVolumeGraph;
    public Slider sensitivitySlider;
    public float currentWidth = 0;
    public float currentWidthKX = 0;
    public float currentMicMean = 0;
    public float currentSensitivity = 0;
    public float wd = 1;

    private MicrophoneCaptureManager micCM;
    private float micVolumeMaxWidth;
    private readonly float micVolumeMax = 100.0f;
    private float prevSensitivityValue;

    void Awake()
    {
        micCM = MicrophoneCaptureManager.instance;

        currentSensitivity = PlayerPrefs.GetFloat("MicrophoneSensitivity", 20);
    }

    void Start () {
        List<string> l = new List<string>(Microphone.devices);
        micDropdown.ClearOptions();
        micDropdown.AddOptions(l);

        micVolumeMaxWidth = microphoneVolumeGraph.gameObject.transform.parent.gameObject.GetComponent<RectTransform>().rect.width;
	}

    private float CalcCurrentVolumeWidth()
    {
        float micAMean = micCM.GetArithmeticMeanWithoutSensitivity();
        float kx = micAMean / micVolumeMax;

        currentMicMean = micAMean;
        currentWidthKX = kx;
        //currentSensitivity = CalcSensiticity(currentSensitivity);
        return (-micVolumeMaxWidth) + Mathf.Clamp(micVolumeMaxWidth * kx * CalcSensiticity(currentSensitivity), 0.01f, micVolumeMaxWidth);
    }

    private float CalcSensiticity(float value)
    {
        if (value >= 20)
        {
            return (value - 10) / 10.0f;
        }
        else
        {
            return value / 20.0f;
        }
    }

    private float GetSensitivity()
    {
        return currentSensitivity;
    }

    public void OnSliderValueChanged()
    {
        currentSensitivity = sensitivitySlider.value;
        micCM.sensitivity = CalcSensiticity(currentSensitivity);
    }

    void Update () {
        Vector2 sd = microphoneVolumeGraph.sizeDelta;
        float k = CalcCurrentVolumeWidth();

        sd.x = Mathf.Lerp(sd.x, k, 0.02f);
        currentWidth = sd.x;
        microphoneVolumeGraph.sizeDelta = sd;

    }

    public void Show()
    {
        prevSensitivityValue = currentSensitivity;
        sensitivitySlider.value = currentSensitivity;

        micCM.BeginCapture();
    }

    public void Hide()
    {
        currentSensitivity = prevSensitivityValue;
        micCM.sensitivity = CalcSensiticity(currentSensitivity);
        gameObject.SetActive(false);
    }

    public void HideAndSave()
    {
        PlayerPrefs.SetFloat("MicrophoneSensitivity", currentSensitivity);
        gameObject.SetActive(false);
    }
}
