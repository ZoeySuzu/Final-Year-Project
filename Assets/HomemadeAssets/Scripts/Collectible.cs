[System.Serializable]
public class Collectible : Item {

    public int amount { get; protected set; }

    public Collectible(string _name, int _amount)
    {
        iType = ItemType.Collectible;
        name = _name;
        amount = _amount ;
        loadDescription();
        loadImage();
    }

    public void changeAmount(int value)
    {
        amount += value;
        if(amount > 99)
        {
            amount = 99;
        }
        if (amount < 0)
        {
            amount = 0;
        }
    }
}
