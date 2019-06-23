using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTable : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public GameObject breathMenu;
    public GameObject fusionMenu;
    public GameObject rhythmMenu;
    public GameObject timbreMenu;
    private GameObject activeMenu;
    private GameObject nextMenu;

    private TableGamesState state;
    public float speed = 0.02f;
    private float t = 0.0f;

    void Start () {
		
	}
	
	void Update () {
        switch (state)
        {
            case TableGamesState.Idle:
                break;

            case TableGamesState.Showing:
                if (t < 1.0f)
                {
                    Vector3 v = transform.position;
                    t += Time.deltaTime * speed;
                    float y = Mathf.Lerp(startPoint.position.y, endPoint.position.y, t);
                    if (t < 1.0f)
                    {
                        v = transform.position;
                        v.y = y;
                        transform.position = v;
                    }
                    else
                    {
                        state = TableGamesState.Idle;

                        v = transform.position;
                        v.y = endPoint.position.y;
                        transform.position = v;
                    }
                }
                break;

            case TableGamesState.Hiding:
                if (t < 1.0f)
                {
                    Vector3 v = transform.position;
                    t += Time.deltaTime * speed;
                    float y = Mathf.Lerp(endPoint.position.y, startPoint.position.y, t);
                    if (t < 1.0f)
                    {
                        v = transform.position;
                        v.y = y;
                        transform.position = v;
                    }
                    else
                    {
                        state = TableGamesState.Idle;

                        v = transform.position;
                        v.y = startPoint.position.y;
                        transform.position = v;

                        ShowMenu(nextMenu);
                        BeginShow();
                    }
                }
                break;
        }
    }

    private void BeginHide()
    {
        t = 0.0f;
        state = TableGamesState.Hiding;
    }

    private void BeginShow()
    {
        t = 0.0f;
        state = TableGamesState.Showing;
    }

    private void ShowMenu(GameObject menu)
    {
        if (activeMenu != null)
        {
            activeMenu.SetActive(false);
        }
        activeMenu = menu;
        activeMenu.SetActive(true);
    }

    public void ShowBreathMenu()
    {
        if (activeMenu != breathMenu)
        {
            nextMenu = breathMenu;
            BeginHide();
        }
    }

    public void ShowFusionMenu()
    {
        if (activeMenu != fusionMenu)
        {
            nextMenu = fusionMenu;
            BeginHide();
        }
    }

    public void ShowRhythmMenu()
    {
        if (activeMenu != rhythmMenu)
        {
            nextMenu = rhythmMenu;
            BeginHide();
        }
    }

    public void ShowTimbreMenu()
    {
        if (activeMenu != timbreMenu)
        {
            nextMenu = timbreMenu;
            BeginHide();
        }
    }

    public void ShowFirstMenu(int menuIndex = 0)
    {
        Vector3 v = transform.position;
        v.y = startPoint.position.y;
        transform.position = v;
        switch(menuIndex)
        {
            case 0:
                ShowMenu(breathMenu);
                break;
            case 1:
                ShowMenu(fusionMenu);
                break;
            case 2:
                ShowMenu(rhythmMenu);
                break;
            case 3:
                ShowMenu(timbreMenu);
                break;
            default:
                ShowMenu(breathMenu);
                break;
        }
        BeginShow();
    }

    public bool CanClick()
    {
        return state == TableGamesState.Idle;
    }

    private enum TableGamesState
    {
        Idle, Showing, Hiding
    }
}
