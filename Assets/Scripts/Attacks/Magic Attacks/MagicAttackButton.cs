using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Specific button for when magic attacks are choosen due to how different they are from attacks
/// </summary>
public class MagicAttackButton : MonoBehaviour
{
    public Attack magicAttackToPerform;

    private bool showSelector = true;

    //Sends the attack the player choose to the battle manager
    public void CastMagicAttack()
    {
        GameObject.Find("BattleManager").GetComponent<BattleManager>().MagictoUse(magicAttackToPerform);
    }

    public void ToggleSelector()
    {
        if(magicAttackToPerform.target == Attack.Target.Multiple)
        {
            foreach(GameObject enemy in GameObject.Find("BattleManager").GetComponent<BattleManager>().Enemies)
            {
                enemy.transform.Find("EnemyHealthBar").gameObject.SetActive(true);
                enemy.transform.Find("Background").gameObject.SetActive(true);
                float elemintalModifier = GameObject.Find("GameManager").GetComponent<GameManager>().Effectiveness(magicAttackToPerform.type, enemy.GetComponent<Enemy>().type);
                if (elemintalModifier == 1.5)
                {
                    enemy.transform.Find("Good").gameObject.SetActive(true);
                }
                else if (elemintalModifier == (2f / 3))
                {
                    enemy.transform.Find("Not").gameObject.SetActive(true);
                }
            }
        }

        if (showSelector)
        {
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").gameObject.SetActive(true);
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").transform.Find("Spell Name").gameObject.GetComponent<Text>().text = magicAttackToPerform.attackName;
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").transform.Find("Spell Description").gameObject.GetComponent<Text>().text = magicAttackToPerform.attackDescription + "\n Cost: " +magicAttackToPerform.attackCost.ToString() + "MP" + "\n Type: " + magicAttackToPerform.type;

        }
        else
        {
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").gameObject.SetActive(false);
            foreach(GameObject enemy in GameObject.Find("BattleManager").GetComponent<BattleManager>().Enemies)
            {
                enemy.transform.Find("EnemyHealthBar").gameObject.SetActive(false);
                enemy.transform.Find("Background").gameObject.SetActive(false);
                enemy.transform.Find("Good").gameObject.SetActive(false);
                enemy.transform.Find("Not").gameObject.SetActive(false);
            }
        }
        showSelector = !showSelector;
    }
}
