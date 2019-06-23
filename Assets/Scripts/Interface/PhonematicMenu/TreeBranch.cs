using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBranch : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2.0f;
    public bool visible = false;

    private float time = 0.0f;
    private bool isMoving = false;
    private bool isShowing = false;

    public void Reset()
    {
        isMoving = false;
        isShowing = false;
        gameObject.transform.position = startPoint.position;
    }

    private void Start()
    {
        time = 0.0f;
    }

    public void Show()
    {
        isShowing = true;
        isMoving = true;
    }

    public void Hide()
    {
        isShowing = false;
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (isShowing)
            {
                time += Time.deltaTime * speed;

                if (time >= 1.0f)
                {
                    time = 1.0f;
                    isMoving = false;
                    visible = true;
                }
            }
            else
            {
                time -= Time.deltaTime * speed;

                if (time <= 0.0f)
                {
                    time = 0.0f;
                    isMoving = false;
                    visible = false;
                }
            }
            gameObject.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, time);
        }
    }
}
