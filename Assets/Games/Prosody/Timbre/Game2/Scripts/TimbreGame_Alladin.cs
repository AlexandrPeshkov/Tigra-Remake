using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimbreGame_Alladin : MonoBehaviour
{
    public float speed = 2f;
    public float gravity = 0.1f;
    public Transform startPoint;

    public UnityAction onFly;
    public UnityAction onStopMoving;

    private float t = 0.0f;
    private float maxHeight = 2.0f;
    private bool isMoving = false;
    private bool isForceDown = false;

    public void MoveUp(float height)
    {
        maxHeight = height;
        isMoving = true;
    }

    public void StopMoving()
    {
        isForceDown = false;
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (!isForceDown)
            {
                Vector2 pos = gameObject.transform.position;
                pos.y = pos.y + speed;
                //pos.y = Mathf.Lerp(pos.y, maxHeight, 0.02f);
                if (pos.y >= maxHeight)
                {
                   isForceDown = true;
                }
                gameObject.transform.position = pos;
            }
            else
            {
                Vector2 pos = gameObject.transform.position;
                pos.y = pos.y - gravity;

                if (pos.y < maxHeight)
                {
                    isForceDown = false;
                }
                if (pos.y <= startPoint.position.y)
                {
                    isForceDown = false;
                    isMoving = false;
                    onStopMoving();
                    pos.y = startPoint.position.y;
                }
                gameObject.transform.position = pos;
            }
            
        }
        else
        {
            Vector2 pos = gameObject.transform.position;
            pos.y = pos.y - gravity;

            if (pos.y <= startPoint.position.y)
            {
                isForceDown = false;
                isMoving = false;
                onStopMoving();
                pos.y = startPoint.position.y;
            }
            gameObject.transform.position = pos;
        }
    }
}
