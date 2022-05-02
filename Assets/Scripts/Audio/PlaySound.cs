using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaySound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name == "Title")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Title);
        }
        else if (SceneManager.GetActiveScene().name == "World")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Overworld);
        }
        else if (SceneManager.GetActiveScene().name == "Combat")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Battle);
        }
        else if (SceneManager.GetActiveScene().name == "Forest")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Forest);
        }
        else if (SceneManager.GetActiveScene().name == "Runswick")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Village);
        }
        else if (SceneManager.GetActiveScene().name == "Victory")
        {
            AudioManager.Stop();
            AudioManager.Play(AudioClipName.Victory);
        }
    }
}
