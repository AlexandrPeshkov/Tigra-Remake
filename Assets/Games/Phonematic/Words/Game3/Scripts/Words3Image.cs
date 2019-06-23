using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Words3Image : MonoBehaviour
{
    public bool isLeft = false;

    private float t = 0.0f;
    private bool isMoving;
    private Vector3 startPos;
    private Vector3 endPos;

    public void MoveToPos(Vector3 pos)
    {
        isMoving = true;
        t = 0.0f;
        startPos = gameObject.transform.position;
        endPos = pos;
        gameObject.transform.position = pos;
    }

    private void Update()
    {
        if (isMoving)
        {
            t += Time.deltaTime * 4;
            if (t > 1.0f)
            {
                t = 1.0f;
                isMoving = false;
            }
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, t);
        }
    }
}
