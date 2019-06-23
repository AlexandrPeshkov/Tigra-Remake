using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class WordsImageButton : MonoBehaviour
{
    public Sprite normalState;
    public Sprite successState;
    public Sprite wrongState;
    public bool isSuccess = false;
    [HideInInspector]
    public UnityAction<WordsImageButton> onClick;
    public bool canClick = true;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetStates(Sprite normal, Sprite success, Sprite wrong)
    {
        normalState = normal;
        successState = success;
        wrongState = wrong;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalState;
        spriteRenderer.sortingLayerName = "Interface";
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
            {
                spriteRenderer.sprite = isSuccess ? successState : wrongState;

                if (onClick != null)
                    onClick.Invoke(this);
            }
        }
    }
}
