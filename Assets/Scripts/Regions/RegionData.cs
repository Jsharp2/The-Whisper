using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionData : MonoBehaviour
{
    //Max number of enemies allowed to spawn
    public int maxEnemies = 4;

    //The scene they will go to for a fight (if I add multiple scenes)
    public string battleScene;

    //The list of possible enemies to encounter
    public List<GameObject> possibleEnemies = new List<GameObject>();
}
