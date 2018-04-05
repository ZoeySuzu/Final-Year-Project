[System.Serializable]

public class Unlocks
{
    public bool[] abilities = new bool[5];
    public Unlocks()
    {
        abilities[0] = true;
        abilities[1] = false;
        abilities[2] = false;
        abilities[3] = false;
        abilities[4] = false;
    }
}

