using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class WordsGame2 : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 1;

    public GameObject[] circles;
    public Transform[] buttonsPoints;
    public Words2Image[] circleImages;
    public Words2Wheel wheel;

    private List<Words2ImageButton> circleButtons = new List<Words2ImageButton>();
    private List<Words2ImageButton> answerButtons = new List<Words2ImageButton>();

    public virtual void Start()
    {
        wheel.onFinish += WheelOnFinish;

        if (playInstructionAudio)
        {
            AudioManager.instance.PlaySunClock();
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
        MenuManager.showMenuOnStart = 2;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void BeginGame()
    {
        AddCircleImages();
        AddAnswerButtons();
    }

    private void WheelOnFinish(int arg0)
    {
        AddAnswerButtons();
    }

    private void RemoveAnswerButtons()
    {
        answerButtons.ForEach((b) => Destroy(b.gameObject));
        answerButtons.Clear();
    }

    private void AddAnswerButtons()
    {
        RemoveAnswerButtons();
        List<Words2Image> temp = circleImages.ToList();
        Words2Image btn = temp.First((b) => b.id == circleButtons[wheel.activeCircle].prefab.id);
        temp.Remove(btn);
        btn = temp.First((b) => b.id == circleButtons[wheel.activeCircle].prefab.pair.id);
        temp.Remove(btn);

        int randomIndex = UnityEngine.Random.Range(0, 3);
        for (int i = 0; i < 3; i++)
        {
            Words2Image prefab = null;
            GameObject go = null;
            if (i != randomIndex)
            {
                int ci = UnityEngine.Random.Range(0, temp.Count);
                prefab = temp[ci];
                temp.RemoveAt(ci);
            }
            else
            {
                prefab = circleButtons[wheel.activeCircle].prefab.pair;
            }
            go = Instantiate(prefab.gameObject);
            Words2Image img = go.GetComponent<Words2Image>();
            Words2ImageButton button = go.AddComponent<Words2ImageButton>();
            button.SetStates(img.normal, img.down, img.wrong);
            button.index = i;
            button.prefab = prefab;
            button.onClick += AnswerButtonOnClick;
            go.transform.position = buttonsPoints[i].position;
            go.transform.localScale = new Vector3(0.807f, 0.807f, 0.807f);

            answerButtons.Add(button);
        }
    }

    private void AnswerButtonOnClick(Words2ImageButton button)
    {
        if (isStarted)
        {
            if (button.prefab.pair.id == circleButtons[wheel.activeCircle].prefab.id)
            {
                AudioManager.instance.PlaySelect();
                circleButtons.ForEach(b => b.canClick = false);
                answerButtons.ForEach(b => b.canClick = false);
                FinishGame();
            }
            else
            {
                button.SetIsWrong();
                AudioManager.instance.PlayWrong();
            }
        }
    }

    private void AddCircleImages()
    {
        List<Words2Image> temp = circleImages.ToList();

        for (int i = 0; i < 6; i++)
        {
            int index = UnityEngine.Random.Range(0, temp.Count);

            GameObject go = Instantiate(temp[index].gameObject);
            Words2Image img = go.GetComponent<Words2Image>();
            Words2ImageButton button = go.AddComponent<Words2ImageButton>();
            button.SetStates(img.normal, img.down, img.wrong);
            button.index = i;
            button.prefab = temp[index];
            button.onClick += CircleImgageButtononClick;
            go.transform.localScale = new Vector3(0.6248479f, 0.6248479f, 0.6248479f);

            circleButtons.Add(button);
            temp.RemoveAt(index);
        }

        wheel.SetWheelButtons(circleButtons);
    }

    private void CircleImgageButtononClick(Words2ImageButton button)
    {
        wheel.Rotate(button.index);
    }

    private IEnumerator BeginShowWinPanel()
    {
        yield return new WaitForSeconds(1.0f);

        ShowWinPanel();
    }
}
