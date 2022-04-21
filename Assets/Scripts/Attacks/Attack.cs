using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    /// <summary>
    /// If it should attack one or multiple targets
    /// </summary>
    public enum Target
    {
        Single,
        Multiple
    }

    /// <summary>
    /// If it's physical or magical attack
    /// </summary>
    public enum Style
    {
        Phyiscal,
        Magical
    }
    //Name of attack
    [SerializeField]
    public string attackName = "Attack Name";

    //Vanilla text of attack
    [SerializeField]
    public string attackDescription = "Attack Description";

    //Base damage
    [SerializeField]
    public int attackDamage = 0;

    //Cost for attack (not currently implemented, but will at least use MP)
    [SerializeField]
    public int attackCost = 0;

    //Typing attack
    [SerializeField]
    public Type type = Type.NORMAS;

    [SerializeField]
    public Target target = Target.Single;

    [SerializeField]
    public Style style = Style.Phyiscal;

}
