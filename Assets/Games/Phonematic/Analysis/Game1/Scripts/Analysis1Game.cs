using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Analysis1Game : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 3;

    private static int activeCharaterButtonIndex = 0;
    public Transform topButtonstLeftPoint;
    public Transform topButtonstRightPoint;
    public Transform centerImagePoint;
    public Analysis1FillPanel[] fillPanels;
    public Sprite[] topButtonsNormalSprite;
    public Sprite[] topButtonsDownSprite;
    public Analysis1HouseButton[] houseButtons;
    public Analysis1CharacterImages[] characterImages;
    private List<Analysis1CharacterImage> selectedImages = new List<Analysis1CharacterImage>();
    private Analysis1CharacterImage activeImage;
    private List<GameObject> objectsForDestroy = new List<GameObject>();

    private List<Analysis1CharacterButton> characterButtons = new List<Analysis1CharacterButton>();

    public virtual void Start()
    {
        if (playInstructionAudio)
        {
            AudioManager.instance.PlayHouses();
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
        
        AddCharacterButtons();
        characterButtons[activeCharaterButtonIndex].SetIsDown();
        houseButtons.ToList().ForEach((b) => b.onClick = HouseButtonOnClick);
        AddCharacterImages();
    }

    private void RemoveAll()
    {
        objectsForDestroy.ForEach((o) => Destroy(o));
        objectsForDestroy.Clear();

        characterButtons.Clear();
        selectedImages.Clear();

        fillPanels.ToList().ForEach((p) => p.Reset());
        houseButtons.ToList().ForEach((b) => b.canClick = true);

        activeImage = null;
    }

    private void AddCharacterButtons()
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject go = new GameObject();
            Analysis1CharacterButton button = go.AddComponent<Analysis1CharacterButton>();
            button.Create(topButtonsNormalSprite[i], topButtonsDownSprite[i]);
            if (i < 4)
            {
                Vector3 v = topButtonstLeftPoint.position;
                v.x = topButtonstLeftPoint.position.x + (i * 1.1f) % 4;
                button.transform.position = v;
            }
            else
            {
                Vector3 v = topButtonstRightPoint.position;
                v.x = topButtonstRightPoint.position.x + (i * 1f) % 4;
                button.transform.position = v;
            }
            button.index = i;
            button.onClick = CharacterButtonOnClick;

            characterButtons.Add(button);

            objectsForDestroy.Add(go);
        }
    }

    private void CharacterButtonOnClick(Analysis1CharacterButton button)
    {
        if (isStarted)
        {
            for (int i = 0; i < characterButtons.Count; i++)
            {
                if (i == button.index)
                {
                    activeCharaterButtonIndex = i;
                }
                else
                {
                    characterButtons[i].SetIsNormal();
                }
            }
            BeginGame();
        }
    }

    private void AddCharacterImages()
    {
        // BEGIN
        List<int> temp = new List<int>();
        for (int i = 0; i < characterImages[activeCharaterButtonIndex].beginImages.Length; i++)
        {
            temp.Add(i);
        }
        for (int i = 0; i < 3; i++)
        {
            int ri = UnityEngine.Random.Range(0, temp.Count);
            Analysis1CharacterImage image = new Analysis1CharacterImage();
            image.image = characterImages[activeCharaterButtonIndex].beginImages[temp[ri]];
            image.position = 0;
            selectedImages.Add(image);
            temp.RemoveAt(ri);
        }
        // MIDDLE
        temp.Clear();
        temp = new List<int>();
        for (int i = 0; i < characterImages[activeCharaterButtonIndex].centerImages.Length; i++)
        {
            temp.Add(i);
        }
        for (int i = 0; i < 3; i++)
        {
            int ri = UnityEngine.Random.Range(0, temp.Count);
            Analysis1CharacterImage image = new Analysis1CharacterImage();
            image.image = characterImages[activeCharaterButtonIndex].centerImages[temp[ri]];
            image.position = 1;
            selectedImages.Add(image);
            temp.RemoveAt(ri);
        }
        if ((characterImages[activeCharaterButtonIndex].endImages != null) && 
            (characterImages[activeCharaterButtonIndex].endImages.Length > 0))
        {
            // END
            temp.Clear();
            temp = new List<int>();
            for (int i = 0; i < characterImages[activeCharaterButtonIndex].endImages.Length; i++)
            {
                temp.Add(i);
            }
            for (int i = 0; i < 3; i++)
            {
                int ri = UnityEngine.Random.Range(0, temp.Count);
                Analysis1CharacterImage image = new Analysis1CharacterImage();
                image.image = characterImages[activeCharaterButtonIndex].endImages[temp[ri]];
                image.position = 2;
                selectedImages.Add(image);
                temp.RemoveAt(ri);
            }
        }

        NextImage();
    }

    private void NextImage()
    {
        if ((activeImage != null) && (activeImage.gameObject != null))
        {
            Destroy(activeImage.gameObject);
        }
        int ri = UnityEngine.Random.Range(0, selectedImages.Count);
        activeImage = selectedImages[ri];
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = activeImage.image;
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 1;
        go.transform.position = centerImagePoint.position;
        activeImage.gameObject = go;
        selectedImages.RemoveAt(ri);


        objectsForDestroy.Add(go);
    }

    private void HouseButtonOnClick(Analysis1HouseButton button)
    {
        if (!fillPanels[button.index].AllIsFilled())
        {
            if (button.index == activeImage.position)
            {
                AudioManager.instance.PlaySelect();
                fillPanels[button.index].FillOne();
                if (fillPanels[button.index].AllIsFilled())
                {
                    button.canClick = false;
                }

                if (selectedImages.Count > 0)
                {
                    NextImage();
                }
                else
                {
                    characterButtons.ToList().ForEach((b) => b.canClick = false);
                    FinishGame();
                }
            }
            else
            {
                AudioManager.instance.PlayWrong();
            }
        }
    }

    private IEnumerator BeginShowWinPanel()
    {
        yield return new WaitForSeconds(1.0f);

        ShowWinPanel();
    }
}
