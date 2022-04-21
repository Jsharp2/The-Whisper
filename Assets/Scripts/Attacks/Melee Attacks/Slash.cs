using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic melee attack for enemies and allies
/// </summary>
public class Slash : Attack
{
    public Slash()
    {
        attackName = "Slash";
        attackDescription = "Attack quickly with their weapon.";
        attackDamage = 10;
        attackCost = 0;
    }
}
