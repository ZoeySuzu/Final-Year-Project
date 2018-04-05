
[System.Serializable]
public class QuestItem : Item {

    public QuestItem(string _name)
    {
        iType = ItemType.Quest;
        name = _name;
        loadDescription();
        loadImage();
    }
}
