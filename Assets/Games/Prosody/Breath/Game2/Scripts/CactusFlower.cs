using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusFlower : MonoBehaviour
{
    public float bloomSpeed = 0.001f;
    public BloomFinishedDelegate onBloomFinished;
    private float startScale;
    private float t = 0.0f;
    private bool isBloomed = false;

    void Start ()
    {
        startScale = gameObject.transform.localScale.x;
        Vector3 scale = new Vector3();
        gameObject.transform.localScale = scale;
        gameObject.SetActive(false);
	}

    void Update ()
    {
		if (isBloomed)
        {
            t += Time.deltaTime * bloomSpeed;

            float scale = Mathf.Lerp(0.0f, startScale, t);
            Vector3 scaleVector = new Vector3(scale, scale, 1);
            gameObject.transform.localScale = scaleVector;

            if (t >= 1.0f)
            {
                isBloomed = false;
                onBloomFinished(this);
            }
        }
	}

    public void Bloom()
    {
        isBloomed = true;
        gameObject.SetActive(true);
    }

    public delegate void BloomFinishedDelegate(CactusFlower cloud);
}
