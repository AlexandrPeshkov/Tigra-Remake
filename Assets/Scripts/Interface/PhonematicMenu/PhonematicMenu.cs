using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhonematicMenu : MonoBehaviour
{
    public TreeBranch soundsBranch;
    public TreeBranch synthesisBranch;
    public TreeBranch analysisBranch;
    public TreeBranch wordsBranch;

    public void Reset(int excludeIndex)
    {
        soundsBranch.Reset();
        wordsBranch.Reset();
        synthesisBranch.Reset();
        analysisBranch.Reset();

        switch (excludeIndex)
        {
            case 0:
                soundsBranch.Show();
                break;
            case 1:
                wordsBranch.Show();
                break;
            case 2:
                synthesisBranch.Show();
                break;
            case 3:
                analysisBranch.Show();
                break;
        }
    }

    public void ShowSoundsBranch()
    {
        if (!soundsBranch.visible)
            soundsBranch.Show();
        else
            soundsBranch.Hide();
    }

    public void ShowWordsBranch()
    {
        if (!wordsBranch.visible)
            wordsBranch.Show();
        else
            wordsBranch.Hide();
    }

    public void ShowAnalysisBranch()
    {
        if (!analysisBranch.visible)
            analysisBranch.Show();
        else
            analysisBranch.Hide();
    }

    public void ShowSynthesisBranch()
    {
        if (!synthesisBranch.visible)
            synthesisBranch.Show();
        else
            synthesisBranch.Hide();
    }

    public void StartGame_ComingSoon()
    {
        SceneManager.LoadScene("ComingSoon");
    }

    public void StartGame_Sounds_Game_1()
    {
        SceneManager.LoadScene("Phonematic_Sounds_Game_1");
    }

    public void StartGame_Analysis_Game_1()
    {
        SceneManager.LoadScene("Phonematic_Analysis_Game_1");
    }

    public void StartGame_Synthesis_Game_1(int index)
    {
        SynthesisGameBase.gameIndex = (SynthesisGameBase.SynthesisGameIndex) index;
        SceneManager.LoadScene("Phonematic_Synthesis_Game_1");
    }

    public void StartGame_Words_Game_1()
    {
        SceneManager.LoadScene("Phonematic_Words_Game_1");
    }

    public void StartGame_Words_Game_2()
    {
        SceneManager.LoadScene("Phonematic_Words_Game_2");
    }

    public void StartGame_Words_Game_3()
    {
        SceneManager.LoadScene("Phonematic_Words_Game_3");
    }
}
