using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<GameObject, int> healingItems = new Dictionary<GameObject, int>();
    public GameObject Potion;
    public GameObject magicPotion;
    public GameObject Revive;
    // Start is called before the first frame update
    void Start()
    {
        healingItems[Potion] = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
