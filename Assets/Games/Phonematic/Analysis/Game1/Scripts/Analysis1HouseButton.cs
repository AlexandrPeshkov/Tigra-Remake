using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Analysis1HouseButton : MonoBehaviour
{
    public int index = 0;
    public bool canClick = true;
    public UnityAction<Analysis1HouseButton> onClick;

    public void OnMouseDown()
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
