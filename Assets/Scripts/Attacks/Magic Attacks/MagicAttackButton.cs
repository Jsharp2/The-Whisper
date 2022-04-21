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
        if (showSelector)
        {
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").gameObject.SetActive(true);
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").transform.Find("Spell Name").gameObject.GetComponent<Text>().text = magicAttackToPerform.attackName;
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").transform.Find("Spell Description").gameObject.GetComponent<Text>().text = magicAttackToPerform.attackDescription + "\n Cost: " +magicAttackToPerform.attackCost.ToString() + "MP" + "\n Type: " + magicAttackToPerform.type;

        }
        else
        {
            transform.parent.transform.parent.transform.parent.gameObject.transform.Find("Spell Desciption Panel").gameObject.SetActive(false);
        }
        showSelector = !showSelector;
    }
}
