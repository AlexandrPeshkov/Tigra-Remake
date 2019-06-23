using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Breath_Game2 : ProsodyGameBase
{
    public RainCloud[] rainClouds;
    public CactusFlower[] flowers;
    public float temp;

    private int activeCloudIndex = 0;
    private List<RainCloud> activeClouds = new List<RainCloud>();
    private int activeFlowerIndex = 0;

    public override void Start()
    {
        ResetClouds();
        AddNewCloud();

        for (int i = 0; i < flowers.Length; i++)
        {
            flowers[i].onBloomFinished += OnFlowerBloomFinished;
        }

        base.Start();
    }

    private void ResetClouds()
    {
        for (int i = 0; i < rainClouds.Length; i++)
        {
            rainClouds[i].ResetPosition();
            rainClouds[i].onNextCloud += OnNextCloud;
            rainClouds[i].onMoveFinished += CloudMoveFinished;
        }
    }

    private void AddNewCloud() 
    {
        if (activeCloudIndex < rainClouds.Length)
        {
            rainClouds[activeCloudIndex].gameObject.SetActive(true);
            activeClouds.Add(rainClouds[activeCloudIndex]);

            activeCloudIndex++;
        }
    }

    protected override void GameStarted()
    {
        MicrophoneCaptureManager.instance.BeginCapture();
    }

    public override void Update()
    {
        if (isStarted)
        {
            if (MicrophoneCaptureManager.instance != null)
            {
                float f = MicrophoneCaptureManager.instance.GetArithmeticMean() ;
                temp = f;
                if (f > 10.0f)
                {
                    BeginMoveClouds(f);
                }
                else
                {
                    EndMoveClouds();
                }
            }
        }
    }

    private void BeginMoveClouds(float mult)
    {
        foreach (RainCloud cloud in activeClouds)
        {
            cloud.MoveBegin(mult);
        }
    }

    private void EndMoveClouds()
    {
        foreach (RainCloud cloud in activeClouds)
        {
            cloud.MoveEnd();
        }
    }

    private void OnNextCloud()
    {
        if (activeCloudIndex != 1)
        {
            BloomFlower();
        }
        AddNewCloud();
    }

    private void CloudMoveFinished(RainCloud cloud)
    {
        cloud.BeginRain();
        activeClouds.Remove(cloud);

        BloomFlower();
        if (cloud == rainClouds[rainClouds.Length - 1])
        {
            BloomFlower();
        }
    }

    private void BloomFlower()
    {
        if (activeFlowerIndex < flowers.Length)
        {
            flowers[activeFlowerIndex].Bloom();
            activeFlowerIndex++;
        }
    }

    private void OnFlowerBloomFinished(CactusFlower flower)
    {
        if (flower == flowers[flowers.Length - 1])
        {
            FinishGame();
        }
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
}
