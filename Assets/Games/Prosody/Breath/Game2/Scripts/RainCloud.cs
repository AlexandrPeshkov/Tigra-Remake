using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RainCloud : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float maxSpeed = 0.017f;
    public float accSpeed = 0.0001f;
    public float dragSpeed = 0.000028f;
    public float nextCloud = 0.75f;
    public ParticleSystem blobParticleSystem;
    public UnityAction onNextCloud;
    public MoveDelegate onMoveFinished;
    public float temp;

    public float currentSpeed = 0.0f;
    private float t = 0.0f;
    private float speedMultiplier = 1.0f;
    private float speedMultiplierMax = 590.0f;
    private bool nextCloudWasInvoked = false;
    private Color secondCloudColor;
    private SpriteRenderer secondCloudSR;
    private bool isMoving = false;
    private bool moveFinished = false;

    public void ResetPosition()
    {
        gameObject.transform.position = startPoint.position;
    }

    public void MoveBegin(float mult)
    {
        speedMultiplier = mult;
        isMoving = true;
    }

    public void MoveEnd()
    {
        isMoving = false;
    }

    void Start () {
        secondCloudSR = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        secondCloudColor = secondCloudSR.color;
    }
	
	void Update ()
    {
        if (isMoving)
        {
            temp = (speedMultiplier / speedMultiplierMax);
            currentSpeed = Mathf.Min(maxSpeed, (currentSpeed + accSpeed * (speedMultiplier * Time.deltaTime)));
        }
        else
        {
            currentSpeed = Mathf.Max(0.0f, currentSpeed - dragSpeed);
        }
        t += currentSpeed;

        float x = Mathf.Lerp(startPoint.position.x, endPoint.position.x, t);
        Vector3 pos = gameObject.transform.position;
        pos.x = x;
        gameObject.transform.position = pos;

        secondCloudColor.a = t;
        secondCloudSR.color = secondCloudColor;

        if (!nextCloudWasInvoked && (t >= nextCloud))
        {
            onNextCloud();
            nextCloudWasInvoked = true;
        }
        if (!moveFinished && (t >= 1.0f))
        {
            moveFinished = true;
            onMoveFinished(this);
        }
    }

    public void BeginRain()
    {
        blobParticleSystem.Play();
    }

    public delegate void MoveDelegate(RainCloud cloud);
}
