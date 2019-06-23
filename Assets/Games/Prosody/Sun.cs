using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {
    public float scaleSpeed = 0.02f;
    public float minScale = 0.5f;
    public float maxScale = 1.0f;
    private float dir = 1;

	void Update () {
        float s = transform.localScale.x + scaleSpeed * dir;
        if (dir == 1)
        {
            if (s > maxScale)
            {
                s = maxScale;
                dir = -1;
            }
        }
        else
        {
            if (s < minScale)
            {
                s = minScale;
                dir = 1;
            }
        }
        Vector3 v = new Vector3(s, s, 1);
        transform.localScale = v;

    }
}
