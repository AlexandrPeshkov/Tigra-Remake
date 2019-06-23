using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WordsCharacterButton : MonoBehaviour
{
    public char character;
    public Sprite normalState;
    public Sprite activeState;
    [HideInInspector]
    public UnityAction<WordsCharacterButton> onClick;
    public bool canClick = true;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetStates(Sprite normal, Sprite active)
    {
        normalState = normal;
        activeState = active;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalState;
        spriteRenderer.sortingLayerName = "Interface";
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
                spriteRenderer.sprite = activeState;
        }
    }

    private void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
            {
                spriteRenderer.sprite = normalState;

                if (onClick != null)
                    onClick.Invoke(this);
            }
        }
    }

    void Update () {
		
	}
}
