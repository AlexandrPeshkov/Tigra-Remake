using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProsodyGameBase : MonoBehaviour
{
    protected static bool startAfterLoad = false;
    public GameObject winPanel;
    public GameObject equalizer;
    public GameObject microphoneSettings;
    protected bool isStarted = false;
    protected int submenuIndex = 0;

    public virtual void Start ()
    {
        isStarted = false;
        if (startAfterLoad)
        {
            startAfterLoad = false;
            StartGame();
        }
    }

    public virtual void StartGame()
    {
        isStarted = true;
        winPanel.SetActive(false);
        if (!startAfterLoad)
        {
            GameStarted();
            startAfterLoad = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    protected virtual void GameStarted()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Update ()
    {

    }

    public virtual void FinishGame()
    {
        isStarted = false;
        ShowWinPanel();
    }

    protected virtual void ShowWinPanel()
    {
        winPanel.SetActive(true);
        microphoneSettings.SetActive(false);
    }

    public virtual void ShowHideEqalizer()
    {
        equalizer.SetActive(!equalizer.activeSelf);
    }

    public virtual void GoToMainMenu()
    {
        winPanel.SetActive(false);
        startAfterLoad = false;
        MenuManager.showMenuOnStart = 1;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    public virtual void ShowMicrophoneSettings()
    {
        microphoneSettings.SetActive(true);
        microphoneSettings.GetComponent<MicrophoneSettingsMenu>().Show();
    }
}
