using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundPronunciationMenu : MonoBehaviour
{
    private GameObject activeMenuItems;
    public List<GameObject> menuItems;
    public List<SoundPronunciationMenuItem> menuItemButtons;

    private void Awake()
    {
        AddListeners();
    }

    public void SetActiveItems(int value)
    {
        if (activeMenuItems != null)
        {
            activeMenuItems.SetActive(false);
        }
        activeMenuItems = menuItems[value];
        activeMenuItems.SetActive(true);
    }

    public void Reset()
    {
        menuItems.ForEach(m => m.SetActive(false));
        activeMenuItems = null;
    }

    private void AddListeners()
    {
        foreach(SoundPronunciationMenuItem item in menuItemButtons)
        {
            Button button = item.gameObject.GetComponent<Button>();
            button.onClick.AddListener(delegate { OnMenuButtonClick(item); });
        }
    }

    private void OnMenuButtonClick(SoundPronunciationMenuItem item)
    {
        SoundPronunciationGame1.startGroupId = item.group;
        SoundPronunciationGame1.startGameId = item.itemId;

        SceneManager.LoadScene("SoundPronunciation_Game_1");
    }
}
