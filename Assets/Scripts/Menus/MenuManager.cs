using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages navigation through the menu system
/// </summary>
public static class MenuManager
{
    /// <summary>
    /// Goes to the menu with the given name
    /// </summary>
    /// <param name="name">name of the menu to go to</param>
    public static void GoToMenu(MenuName name)
    {
        switch (name)
        {
            //case MenuName.Difficulty:

            //    // go to DifficultyMenu scene
            //    SceneManager.LoadScene("DifficultyMenu");
            //    break;
            case MenuName.Gameplay:
                SceneManager.LoadScene("World");
                break;

            case MenuName.Main:

                // go to MainMenu scene
                SceneManager.LoadScene("Title");
                break;

            case MenuName.Instructions:

                // deactivate MainMenuCanvas and instantiate prefab
                GameObject mainMenuCanvas = GameObject.Find("Canvas");
                if (mainMenuCanvas != null)
                {
                    mainMenuCanvas.SetActive(false);
                }
                Object.Instantiate(Resources.Load("Instructions"));
                break;
            case MenuName.Pause:

                if (GameObject.Find("Pause Menu(Clone)") == null)
                {
                    // instantiate prefab
                    Object PauseMenu = Object.Instantiate(Resources.Load("Pause Menu"));
                }
                else
                {
                    Object.Destroy(GameObject.Find("Pause Menu(Clone)"));
                    Time.timeScale = 1;
                }
                break;

            case MenuName.GameOver:
                SceneManager.LoadScene("GameOver");
                GameManager.instance.hasKey = false;
                GameManager.instance.DeleteEnemies();
                break;

            case MenuName.Victory:
                SceneManager.LoadScene("Victory");
                GameManager.instance.hasKey = false;
                GameManager.instance.DeleteEnemies();
                break;
        }
    }
}