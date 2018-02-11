using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Save
{
    public List<FriendPoints> relationshipValues = new List<FriendPoints>();
    public List<Stat> PlayerStats = new List<Stat>();
    public Inventory inventory = null;
}
