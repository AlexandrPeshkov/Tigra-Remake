using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhythm_Game2 : RhythmGameBase
{
    public GameObject[] blocks;
    public Transform blockStartPos;
    public Transform blockFinishPos;

    private int currentBlock = 0;

    public override void Start()
    {
        base.Start();

        foreach(GameObject block in blocks)
        {
            block.SetActive(false);
            Vector2 pos = block.transform.position;
            pos.y = blockStartPos.position.y;
            block.transform.position = pos;
        }
    }

    public void AddNewBlock()
    {
        if (currentBlock < blocks.Length)
        {
            blocks[currentBlock].SetActive(true);
            currentBlock++;
        }
    }

    protected override void BeepIntersect()
    {
        AddNewBlock();
    }

    public override void Update()
    {
        base.Update();

        if (isStarted &&
            (blocks[blocks.Length - 1].activeSelf) && 
            (blocks[blocks.Length - 1].transform.position.y < blockFinishPos.position.y))
        {
            FinishGame();
        }
    }
}
