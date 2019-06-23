using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundPronunciationGame1 : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 3;

    public static int startGroupId = 0;
    public static int startGameId = 0;

    private static Coroutine animateCoroutine;

    public List<SoundPronunciationGame1Screen> screens;
    public SpriteRenderer successSprite;
    public SpriteRenderer wrongSprite;
    public SpriteRenderer table;
    public Text titleText;

    private int activeScreenId = 0;
    private int activeSpriteAnim = 0;

    public virtual void Start()
    {
        if (playInstructionAudio)
        {
            //AudioManager.instance.PlayHouses();
        }
        playInstructionAudio = false;

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
        //winPanel.SetActive(false);
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
        BeginGame();
    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FinishGame()
    {
        AudioManager.instance.PlayCorrectly();
        isStarted = false;

        StartCoroutine(BeginShowWinPanel());
    }

    protected virtual void ShowWinPanel()
    {
        //winPanel.SetActive(true);
    }

    public virtual void GoToMainMenu()
    {
        //winPanel.SetActive(false);
        startAfterLoad = true;
        playInstructionAudio = true;
        submenuIndex = startGroupId;
        MenuManager.showMenuOnStart = 3;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void BeginGame()
    {
        RemoveAll();

        FillScreen();
    }

    private void RemoveAll()
    {

    }

    public void FillScreen(int screenId = -1)
    {
        activeScreenId = (screenId == -1) ? FindScreen(startGroupId, startGameId) : screenId;

        if (activeScreenId != -1)
        {
            int spriteAnimIndex = Mathf.Min(activeSpriteAnim, screens[activeScreenId].successSprites.Count - 1);
            successSprite.sprite = screens[activeScreenId].successSprites[spriteAnimIndex];
            wrongSprite.sprite = screens[activeScreenId].wrongSprite;
            table.sprite = screens[activeScreenId].table;
            titleText.text = screens[activeScreenId].title;
        }
        RestartAnimateSprite();
    }

    private int FindScreen(int group, int game)
    {
        for (int i = 0; i < screens.Count; i++)
        {
            if ((group == screens[i].groupId) && (game == screens[i].gameId))
            {
                return i;
            }
        }

        return -1;
    }

    private void RestartAnimateSprite()
    {
        if (animateCoroutine != null)
        {
            StopCoroutine(animateCoroutine);
        }
        
        animateCoroutine = StartCoroutine(AnimateSprites());
    }

    public void Prev()
    {
        int screenId = (activeScreenId - 1 < 0) ? screens.Count - 1 : activeScreenId - 1;
        FillScreen(screenId);
    }

    public void Next()
    {
        int screenId = (activeScreenId + 1 >= screens.Count) ? 0 : activeScreenId + 1;
        FillScreen(screenId);
    }

    private IEnumerator BeginShowWinPanel()
    {
        yield return new WaitForSeconds(1.0f);

        ShowWinPanel();
    }

    private IEnumerator AnimateSprites()
    {
        yield return new WaitForSeconds(0.7f);

        activeSpriteAnim++;
        if (activeSpriteAnim > 1)
        {
            activeSpriteAnim = 0;
        }

        int spriteAnimIndex = Mathf.Min(activeSpriteAnim, screens[activeScreenId].successSprites.Count - 1);
        successSprite.sprite = screens[activeScreenId].successSprites[spriteAnimIndex];

        RestartAnimateSprite();
    }
}
