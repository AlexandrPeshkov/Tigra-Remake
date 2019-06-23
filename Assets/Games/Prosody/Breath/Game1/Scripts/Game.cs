using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : ProsodyGameBase
{
    public GameObject tumbleweed;

    protected override void GameStarted()
    {
        MicrophoneCaptureManager.instance.BeginCapture();
    }

    public override void FixedUpdate()
    { 
        if (isStarted)
        {
            if (MicrophoneCaptureManager.instance != null)
            {
                Rigidbody2D rb = tumbleweed.GetComponent<Rigidbody2D>();
                float f = MicrophoneCaptureManager.instance.GetArithmeticMean() / 17.0f;
                if (f > 0.55f)
                {
                    rb.AddForce(new Vector2(f * 0.8f, 0));
                }
            }
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
