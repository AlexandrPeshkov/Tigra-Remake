using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Words2Wheel : MonoBehaviour
{
    public Words2Circle[] circles;
    public GameObject arrow;
    [HideInInspector]
    public UnityAction<int> onFinish;
    public int activeCircle;

    private float currentAngle;
    private float startAngle;
    private float endAngle;
    private float t;
    private bool isStarted = false;

    public void Rotate(int index)
    {
        if (!isStarted && (index != activeCircle))
        {
            startAngle = currentAngle;
            activeCircle = index;
            endAngle = circles[index].angle;
            t = 0;
            isStarted = true;
        }
    }

    private void Update()
    {
        if (isStarted)
        {
            t += Time.deltaTime * 2.0f;
            if (t > 1.0f)
            {
                t = 1.0f;
                isStarted = false;
                currentAngle = endAngle;

                onFinish.Invoke(activeCircle);
            }
            //currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            Vector3 sa = arrow.transform.eulerAngles;
            sa.z = startAngle;

            Vector3 ea = arrow.transform.eulerAngles;
            ea.z = endAngle;

            arrow.transform.rotation = Quaternion.Lerp(Quaternion.Euler(sa), Quaternion.Euler(ea), t);
        }
    }

    public void SetWheelButtons(List<Words2ImageButton> buttons)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.transform.position = circles[i].gameObject.transform.position;
        }
    }
}
