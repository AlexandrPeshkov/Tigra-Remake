using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AntLeaf : MonoBehaviour
{
    public UnityEvent OnClick = new UnityEvent();
    public char character;

    private float speed = 2f;
    private bool isHide = false;
    private SpriteRenderer spriteRenderer;
    private float t = 0.0f;
    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        OnClick.Invoke();
    }

    public void Hide()
    {
        startPos = transform.position;
        endPos = new Vector3(startPos.x, -6, startPos.z);
        isHide = true;
    }

    void Update()
    {
        if (isHide)
        {
            t += Time.deltaTime * speed;
            Color c = spriteRenderer.color;
            c.a = 1.0f - t;
            spriteRenderer.color = c;

            Vector3 pos = Vector3.Lerp(startPos, endPos, t);
            transform.position = pos;

            if (t >= 1)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
