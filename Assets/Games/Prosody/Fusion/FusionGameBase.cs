using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionGameBase : ProsodyGameBase
{
    protected bool isFusionOn = false;
    public float fusionTime = 0.0f;

    private float temp;
    private float fusionDelay = 0.0f;
    private float fusionDelayMax = 0.4f;
    private bool fusionTimeDelayEnabled = false;

    public override void Start()
    {
        base.Start();

        submenuIndex = 1;
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
        if (isFusionOn)
        {
            fusionTime += Time.deltaTime;
            fusionDelay += Time.deltaTime;
        }
    }

    public override void Update()
    {
        if (isStarted)
        {
            float f = MicrophoneCaptureManager.instance.GetArithmeticMean();
            temp = f;
            if (f > 3.0f)
            {
                if (!isFusionOn)
                {
                    isFusionOn = true;
                    fusionTime = 0.0f;
                    FusionOn();
                }
            }
            else
            {
                if (isFusionOn)
                {
                    if (fusionTimeDelayEnabled)
                    {
                        if (fusionDelay >= fusionDelayMax)
                        {
                            fusionTimeDelayEnabled = false;
                            fusionDelay = 0.0f;

                            isFusionOn = false;
                            fusionTime = 0.0f;
                            FusionOff();
                        }
                    }
                    else
                    {
                        fusionTimeDelayEnabled = true;
                        fusionDelay = 0.0f;
                    }
                }
            }
        }
    }

    protected virtual void FusionOn()
    {

    }

    protected virtual void FusionOff()
    {

    }
}
