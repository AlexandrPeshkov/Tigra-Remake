﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftMenu : MonoBehaviour
{

    public void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
