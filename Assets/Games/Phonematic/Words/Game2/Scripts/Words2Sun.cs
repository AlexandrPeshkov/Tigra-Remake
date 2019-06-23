using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Words2Sun : MonoBehaviour {
    private float t = 1.0f;
    private float speed = -1;
    private float scale = 0.0f;

	void Update ()
    {
        t += Time.deltaTime * speed;
        if ((t < 0) || (t > 1.0f))
        {
            speed = -speed;
        }
        scale = Mathf.Lerp(1.1f, 0.75f, t);
        gameObject.transform.localScale = new Vector3(scale, scale, 1.0f);
        gameObject.transform.Rotate(new Vector3(0, 0, 0.05f), Space.Self);
    }
}
