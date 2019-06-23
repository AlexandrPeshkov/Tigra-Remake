using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimbreGameBase : ProsodyGameBase
{
    protected bool isTimbreOn = false;
    public float timbreTime = 0.0f;

    private float temp;
    private float timbreDelay = 0.0f;
    private float timbreDelayMax = 0.8f;
    private bool timbreTimeDelayEnabled = false;

    protected float timbreLevel = 0.0f;

    public override void Start()
    {
        base.Start();

        submenuIndex = 3;
    }

    protected override void GameStarted()
    {
        MicrophoneCaptureManager.instance.BeginCapture();
    }

    protected override void ShowWinPanel()
    {
        MicrophoneCaptureManager.instance.StopCapture();
        base.ShowWinPanel();
    }

    public override void GoToMainMenu()
    {
        MicrophoneCaptureManager.instance.StopCapture();
        base.GoToMainMenu();
    }

    public override void FixedUpdate()
    {
        if (isTimbreOn)
        {
            timbreTime += Time.deltaTime;
            timbreDelay += Time.deltaTime;
        }
    }

    public override void Update()
    {
        if (isStarted)
        {
            float f = MicrophoneCaptureManager.instance.GetArithmeticMean();
            if (f > 10.0f)
            {
                if (!isTimbreOn)
                {
                    isTimbreOn = true;
                    timbreTime = 0.0f;
                    TimbreOn();
                }
                else
                {
                    timbreLevel = f;
                }
            }
            else
            {
                if (isTimbreOn)
                {
                    if (timbreTimeDelayEnabled)
                    {
                        if (timbreDelay >= timbreDelayMax)
                        {
                            timbreTimeDelayEnabled = false;
                            timbreDelay = 0.0f;

                            isTimbreOn = false;
                            timbreTime = 0.0f;
                            TimbreOff();
                        }
                    }
                    else
                    {
                        timbreTimeDelayEnabled = true;
                        timbreDelay = 0.0f;
                    }
                }
            }
        }
    }

    protected virtual void TimbreOn()
    {

    }

    protected virtual void TimbreOff()
    {

    }
}
