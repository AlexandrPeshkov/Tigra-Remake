using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float minSpeed = 0.1f;
    public float maxSpeed = 0.5f;
    private float speed = 0.1f;
    private float camWidth;
    private float width;

    private void Start()
    {
        camWidth = Camera.main.orthographicSize * Camera.main.aspect;
        if (GetComponent<SpriteRenderer>() != null)
        {
            width = GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        } 
        else
        {
            Vector3[] v = new Vector3[4];
            GetComponent<RectTransform>().GetWorldCorners(v);
            width = v[2].x - v[0].x;
        }
        CalcSpeed();
    }

    private void CalcSpeed()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update () {
        Vector3 p = transform.position;
        p.x += speed;

        if (p.x > camWidth)
        {
            p.x = -camWidth - width;
            CalcSpeed();
        }
        transform.position = p;
	}
}
