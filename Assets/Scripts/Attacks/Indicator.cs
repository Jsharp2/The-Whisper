using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    //Max time allowed on screen
    [SerializeField]
    float maxTime = 1f;

    private void Start()
    {
        //Destorys instance once max time is reached
        Destroy(gameObject, maxTime);
    }
}
