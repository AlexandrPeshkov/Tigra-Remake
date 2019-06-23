using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : MonoBehaviour
{
    [SerializeField]
    public List<LizardPostition> lizardPostitions;
    public int activeLizardPosition;
    public float speed = 0.02f;
    public float maxY = 1.5f;
    private float startY;
    private float t = 0.0f;
    private LizardState state = LizardState.Idle;

    private void Start()
    {
        startY = transform.position.y;

        CalcNewPosition();
        BeginShow();
    }

    private LizardPostition GetCurrentPosition()
    {
        return lizardPostitions[activeLizardPosition];
    }

    void Update()
    {
        if (lizardPostitions.Count > 0)
        {
            switch (state)
            {
                case LizardState.Idle:
                    break;

                case LizardState.Showing:
                    if (t < 1.0f)
                    {
                        Vector3 v;
                        LizardPostition pos = GetCurrentPosition();
                        t += Time.deltaTime * pos.speed;
                        float y = Mathf.Lerp(startY, startY + pos.maxY, t);
                        if (t < 1.0f)
                        {
                            v = transform.position;
                            v.y = y;
                            transform.position = v;
                        }
                        else
                        {
                            state = LizardState.Idle;

                            v = transform.position;
                            v.y = startY + pos.maxY;
                            transform.position = v;

                            BeginHide();
                        }
                    }
                    break;

                case LizardState.Hiding:
                    if (t < 1.0f)
                    {
                        Vector3 v;
                        LizardPostition pos = GetCurrentPosition();
                        t += Time.deltaTime * pos.speed;
                        float y = Mathf.Lerp(startY + pos.maxY, startY, t);
                        if (t < 1.0f)
                        {
                            v = transform.position;
                            v.y = y;
                            transform.position = v;
                        }
                        else
                        {
                            state = LizardState.Idle;

                            v = transform.position;
                            v.y = startY;
                            transform.position = v;

                            CalcNewPosition();
                            BeginShow();
                        }
                    }
                    break;
            }
        }
    }

    private void CalcNewPosition()
    {
        activeLizardPosition = Random.Range(0, lizardPostitions.Count);
    }

    private void BeginHide()
    {
        StartCoroutine(HideLizard());
    }

    private void Hide()
    {
        t = 0.0f;
        state = LizardState.Hiding;
    }

    private void BeginShow()
    {
        StartCoroutine(ShowLizard());
    }

    private void Show()
    {
        t = 0.0f;
        state = LizardState.Showing;
    }

    IEnumerator ShowLizard()
    {
        yield return new WaitForSeconds(GetCurrentPosition().showTimeout);

        Show();
    }

    IEnumerator HideLizard()
    {
        yield return new WaitForSeconds(GetCurrentPosition().hideTimeout);

        Hide();
    }

    [System.Serializable]
    public struct LizardPostition
    {
        public float speed;
        public float maxY;
        public float showTimeout;
        public float hideTimeout;
    }

    public enum LizardState
    {
        Idle, Showing, Hiding
    }
}
