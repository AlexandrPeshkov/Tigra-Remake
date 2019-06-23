using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneCaptureManager : MonoBehaviour
{
    private static int samplesCount = 64;
    public static MicrophoneCaptureManager instance = null;
    public FFTWindow fftMode;
    private AudioSource aSource;
    private float[] samples = new float[samplesCount];
    [HideInInspector]
    public bool captureStarted = false;
    private int activeMicrophone = -1;
    private string activeMicrophoneName = "";
    public int latency = 0;
    public int captureTimeoutSeconds = 2;
    [HideInInspector]
    public float sensitivity = 1;
    private float capturetime = 0.0f;

    //arithmetic equalization
    public List<float> arithmEqual = new List<float>();
    private int arithmEqualMax = 7;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        aSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
        SetActiveMicrophone(0);
    }

    private void ResetSamples()
    {
        samples = new float[samplesCount];
        arithmEqual.Clear();
    }

    public void SetActiveMicrophone(int value)
    {
        if (Microphone.devices.Length > 0)
        {
            activeMicrophone = Mathf.Min(value, Microphone.devices.Length - 1);
            activeMicrophoneName = Microphone.devices[activeMicrophone];
        }
        else
        {
            activeMicrophone = -1;
        }
    }

    public int GetActiveMicrophone()
    {
        return activeMicrophone;
    }

    public void BeginCapture()
    {
        if (activeMicrophone == -1)
        {
            SetActiveMicrophone(0);
        }
        if (!captureStarted && (GetActiveMicrophone() >= 0))
        {
            StartCoroutine(StartRecording());

            //return true;
        }

        //return false;
    }

    public void StopCapture()
    {
        ResetSamples();
        capturetime = 0.0f;
        aSource.Stop();
        aSource.clip = null;
        //Microphone.End(activeMicrophoneName);
        Microphone.End(null);
        captureStarted = false;
    }

    public float[] GetSamples()
    {
        return samples;
    }

    private float GetArithmeticValue()
    {
        float k = 0;
        float[] ar = GetSamples();
        float[] samples = new float[ar.Length];
        for (int i = 0; i < ar.Length; i++)
        {
            samples[i] = ar[i];
        }

        float temp;
        for (int i = 0; i < samples.Length; i++)
        {
            for (int j = i + 1; j < samples.Length; j++)
            {
                if (samples[i] > samples[j])
                {
                    temp = samples[i];
                    samples[i] = samples[j];
                    samples[j] = temp;
                }
            }
        }
        for (int i = samples.Length - 1; i > samples.Length - 5; i--)
        {
            k += samples[i] * 1000.0f;
        }

        float middleArithm = k / 5.0f;

        if (arithmEqual.Count >= arithmEqualMax)
        {
            arithmEqual.RemoveAt(0);
        }
        arithmEqual.Add(middleArithm);

        k = 0.0f;
        for (int i = 0; i < arithmEqual.Count; i++)
        {
            k += arithmEqual[i];
        }
        float result = k / arithmEqual.Count;

        return result;
    }

    public float GetArithmeticMean()
    {
        return GetArithmeticValue() * sensitivity;
    }

    public float GetArithmeticMeanWithoutSensitivity()
    {
        return GetArithmeticValue();
    }

    void Update()
    {
        if (captureStarted && aSource.isPlaying)
        {
            capturetime += Time.deltaTime;
            //if (capturetime > 2.5f)
            {
                aSource.GetSpectrumData(samples, 0, fftMode);
            }
        }
    } 

    IEnumerator StartRecording()
    {
        yield return new WaitForSeconds(0.1f);
        Microphone.End(null);
        aSource.clip = Microphone.Start(activeMicrophoneName, true, 1, 44100);

        bool fl = true;
        float beginTime = System.DateTime.Now.Second;
        while (!(Microphone.GetPosition(null) > 0))
        {
            if (System.DateTime.Now.Second - beginTime > 2)
            {
                fl = false;
                break;
            }
        }
        if (fl)
        {
            ResetSamples();
            aSource.Play();
            captureStarted = true;
            capturetime = 0.0f;
        }
    }
}
