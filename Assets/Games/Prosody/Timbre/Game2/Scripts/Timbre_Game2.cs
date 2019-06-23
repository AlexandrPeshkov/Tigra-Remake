using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timbre_Game2 : TimbreGameBase
{
    public GameObject[] activeDigits;
    public Transform[] activePoints;
    public Slider slider;
    public TimbreGame_Alladin alladin;
    public float speedKoef = 9.0f;
    public Animator alladinAnimator;
    public bool stopEventInvoked = false;

    public override void Start()
    {
        PlayIdleAnim();
        alladin.onStopMoving += Alladin_onStopMoving;
        base.Start();
    }

    private void Alladin_onStopMoving()
    {
        PlayIdleAnim();
        stopEventInvoked = true;
    }

    private void PlayIdleAnim()
    {
        if (!stopEventInvoked)
        {
            alladinAnimator.CrossFade("man_idle_anim", 0.3f);
        }
    }

    private void PlayFlyAnim()
    {
        if (!alladinAnimator.GetCurrentAnimatorStateInfo(0).IsName("man_fly_anim"))
        {
            alladinAnimator.CrossFade("man_fly_anim", 0.3f);
        }
    }

    protected override void TimbreOn()
    {
        if (isStarted)
        { 
            stopEventInvoked = false;
            alladin.MoveUp((timbreLevel / speedKoef) - 3.8f);
            PlayFlyAnim();
        }
    }

    protected override void TimbreOff()
    {
        alladin.StopMoving();
    }

    public override void Update()
    {
        base.Update();

        if (isStarted)
        {
            if (isTimbreOn)
            {
                alladin.MoveUp((timbreLevel / speedKoef) - 3.8f);
            }
        }

        CheckPoints();
    }

    private void CheckPoints()
    {
        for(int i = 0; i < activePoints.Length; i++)
        {
            activeDigits[i].SetActive(alladin.gameObject.transform.position.y > activePoints[i].position.y);

            if (alladin.gameObject.transform.position.y > activePoints[(int) slider.value].position.y)
            {
                TimbreOff();
                FinishGame();
                isTimbreOn = false;
                return;
            }
        }
    }
}
