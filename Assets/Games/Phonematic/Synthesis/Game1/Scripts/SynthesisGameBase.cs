using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class SynthesisGameBase : MonoBehaviour
{
    protected static bool startAfterLoad = true;
    protected static bool playInstructionAudio = true;

    public GameObject winPanel;
    protected bool isStarted = false;
    protected int submenuIndex = 2;

    public static SynthesisGameIndex gameIndex = SynthesisGameIndex.Word3;
    public SynthesisTree[] treePrefabs;
    public AntLeaf leafPrefab;
    public Transform[] leafPoints;

    private SynthesisTree activeTree;
    private List<AntLeaf> antLeafs;


    private int CharacterCount { get { return (int) gameIndex + 3;  } }

    public virtual void Start()
    {
        if (playInstructionAudio)
        {
            AudioManager.instance.PlayBuildWord();
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
        gameIndex = SynthesisGameIndex.Word3;
        MenuManager.showMenuOnStart = 2;
        MenuManager.showSubmenuOnStart = submenuIndex;
        SceneManager.LoadScene("MainMenu");
    }

    private void BeginGame()
    {
        antLeafs = new List<AntLeaf>();

        AddTree();
        AddAntLeafs();
    }

    private void AddTree()
    {
        SynthesisTree prefab = treePrefabs.First(t => t.gameIndex == gameIndex);

        if (prefab != null)
        {
            GameObject go = Instantiate(prefab.gameObject);
            activeTree = go.GetComponent<SynthesisTree>();
            activeTree.GenerateWord();
        }
    }

    private void AddAntLeafs()
    {
        List<Transform> leafPositions = leafPoints.ToList();

        string word = activeTree.Word;
        for (int i = 0; i < word.Length; i++)
        {
            char ch = word[i];

            GameObject go = Instantiate(leafPrefab.gameObject);
            AntLeaf al = go.GetComponent<AntLeaf>();
            int pointIndex = Random.Range(0, leafPositions.Count);
            al.transform.position = leafPositions[pointIndex].position;
            al.character = ch;
            float rotate = Random.Range(0, 360);
            go.transform.Rotate(0, 0, rotate);
            al.OnClick.AddListener(() => AntLeafOnClick(al));

            GameObject character = SynthesisAlphabet.instance.GetCharacter(ch);
            character.transform.SetParent(go.transform, false);
            character.transform.Translate(new Vector3(0, -0.15f, 0));
            character.transform.Rotate(0, 0, -rotate);

            antLeafs.Add(al);
            leafPositions.RemoveAt(pointIndex);
        }
    }

    private void AntLeafOnClick(AntLeaf leaf)
    {
        if (activeTree.ClickOnLeaf(leaf.character))
        {
            leaf.Hide();
            AudioManager.instance.PlaySelect();
        }
        else
        {
            AudioManager.instance.PlayWrong();
        }

        if (activeTree.IsWin())
            FinishGame();
    }

    private IEnumerator BeginShowWinPanel()
    {
        yield return new WaitForSeconds(1.0f);

        ShowWinPanel();
    }

    public enum SynthesisGameIndex
    {
        Word3,
        Word4,
        Word5,
        Word6
    }
}
