using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private DoorsState state;
    public float speed = 0.02f;
    public int openTryCount = 5;
    private int openTry = 0;
    public bool isLeft = false;
    private float maxX = 1.0f;
    private float startX;
    private float t = 0.0f;
    [HideInInspector]
    public Action onDoorsOpened;

    void Start ()
    {
        RecalcMaxX();

    }

    public void RecalcMaxX()
    {
        maxX = ((isLeft) ? -10 : 10) / (float)openTryCount;
    }
	
	void Update () {
        switch (state)
        {
            case DoorsState.Idle:
                break;

            case DoorsState.Opening:
                if (t < 1.0f)
                {
                    Vector3 v;
                    t += Time.deltaTime * speed;
                    float x = Mathf.Lerp(startX, startX + maxX, t);
                    if (t < 1.0f)
                    {
                        v = transform.position;
                        v.x = x;
                        transform.position = v;
                    }
                    else
                    {
                        state = DoorsState.Idle;

                        v = transform.position;
                        v.x = startX + maxX;
                        transform.position = v;

                        if (openTry >= openTryCount)
                        {
                            Finish();
                        }
                    }
                }
                break;
        }
    }

    private void Finish()
    {
        if (isLeft)
        {
            onDoorsOpened();
        }
    }

    public void OpenDoor()
    {
        openTry++;
        startX = transform.position.x;
        t = 0.0f;
        state = DoorsState.Opening;
    }

    private enum DoorsState
    {
        Idle, Opening
    }
}
