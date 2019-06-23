using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComingSoon : MonoBehaviour {

    public virtual void GoToMainMenu()
    {
        MenuManager.showMenuOnStart = 2;
        MenuManager.showSubmenuOnStart = 0;
        SceneManager.LoadScene("MainMenu");
    }
}
