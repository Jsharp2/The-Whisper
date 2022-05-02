using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    //Handles the quit button for the game
    public void HandleQuitButtonOnClickEvent()
    {
        // unpause game, destroy menu, and go to main menu
        Time.timeScale = 1;
        Debug.Log(gameObject);
        Destroy(gameObject);
        Object.Instantiate(Resources.Load("TitleCanvas"));
    }
}
