using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //The type of the character
    [SerializeField] public Type type;

    //Their HP values
    [SerializeField] public int maxHP;
    [SerializeField] public int currHP;

    //Attack values
    [SerializeField] public int attack;
    [SerializeField] public int currAttack;

    //Defense values
    [SerializeField] public int defense;
    [SerializeField] public int currDefense;

    //Magic values
    [SerializeField] public int magic;
    [SerializeField] public int currMagic;

    //Magic Points values
    [SerializeField] public int currMP;
    [SerializeField] public int maxMP;

    //How much charge they need
    [SerializeField] public float maxCharge = 5f;
    public float curCharge = 0;

    //A list of thier attacks
    public List<Attack> attacks = new List<Attack>();

    //A list of their magic attacks
    public List<Attack> magicAttacks = new List<Attack>();


    // Start is called before the first frame update
    public void Start()
    {
        //Sets all their stats to be max at start
        currAttack = attack;
        currDefense = defense;

        //Gives a random amonut towards charge for first attack
        curCharge = Random.Range(0, maxCharge/3);
    }
}
