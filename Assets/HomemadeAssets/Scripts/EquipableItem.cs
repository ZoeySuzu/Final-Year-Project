[System.Serializable]
public class EquipableItem : Item {

    public EquipableItem(string _name)
    {
        iType = ItemType.Equipable;
        name = _name;
        loadDescription();
        loadImage();
    }
}
