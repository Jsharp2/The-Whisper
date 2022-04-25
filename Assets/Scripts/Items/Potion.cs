using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public int amount;
    public void usePotion(GameObject target)
    {
        target.GetComponent<Friendly>().currHP += amount;
        if(target.GetComponent<Friendly>().currHP > target.GetComponent<Friendly>().maxHP)
        {
            target.GetComponent<Friendly>().currHP = target.GetComponent<Friendly>().maxHP;
        }
    }
}
