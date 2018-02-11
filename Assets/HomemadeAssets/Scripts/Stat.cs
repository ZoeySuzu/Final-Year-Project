using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat {
    public string statName { get; set; }
    public int max { get; set; }
    private int _statValue;

    public int statValue {
        get { return _statValue; }
        set { if (0 < value && value < max) _statValue = value; }
    }

    public Stat(string _name, int _max)
    {
        statName = _name; _statValue = max = _max;

    }

    public void changeStat(int difference)
    {
        int tempValue = _statValue + difference;
        if(tempValue < 0)
        {
            _statValue = 0;
        }
        else if( tempValue > max)
        {
            _statValue = max;
        }
        else
        {
            _statValue = tempValue;
        }
    }

}
