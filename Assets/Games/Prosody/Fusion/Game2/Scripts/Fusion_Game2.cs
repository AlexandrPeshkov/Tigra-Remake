using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fusion_Game2 : FusionGameBase
{
    public Text fusionText;
    public GameObject[] cocounts;
    public Animator manAnimator;

    private int activeCocountIndex = 0;
    private float shakeTime = 0.0f;
    public float shakeTimeMax = 0.8f;
    private bool isShaking = false;
    

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isShaking)
        {
            shakeTime += Time.deltaTime;
        }
    }

    public override void Update () {
        base.Update();

        if (isStarted)
        {
            UpdateFusionTime();

            if (isShaking)
            {
                if (shakeTime >= shakeTimeMax)
                {
                    shakeTime = 0.0f;
                    DropCocount();
                }
            }
        }
    }

    private void UpdateFusionTime()
    {
        int seconds = (int)fusionTime;
        int millisec = (int)(((fusionTime * 1000) % 1000) / 10);
        fusionText.text = seconds.ToString("00") + ":" + millisec.ToString("00");
    }

    private void DropCocount()
    {
        if (activeCocountIndex < cocounts.Length)
        {
            cocounts[activeCocountIndex].transform.SetParent(null);
            cocounts[activeCocountIndex].GetComponent<Rigidbody2D>().simulated = true;
            activeCocountIndex++;
            if (activeCocountIndex >= cocounts.Length)
            {
                StartCoroutine(BeginFinishGame());
            }
        }
    }

    protected override void FusionOn()
    {
        isShaking = true;
        shakeTime = 0.0f;

        if (!manAnimator.GetCurrentAnimatorStateInfo(0).IsName("shake_anim"))
        {
            manAnimator.CrossFade("shake_anim", 0.3f);
        }
    }

    protected override void FusionOff()
    {
        isShaking = false;
        shakeTime = 0.0f;
        manAnimator.CrossFade("palm_idle", 0.3f);
    }

    public void PlayManAnim()
    {
        //manAnimator.Play("shake_anim");
    }

    public void PlayManAnimIdle()
    {
        //manAnimator.Play("shake_anim");
    }

    IEnumerator BeginFinishGame()
    {
        yield return new WaitForSeconds(2);


        FinishGame();
    }
}
