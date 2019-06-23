using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static int showMenuOnStart = 0;
    public static int showSubmenuOnStart = 0;
    public GameObject mainMenu;
    public GameObject prosodyMenu;
    public GameObject phonematicMenu;
    public GameObject soundPronunciationMenu;
    public GameObject micSettingsMenu;

    public RectTransform backButton;
    public RectTransform menuButton;
    private Vector2 backButtonStartPos;
    private Vector2 menuButtonStartPos;

    private GameObject activeMenu;

    private void Awake()
    {
        backButtonStartPos = backButton.anchoredPosition;
        menuButtonStartPos = menuButton.anchoredPosition;
    }

    private void Start()
    {
        switch(showMenuOnStart)
        {
            case 0:
                ShowMainMenu();
                break;

            case 1:
                ShowProsodyMenu();
                break;

            case 2:
                ShowPhonematicMenu();
                break;

            case 3:
                ShowSoundPronunciationMenu();
                soundPronunciationMenu.GetComponent<SoundPronunciationMenu>().SetActiveItems(showSubmenuOnStart);
                break;
        }
    }

    public void HideBackButton()
    {
        backButton.gameObject.SetActive(false);
        menuButton.anchoredPosition = backButtonStartPos;
    }

    public void ShowBackButton()
    {
        backButton.gameObject.SetActive(true);
        menuButton.anchoredPosition = menuButtonStartPos;
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

    public void ShowMainMenu()
    {
        ShowMenu(mainMenu);

        HideBackButton();
    }
    
    public void ShowProsodyMenu()
    {
        ShowMenu(prosodyMenu);
        prosodyMenu.GetComponent<ProsodyMenu>().Show(showSubmenuOnStart);
        ShowBackButton();
    }

    public void ShowPhonematicMenu()
    {
        ShowMenu(phonematicMenu);
        phonematicMenu.GetComponent<PhonematicMenu>().Reset(showSubmenuOnStart);
        ShowBackButton();
    }

    public void ShowPhonematicMenuFromMainMenu()
    {
        ShowMenu(phonematicMenu);
        phonematicMenu.GetComponent<PhonematicMenu>().Reset(-1);
        ShowBackButton();
    }

    public void ShowSoundPronunciationMenu()
    {
        ShowMenu(soundPronunciationMenu);
        soundPronunciationMenu.GetComponent<SoundPronunciationMenu>().Reset();
        ShowBackButton();
    }

    public void ShowMicSettingsMenu()
    {
        ShowMenu(micSettingsMenu);
        micSettingsMenu.GetComponent<MicrophoneSettingsMenu>().Show();
    }
}
