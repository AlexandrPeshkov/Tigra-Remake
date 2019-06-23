using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisAlphabet : MonoBehaviour
{
    public static SynthesisAlphabet instance;

    public Sprite[] characterSprites;
    private string alphabet = "абвгдеёжзийклмнопрстуфхцчшщьыъэюя";

    private void Awake()
    {
        instance = this;
    }

    public GameObject GetCharacter(char character)
    {
        int index = alphabet.LastIndexOf(character);
        GameObject go = new GameObject();
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = characterSprites[index];
        sr.sortingLayerName = "Foreground";
        sr.sortingOrder = 2;

        return go;
    }
}
