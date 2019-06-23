using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fusion_Game1 : FusionGameBase
{
    public Text fusionText;
    public Animator cheekAnimator;
    public GameObject snake;
    public Animator snakeAnimator;
    public Transform snakeStartPoint;
    public Transform snakeStartAnimPoint;
    public Transform snakeEndPoint;
    public float speed = 0.5f;
    public ParticleSystem melodyParticles;
    public float startMelodyTime;

    private float t = 0.0f;


    public override void StartGame()
    {
        snakeAnimator.StopPlayback();
        snakeAnimator.Play("New State");
        base.StartGame();
    }

    private void UpdateFusionTime()
    {
        int seconds = (int)fusionTime;
        int millisec = (int)(((fusionTime * 1000) % 1000) / 10);
        fusionText.text = seconds.ToString("00") + ":" + millisec.ToString("00");
    }

    public override void Update () {
        base.Update();


        if (isStarted)
        {
            UpdateFusionTime();

            if (isFusionOn)
            { 
                if (!cheekAnimator.GetCurrentAnimatorStateInfo(0).IsName("Cheek_anim"))
                {
                    cheekAnimator.Play("Cheek_anim");
                    melodyParticles.Play();
                    startMelodyTime = Time.time;
                }

                Vector2 v = snake.transform.position;
                t += Time.deltaTime * speed;
                float y = Mathf.Lerp(snakeStartPoint.position.y, snakeEndPoint.position.y, t);
                if (t < 1)
                {
                    v.y = y;
                    snake.transform.position = v;

                    if ((snake.transform.position.y >= snakeStartAnimPoint.position.y) && 
                        !snakeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Snake_anim"))
                    {
                        snakeAnimator.Play("Snake_anim");
                    }
                }
                else
                {
                    v.y = snakeEndPoint.position.y;
                    snake.transform.position = v;
                    FinishGame();
                }
            }
            else
            {
                if (cheekAnimator.GetCurrentAnimatorStateInfo(0).IsName("Cheek_anim"))
                {
                    if (Time.time - startMelodyTime > 1)
                    {
                        cheekAnimator.StopPlayback();
                        cheekAnimator.Play("New State");

                        melodyParticles.Stop();
                    }
                }
            }
        }
    }

}
