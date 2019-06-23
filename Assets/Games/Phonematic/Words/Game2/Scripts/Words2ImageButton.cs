using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Words2ImageButton : MonoBehaviour
{

    public Sprite normalState;
    public Sprite downState;
    public Sprite wrongState;
    public bool isSuccess = false;
    [HideInInspector]
    public UnityAction<Words2ImageButton> onClick;
    public bool canClick = true;
    public int index;
    public Words2Image prefab;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void SetStates(Sprite normal, Sprite success, Sprite wrong)
    {
        normalState = normal;
        downState = success;
        wrongState = wrong;
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = normalState;
        spriteRenderer.sortingLayerName = "Foreground";
        spriteRenderer.sortingOrder = 5;
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
                spriteRenderer.sprite = normalState;
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

    public void SetIsWrong()
    {
        spriteRenderer.sprite = wrongState;
        canClick = false;
    }
}
