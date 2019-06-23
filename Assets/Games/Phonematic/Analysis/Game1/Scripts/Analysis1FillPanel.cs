using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Analysis1FillPanel : MonoBehaviour
{
    public GameObject[] checkBoxes;

    public void FillOne()
    {
        if (!AllIsFilled())
        {
            foreach (GameObject go in checkBoxes)
            {
                if (!go.activeSelf)
                {
                    go.SetActive(true);
                    break;
                }
            }
        }
    }

    public void Reset()
    {
        foreach (GameObject go in checkBoxes)
        {
            go.SetActive(false);
        }
    } 

    public bool AllIsFilled()
    {
        bool b = true;
        foreach (GameObject go in checkBoxes)
        {
            if (!go.activeSelf)
            {
                b = false;
                break;
            }
        }

        return b;
    }
}
