using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory{

    public Collectible money { get; private set; }
    public List<Collectible> collectibleItems { get; private set; }
    public List<QuestItem> questItems { get; private set; }
    public List<EquipableItem> equipableItems { get; private set; }

    public Inventory()
    {
        money = new Collectible("money", 100);
        collectibleItems = new List<Collectible>();
        questItems = new List<QuestItem>();
        equipableItems = new List<EquipableItem>();
    }

    public void addMoney(int amount)
    {
        money.changeAmount(amount);
    }

    public bool spendMoney(int amount)
    {
        if (money.amount - amount >= 0)
        {
            money.changeAmount(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool exchangeCollectible(string name, int number)
    {
        Collectible item = findCollectibleItem(name);
        if (item != null)
        {
            if(item.amount-number >= 0)
            {
                item.changeAmount(-number);
                return true;
            }
        }
        return false;
    }

    public void addItem(Item item)
    {
        try
        {
            if (item.iType == ItemType.Collectible)
            {
                collectibleItems.Add((Collectible)item);
            }
            else if (item.iType == ItemType.Equipable)
            {
                equipableItems.Add((EquipableItem)item);
            }
            else if (item.iType == ItemType.Quest)
            {
                questItems.Add((QuestItem)item);
            }
        }
        catch (Exception e)
        {
            Debug.Log("Item couldn't be added");
        }
    }


    public void removeItem(Item item)
    {
        try
        {
            if (item.iType == ItemType.Collectible)
            {
                collectibleItems.Remove((Collectible)item);
            }
            else if (item.iType == ItemType.Equipable)
            {
                equipableItems.Remove((EquipableItem)item);
            }
            else if (item.iType == ItemType.Quest)
            {
                questItems.Remove((QuestItem)item);
            }
        }catch(Exception e)
        {
            Debug.Log("Item couldn't be found");
        }
    }

    public Item findItem(string itemname, ItemType iType)
    {
        if(iType == ItemType.Equipable) { return findEquipableItem(itemname); }
        else if (iType == ItemType.Quest) { return findQuestItem(itemname); }
        else if (iType == ItemType.Collectible) { return findCollectibleItem(itemname); }
        return null;
    }

    private QuestItem findQuestItem(string itemname)
    {
        foreach(QuestItem qi in questItems)
        {
            if(qi.name == itemname)
            {
                return qi;
            }
        }
        return null;
    }
    private EquipableItem findEquipableItem(string itemname)
    {
        foreach (EquipableItem ei in equipableItems)
        {
            if (ei.name == itemname)
            {
                return ei;
            }
        }
        return null;
    }

    private Collectible findCollectibleItem(string itemname)
    {
        foreach (Collectible ci in collectibleItems)
        {
            if (ci.name == itemname)
            {
                return ci;
            }
        }
        return null;
    }

}
