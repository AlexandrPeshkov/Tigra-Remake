using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordsGame1 : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 1;

    private string characters = "сзцшжщчлр";
    public Sprite[] characterButtonsNormal;
    public Sprite[] characterButtonsActive;
    public Transform characterButtonsStartPos;
    public Transform selectedCharacterPoint;
    [Space]
    public Transform[] treeButtonsPosition;
    public WordsCharacterImageButtons[] characterImageButtons;

    private List<GameObject> characterButtons = new List<GameObject>();
    private static int selectedCharacterIndex = 0;
    private GameObject selectedCharacterObject;
    private List<WordsCharacterImageButtons> activeImageButtons = new List<WordsCharacterImageButtons>();
    private List<WordsImageButton> activeButtons = new List<WordsImageButton>(); 


    public virtual void Start()
    {
        if (playInstructionAudio)
        {
            selectedCharacterIndex = 0;
            AudioManager.instance.PlayFourExcess();
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
        selectedCharacterIndex = 0;
        MenuManager.showMenuOnStart = 2;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void BeginGame()
    {
        AddCharacterButtons();
        AddSelectedCharacter(characters[selectedCharacterIndex]);
        AddWordsCharacterImageButtons();
    }

    private void RemoveAll()
    {
        characterButtons.ForEach(b => Destroy(b));
        characterButtons.Clear();

        Destroy(selectedCharacterObject);

        activeButtons.ForEach(b => Destroy(b.gameObject));
        activeButtons.Clear();
    }

    private void AddCharacterButtons()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            WordsCharacterButton cb = go.AddComponent<WordsCharacterButton>();
            cb.SetStates(characterButtonsNormal[i], characterButtonsActive[i]);
            cb.character = characters[i];
            cb.onClick += WordsCharacterButtonOnClick;
            Vector3 pos = characterButtonsStartPos.position;
            pos.x = characterButtonsStartPos.position.x - (characters.Length - i - 1) * 1.1f;
            go.transform.position = pos;
            go.AddComponent<BoxCollider2D>();

            characterButtons.Add(go);
        }
    }

    private void AddSelectedCharacter(char character)
    {
        selectedCharacterObject = SynthesisAlphabet.instance.GetCharacter(character);
        selectedCharacterObject.transform.SetParent(gameObject.transform, false);
        selectedCharacterObject.transform.position = selectedCharacterPoint.position;
        selectedCharacterObject.transform.localScale = Vector3.one * 1.2f;
    }

    private void WordsCharacterButtonOnClick(WordsCharacterButton button)
    {
        if (isStarted)
        {
            selectedCharacterIndex = characters.IndexOf(button.character);
            RemoveAll();
            BeginGame();
        }
    }

    private void SelectCharacter(int index)
    {
        selectedCharacterIndex = index;
    }

    private void AddWordsCharacterImageButtons()
    {
        WordsCharacterImageButtons wcb = characterImageButtons[selectedCharacterIndex];
        int randomPos = Random.Range(0, 4);

        List<int> buttons = new List<int>();
        for (int i = 0; i < characterImageButtons[selectedCharacterIndex].normalButtons.Length; i++)
        {
            buttons.Add(i);
        }

        for (int i = 0; i < 4; i++)
        {
            GameObject go = new GameObject();
            go.AddComponent<SpriteRenderer>();
            WordsImageButton cb = go.AddComponent<WordsImageButton>();
            cb.onClick += WordsCharacterImageButtonOnClick;
            go.transform.position = treeButtonsPosition[i].position;
            if (i != randomPos)
            {
                int index = Random.Range(0, buttons.Count);
                int wrongImageIndex = buttons[index];
                buttons.RemoveAt(index);

                cb.SetStates(wcb.normalButtons[wrongImageIndex],
                    wcb.successButtons[wrongImageIndex],
                    wcb.wrongButtons[wrongImageIndex]);
                cb.isSuccess = false;
            }
            else
            {
                RandomFillSuccessImage(cb);
                cb.isSuccess = true;
            }
            go.AddComponent<BoxCollider2D>();
            activeButtons.Add(cb);
        }
    }

    private void RandomFillSuccessImage(WordsImageButton button)
    {
        char character = characters[selectedCharacterIndex];

        List<WordsCharacterImageButtons> imagesWithoutSelected = characterImageButtons.ToList();
        imagesWithoutSelected.RemoveAt(selectedCharacterIndex);

        WordsCharacterImageButtons successButton = null;
        int randomIndex = 0;
        bool haveChar = true;
        do
        {
            int successImageIndex = Random.Range(0, imagesWithoutSelected.Count);
            randomIndex = Random.Range(0, imagesWithoutSelected[successImageIndex].normalButtons.Length);
            successButton = imagesWithoutSelected[successImageIndex];
            string filename = successButton.normalButtons[randomIndex].name;
            haveChar = filename.IndexOf(character) >= 0;
        } while (haveChar);

        button.SetStates(successButton.normalButtons[randomIndex],
            successButton.successButtons[randomIndex],
            successButton.wrongButtons[randomIndex]);
    }

    private void WordsCharacterImageButtonOnClick(WordsImageButton button)
    {
        if (isStarted)
        {
            if (button.isSuccess)
            {
                AudioManager.instance.PlaySelect();
                activeButtons.ForEach(b => b.canClick = false);
                FinishGame();
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
