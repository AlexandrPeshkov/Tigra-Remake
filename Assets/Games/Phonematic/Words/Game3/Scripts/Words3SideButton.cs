using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Words3SideButton : MonoBehaviour
{
    public bool isLeft;
    public Transform point;
    public UnityAction<Words3SideButton> onClick;
    [HideInInspector]
    public bool canClick = true;

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
}
