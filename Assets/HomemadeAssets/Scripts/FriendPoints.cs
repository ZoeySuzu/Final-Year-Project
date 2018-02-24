using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendPoints{

    public string name { get; private set; }
    public int value { get { return value; } set { if (-999 < value && value < 999) this.value = value; } }

    public FriendPoints(string _name, int _value)
    {
        name = _name;
        value = _value;
    }

    public bool checkFriendPoints(bool _higher, int _value)
    {
        if (_higher)
        {
            if (value >= _value) return true;
        }
        else
        {
            if (value < _value) return true;
        }
        return false;
    }

    public void changeStat(int difference)
    {
        int tempValue = value + difference;
        if (tempValue < -999)
        {
            value = -999;
        }
        else if (tempValue > 999)
        {
            value = 999;
        }
        else
        {
            value = tempValue;
        }
    }
}
