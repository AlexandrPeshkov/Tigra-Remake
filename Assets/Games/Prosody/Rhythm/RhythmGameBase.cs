using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGameBase : ProsodyGameBase
{
    public RhythmSystem rhythmSystem;

    public override void Start()
    {
        rhythmSystem.onBeepIntersect += BeepIntersect;
        base.Start();
        submenuIndex = 2;
    }

    protected virtual void BeepIntersect()
    {

    }

    protected override void GameStarted()
    {
        rhythmSystem.BeginRhythm();
    }

    protected override void ShowWinPanel()
    {
        rhythmSystem.StopRhythm();
        base.ShowWinPanel();
    }

    public override void GoToMainMenu()
    {
        rhythmSystem.StopRhythm();
        base.GoToMainMenu();
    }
}
