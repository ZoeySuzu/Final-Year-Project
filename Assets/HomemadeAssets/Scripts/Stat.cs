using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {
    public string statName { get; set; }
    public int statMax { get; set; }
    private int value;

    public int statValue {
        get { return value; }
        set { if (0 < value && value < statMax) this.value = value; }
    }

    public Stat(string _name, int _max)
    {
        statName = _name; value = statMax = _max;

    }

    public void changeStat(int difference)
    {
        int tempValue = value + difference;
        if(tempValue < 0)
        {
            value = 0;
        }
        else if( tempValue > statMax)
        {
            value = statMax;
        }
        else
        {
            value = tempValue;
        }
    }

}
