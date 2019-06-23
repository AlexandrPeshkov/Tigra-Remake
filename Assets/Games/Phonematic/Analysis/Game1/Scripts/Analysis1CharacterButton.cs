using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Analysis1CharacterButton : MonoBehaviour
{
    public Sprite normalState;
    public Sprite downState;
    public Sprite leftCharacterImage;
    public Sprite rightCharacterImage;
    public Words3Images leftImages;
    public Words3Images rightImages;
    public bool canClick = true;
    public UnityAction<Analysis1CharacterButton> onClick;
    public int index = 0;

    private SpriteRenderer spriteRenderer;

    public void Create(Sprite normal, Sprite down)
    {
        normalState = normal;
        downState = down;
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = "Interface";
        spriteRenderer.sortingOrder = 1;
        spriteRenderer.sprite = normal;
        gameObject.AddComponent<BoxCollider2D>();
    }

    public void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
            {
                spriteRenderer.sprite = downState;
            }
        }
    }

    public void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
            {
            }
        }
    }

    public void OnMouseUpAsButton()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (canClick)
            {
                if (onClick != null)
                    onClick.Invoke(this);
            }
        }
    }

    public void SetIsDown()
    {
        spriteRenderer.sprite = downState;
    }

    public void SetIsNormal()
    {
        spriteRenderer.sprite = normalState;
    }
}
