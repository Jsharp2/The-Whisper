using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnEffectiveness : MonoBehaviour
{
    public float Effectiveness(Type attackType, Type targetType)
    {
        if ((attackType == Type.AUQAS && targetType == Type.IGNIS) ||
            (attackType == Type.IGNIS && targetType == Type.NATURA) ||
            (attackType == Type.NATURA && targetType == Type.AUQAS) ||
            (attackType == Type.LUMON && targetType == Type.IGNIS) || (attackType == Type.LUMON && targetType == Type.NATURA) || (attackType == Type.LUMON && targetType == Type.LUMON) ||
            (attackType == Type.MORTON && targetType == Type.LUMON))
        {
            return 1.5f;
        }
        else if ((attackType == Type.AUQAS && targetType == Type.AUQAS) || (attackType == Type.AUQAS && targetType == Type.NATURA) || (attackType == Type.AUQAS && targetType == Type.MORTON) ||
            (attackType == Type.IGNIS && targetType == Type.IGNIS) || (attackType == Type.IGNIS && targetType == Type.AUQAS) || (attackType == Type.AUQAS && targetType == Type.MORTON) ||
            (attackType == Type.NATURA && targetType == Type.IGNIS) || (attackType == Type.NATURA && targetType == Type.NATURA) || (attackType == Type.NATURA && targetType == Type.MORTON) ||
            (attackType == Type.LUMON && targetType == Type.IGNIS) || (attackType == Type.LUMON && targetType == Type.NATURA) || (attackType == Type.LUMON && targetType == Type.LUMON) ||
            (attackType == Type.MORTON && targetType == Type.LUMON))
        {
            return (2f / 3);
        }
        else
        {
            return 1;
        }
    }
}
