using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProsodyMenu : MonoBehaviour {
    public GameTable gameTable;

    public void StartGame_Breath_Game_1()
    {
        SceneManager.LoadScene("Breath_Game_1");
    }

    public void StartGame_Breath_Game_2()
    {
        SceneManager.LoadScene("Breath_Game_2");
    }

    public void StartGame_Rhythm_1()
    {
        SceneManager.LoadScene("Rhythm_Game_1");
    }

    public void StartGame_Rhythm_2()
    {
        SceneManager.LoadScene("Rhythm_Game_2");
    }

    public void StartGame_Fusion_1()
    {
        SceneManager.LoadScene("Fusion_Game_1");
    }

    public void StartGame_Fusion_2()
    {
        SceneManager.LoadScene("Fusion_Game_2");
    }

    public void StartGame_Timbre_1()
    {
        SceneManager.LoadScene("Timbre_Game_1");
    }

    public void StartGame_Timbre_2()
    {
        SceneManager.LoadScene("Timbre_Game_2");
    }

    public void Show(int menuIndex = 0)
    {
        gameTable.ShowFirstMenu(menuIndex);
    }

    public void ShowGameTable_Breath()
    {
        if (gameTable.CanClick())
        {
            gameTable.ShowBreathMenu();
        }
    }

    public void ShowGameTable_Fusion()
    {
        if (gameTable.CanClick())
        {
            gameTable.ShowFusionMenu();
        }
    }

    public void ShowGameTable_Rhythm()
    {
        if (gameTable.CanClick())
        {
            gameTable.ShowRhythmMenu();
        }
    }

    public void ShowGameTable_Timbre()
    {
        if (gameTable.CanClick())
        {
            gameTable.ShowTimbreMenu();
        }
    }
}
