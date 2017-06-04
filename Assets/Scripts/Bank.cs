using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : GameManager {

    public int Pot = 0;

    public void AddToPot(int value)
    {
        Pot += value;
    }

    public void SubstractFromPot(int value)
    {
        Pot -= value;
        if (Pot < 0)
        {
            Pot = 0;
        }
    }
}
