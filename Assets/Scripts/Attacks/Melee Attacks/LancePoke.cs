using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The stat for Lance Poke
/// </summary>
public class LancePoke : Attack
{
    public LancePoke()
    {
        attackName = "Lance Poke";
        attackDescription = "User attacks with a lance to deal damage";
        attackDamage = 15;
        attackCost = 0;
    }
}
