using System;
using UnityEngine;

public class MinigameDisplay : Display
{
    public override void UpdateDisplay(int p_operation, float p_value, float p_data)
    {
        switch (p_operation)
        {
            case 0:
                GetComponent<Animator>().SetInteger("State", (int)p_value);
                break;
        }
    }
}
