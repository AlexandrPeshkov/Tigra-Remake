using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class LexisWordsGame1House : MonoBehaviour
{
    public List<GameObject> successSigns;
    public int groupId = 0;
    public bool canClick = true;
    public UnityAction<LexisWordsGame1House> onClick;

    private int successCount = 0;

    private void Start()
    {
        Reset();
    }

    public void SuccessSignEnable()
    {
        if (successCount < successSigns.Count)
        {
            successSigns[successCount].SetActive(true);
            successCount++;
        }
    }

    public void Reset()
    {
        successSigns.ForEach(s => s.SetActive(false));
        successCount = 0;
    }

    public void OnMouseUpAsButton()
    {
        if (canClick)
        {
            if (onClick != null)
                onClick.Invoke(this);
        }
    }
}
