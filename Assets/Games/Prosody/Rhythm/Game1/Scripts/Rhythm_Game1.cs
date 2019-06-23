using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rhythm_Game1 : RhythmGameBase
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject[] lamps;
    public Sprite lampUnactive;
    public Sprite lampActive;

    public override void Start()
    { 
        leftDoor.GetComponent<Door>().onDoorsOpened += ShowWinPanel;
        base.Start();
    }

    protected override void BeepIntersect()
    {
        OpenDoors();
    }

    private void OpenDoors()
    {
        leftDoor.GetComponent<Door>().OpenDoor();
        rightDoor.GetComponent<Door>().OpenDoor();
    }
}
