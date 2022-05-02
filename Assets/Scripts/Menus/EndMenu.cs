using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        MenuManager.GoToMenu(MenuName.Main);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void GotToEndScreen()
    {
        MenuManager.GoToMenu(MenuName.GameOver);
    }

    public void ReturnToWorld()
    {
        MenuManager.GoToMenu(MenuName.Gameplay);
    }

}
