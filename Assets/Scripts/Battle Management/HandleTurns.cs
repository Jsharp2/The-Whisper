using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HandleTurns
{
    public string AttackerName; //Name of attacker
    public string Type;
    public GameObject Attacker; //Character attacked
    public GameObject Attacked; //Who is attacking

    public Attack choosenAttack;
}
