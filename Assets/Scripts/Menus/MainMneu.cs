using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMneu : MonoBehaviour
{
    private void Start()
    {
        //AudioManager.ToggleLoop(true);
    }

    //Sends the player to the Gameplay area
    public void HandlePlayButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuName.Gameplay);
        
    }

    /// <summary>
    /// Sends the player to the instructions area
    /// </summary>
    public void HandleInstructionsButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuName.Instructions);
        //AudioManager.Play(AudioClipName.Fireball);
    }

    /// <summary>
    /// Sends the player to the instructions area
    /// </summary>
    public void HandleResourcesButtonOnClickEvent()
    {
        MenuManager.GoToMenu(MenuName.Resources);
    }

    /// <summary>
    /// Ends the game
    /// </summary>
    public void HandleEndButtonOnClickEvent()
    {
        Application.Quit();
    }
}
