using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The Types that attacks and characters can be
/// </summary>
public enum Type
{
    NORMAS,
    AUQAS,
    IGNIS,
    NATURA,
    LUMON,
    MORTON
}


//Type Chart loosly based around Pokemon's design
//          Aquas   Ignis	Natura	 Normas  Lumon	 Morton	 Defending
//Aquas	     0.6     1.5      0.6       1       1      0.6
//Ignis	     0.6     0.6      1.5       1       1      0.6
//Natura	 1.5     0.6      0.6       1       1      0.6
//Normas	  1       1        1        1       1       1 
//Lumon	      1      0.6      0.6       1      0.6     1.5
//Morton	  1       1        1        1      0.6     1.5
//Attacking							
