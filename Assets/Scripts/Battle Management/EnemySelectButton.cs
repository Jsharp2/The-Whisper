using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelectButton : MonoBehaviour
{
    //Holds which enemy it's realted to 
    public GameObject Enemy;

    //Hanldes selector over enemies
    private bool showSelector = true;

    //The player's descions about which enemy to attack
    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<BattleManager>().EnemytoAttack(Enemy);
    }

    //Turns on and off the indicator for an enemy that is selected
    public void ToggleSelector()
    {
        if (showSelector)
        {
            Enemy.transform.Find("EnemyHealthBar").gameObject.SetActive(true);
            Enemy.transform.Find("Background").gameObject.SetActive(true);
            float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(GameObject.Find("BattleManager").GetComponent<BattleManager>().attackType, Enemy.GetComponent<Enemy>().type);
            if(elemintalModifier == 1.5)
            {
                Enemy.transform.Find("Good").gameObject.SetActive(true);
            }
            else if (elemintalModifier == (2f/3))
            {
                Enemy.transform.Find("Not").gameObject.SetActive(true);
            }

        }
        else
        {
            Enemy.transform.Find("EnemyHealthBar").gameObject.SetActive(false);
            Enemy.transform.Find("Background").gameObject.SetActive(false);
            Enemy.transform.Find("Good").gameObject.SetActive(false);
            Enemy.transform.Find("Not").gameObject.SetActive(false);
        }
        showSelector = !showSelector;
    }
}
