using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SynthesisTree : MonoBehaviour
{
    public SynthesisGameBase.SynthesisGameIndex gameIndex;
    public List<WordPair> words;
    public SynthesisTreeLeaf[] leafs;
    public Transform imagePosition;
    [HideInInspector]
    public WordPair gameWord;
    public GameObject image;

    private int wordIndex;
    private int CharacterCount { get { return (int)gameIndex + 3; } }
    public string Word { get { return gameWord.word;  } }

    void Start () {
		
	}

	void Update () {
		
	}

    public void GenerateWord()
    {
        wordIndex = Random.Range(0, words.Count);
        gameWord = words[wordIndex];

        image = new GameObject();
        image.transform.SetParent(transform, true);
        image.transform.position = imagePosition.position;
        SpriteRenderer sr = image.AddComponent<SpriteRenderer>();
        sr.sprite = gameWord.sprite;
        sr.sortingLayerName = "Foreground";
    }

    public bool ClickOnLeaf(char character)
    {
        int chIndex = Word.IndexOf(character);
        if (chIndex == 0)
        {
            List<SynthesisTreeLeaf> l = leafs.ToList();
            l[0].Hide();
            l.RemoveAt(0);
            leafs = l.ToArray();
            gameWord.word = gameWord.word.Remove(chIndex, 1);

            return true;
        }

        return false;
    }

    public bool IsWin()
    {
        return gameWord.word.Length == 0;
    }
}
