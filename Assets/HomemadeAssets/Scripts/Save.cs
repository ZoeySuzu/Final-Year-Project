using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Save
{
    public List<FriendPoints> relationshipValues = new List<FriendPoints>();
    public List<Stat> PlayerStats = new List<Stat>();
    public List<Level> LevelList = new List<Level>(); 
    public Inventory inventory = null;
    public Unlocks unlocks = null;
}
