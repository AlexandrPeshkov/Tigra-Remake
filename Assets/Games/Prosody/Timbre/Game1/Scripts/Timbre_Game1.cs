using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timbre_Game1 : TimbreGameBase
{
    public GameObject[] activeDigits;
    public Transform[] activePoints;
    public Slider slider;
    public TimbreGame_fountain fountain;
    public bool stopEventInvoked = false;
    public int koef = 6;
    private int currentPower = 0;

    public override void Start()
    {
        PlayIdleAnim();
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
        }
    }

    private void PlayFlyAnim()
    {
    }

    protected override void TimbreOn()
    {
        if (isStarted)
        { 
            stopEventInvoked = false;
        }
    }

    protected override void TimbreOff()
    {
        currentPower = 0;
    }

    public override void Update()
    {
        base.Update();

        currentPower = 0;
        if (isStarted)
        {
            if (isTimbreOn)
            {
                currentPower = Mathf.Min(10, (int)(timbreLevel / koef));
            }
            fountain.SetPower(currentPower);
        }
        CheckPoints();


        /*if (Input.GetMouseButtonDown(0))
        {
            temp1++;
            if (temp1 > 10)
            {
                temp1 = 0;
            }
        }
        else
        {
            //temp = 0.0f;
        }
        fountain.SetPower(temp1);*/

    }

    private void CheckPoints()
    {
        for(int i = 0; i < activeDigits.Length; i++)
        {
            activeDigits[i].SetActive(fountain.GetMaxPosition() > activePoints[i].position.y);

            if (fountain.GetMaxPosition() > activePoints[(int) slider.value].position.y)
            {
                TimbreOff();
                FinishGame();
                isTimbreOn = false;
                return;
            }
        }
    }
}
