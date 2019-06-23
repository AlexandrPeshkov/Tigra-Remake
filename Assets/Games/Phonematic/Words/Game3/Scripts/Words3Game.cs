using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Words3Game : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 1;

    public Transform centerCharacterPoint;
    public Transform leftCharacterPoint;
    public Transform rightCharacterPoint;
    public Transform[] leftImagesPoints;
    public Transform[] rightImagesPoints;
    public Words3CharacterButton[] characterButtons;
    public Words3SideButton[] sideButtons;

    private static int activeCharaterButtonIndex = 0;
    private List<GameObject> objectsForDestroy = new List<GameObject>();
    private Words3Image activeImage;
    private List<KeyValuePair<Sprite, bool>> selectedImages = new List<KeyValuePair<Sprite, bool>>();

    public virtual void Start()
    {
        if (playInstructionAudio)
        {
            AudioManager.instance.PlayAnimals();
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

        characterButtons[activeCharaterButtonIndex].SetIsDown();
        characterButtons.ToList().ForEach((b) => b.canClick = true);
        sideButtons.ToList().ForEach((b) => b.canClick = true);
        characterButtons.ToList().ForEach((b) => b.onClick = CharacterButtonsOnClick);
        sideButtons.ToList().ForEach((b) => b.onClick = SideButtonsOnClick);
        AddCharacterImages();
        AddImages();
    }

    private void CharacterButtonsOnClick(Words3CharacterButton button)
    {
        if (isStarted)
        {
            for (int i = 0; i < characterButtons.Length; i++)
            {
                if (characterButtons[i] == button)
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

    private void RemoveAll()
    {
        objectsForDestroy.ForEach((o) => Destroy(o));
        objectsForDestroy.Clear();
        selectedImages.Clear();
    }

    private void AddCharacterImages()
    {
        GameObject leftCharacter = new GameObject();
        SpriteRenderer sr = leftCharacter.AddComponent<SpriteRenderer>();
        sr.sprite = characterButtons[activeCharaterButtonIndex].leftCharacterImage;
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 1;
        leftCharacter.transform.position = leftCharacterPoint.transform.position;
        objectsForDestroy.Add(leftCharacter);

        GameObject rightCharacter = new GameObject();
        sr = rightCharacter.AddComponent<SpriteRenderer>();
        sr.sprite = characterButtons[activeCharaterButtonIndex].rightCharacterImage;
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 1;
        rightCharacter.transform.position = rightCharacterPoint.transform.position;
        objectsForDestroy.Add(rightCharacter);
    }

    private void AddImages()
    {
        List<Sprite> leftTemp = characterButtons[activeCharaterButtonIndex].leftImages.images.ToList();
        List<Sprite> rightTemp = characterButtons[activeCharaterButtonIndex].rightImages.images.ToList();

        for (int i = 0; i < 3; i++)
        {
            int ri = UnityEngine.Random.Range(0, leftTemp.Count);
            selectedImages.Add(new KeyValuePair<Sprite, bool>(leftTemp[ri], true));
            leftTemp.RemoveAt(ri);
        }
        for (int i = 0; i < 3; i++)
        {
            int ri = UnityEngine.Random.Range(0, rightTemp.Count);
            selectedImages.Add(new KeyValuePair<Sprite, bool>(rightTemp[ri], false));
            rightTemp.RemoveAt(ri);
        }

        NextImage();
    }

    private void NextImage()
    {
        int ri = UnityEngine.Random.Range(0, selectedImages.Count);

        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = selectedImages[ri].Key;
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 1;
        activeImage = go.AddComponent<Words3Image>();
        activeImage.isLeft = selectedImages[ri].Value;
        go.transform.position = centerCharacterPoint.position;
        selectedImages.RemoveAt(ri);

        objectsForDestroy.Add(go);
    }

    private void SideButtonsOnClick(Words3SideButton button)
    {
        if (isStarted)
        {
            if (activeImage.isLeft == button.isLeft)
            {
                AudioManager.instance.PlaySelect();
                button.canClick = false;
                activeImage.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 10;
                activeImage.MoveToPos(button.point.position);

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
