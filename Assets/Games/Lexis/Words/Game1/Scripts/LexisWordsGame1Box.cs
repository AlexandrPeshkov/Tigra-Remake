using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LexisWordsGame1Box : MonoBehaviour
{
    public SpriteRenderer image;
    public int groupId = 0;

    public void SetImage(int groupId, Sprite sprite)
    {
        this.groupId = groupId;
        image.sprite = sprite;
    }
}
