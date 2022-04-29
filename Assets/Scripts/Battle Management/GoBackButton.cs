using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButton : MonoBehaviour
{
    public GameObject lastScreen;

    public void GoBack()
    {
        Debug.Log(lastScreen);
        //lastScreen.SetActive(true);
        Debug.Log(gameObject.transform.parent);
        lastScreen.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
