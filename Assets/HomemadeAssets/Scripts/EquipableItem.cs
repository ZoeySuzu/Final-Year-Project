using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item {

    public EquipableItem(string _name)
    {
        iType = ItemType.Equipable;
        name = _name;
        loadDescription();
        loadImage();
    }
}
