using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LexisWordsGame1 : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 3;

    public List<LexisWordsGame1House> houses;
    public List<LexisWordsGroupSprites> groupSprites;
    public LexisWordsGame1Box box;

    private static int activeCharaterButtonIndex = 0;

    private List<GrpoupSprite> activeGroupSprites = new List<GrpoupSprite>();
    private int activeIndex = 0;

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
        winPanel.SetActive(true);
    }

    public virtual void GoToMainMenu()
    {
        winPanel.SetActive(false);
        startAfterLoad = true;
        playInstructionAudio = true;
        activeCharaterButtonIndex = 0;
        MenuManager.showMenuOnStart = 2;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void BeginGame()
    {
        RemoveAll();

        FillGroups(); 
        foreach(LexisWordsGame1House house in houses)
        {
            house.onClick = HouseOnClick;
        }
        box.SetImage(activeGroupSprites[activeIndex].groupId, activeGroupSprites[activeIndex].sprite);
    }

    private void HouseOnClick(LexisWordsGame1House house)
    {
        if (house.groupId == activeGroupSprites[activeIndex].groupId)
        {
            AudioManager.instance.PlaySelect();
            house.SuccessSignEnable();

            activeIndex++;
            if (activeIndex >= 16)
            {
                FinishGame();
            }
            else
            {
                box.SetImage(activeGroupSprites[activeIndex].groupId, activeGroupSprites[activeIndex].sprite);
            }
        }
        else
        {
            AudioManager.instance.PlayWrong();
        }
    }

    private void RemoveAll()
    {
        activeGroupSprites.Clear();
        houses.ForEach(h => h.Reset());
        
        activeIndex = 0;
    }

    public void FillGroups()
    {
        for (int i = 0; i < 4; i++)
        {
            List<Sprite> sprites = groupSprites[i].sprites.ToList();

            for (int j = 0; j < 4; j++)
            {
                int r = UnityEngine.Random.Range(0, sprites.Count);
                GrpoupSprite gs = new GrpoupSprite();
                gs.groupId = i;
                gs.sprite = sprites[r];
                sprites.RemoveAt(r);

                activeGroupSprites.Add(gs);
            }
        }

        List<GrpoupSprite> listGs = new List<GrpoupSprite>();
        int l = activeGroupSprites.Count;
        for (int i = 0; i < l; i++)
        {
            int r = UnityEngine.Random.Range(0, activeGroupSprites.Count);
            listGs.Add(activeGroupSprites[r]);
            activeGroupSprites.RemoveAt(r);
        }
        activeGroupSprites = listGs;
    }


    private IEnumerator BeginShowWinPanel()
    {
        yield return new WaitForSeconds(1.0f);

        ShowWinPanel();
    }

    private class GrpoupSprite
    {
        public int groupId = 0;
        public Sprite sprite;
    }
}
