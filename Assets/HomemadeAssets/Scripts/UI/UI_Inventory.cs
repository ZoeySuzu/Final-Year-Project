using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UI_Inventory : MonoBehaviour {
    public static UI_Inventory Instance { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField]
    private GameObject SlotInstance;
    private Text description;
    Inventory inventory;
    private ItemType currentList;

    List<GameObject> slots;

    private ColorBlock col1, col2;

    // Use this for initialization
    void Start() {
        inventory = GameController.Instance.inventory;
        slots = new List<GameObject>();
        description = transform.FindChild("UI_Inventory_Description").GetComponent<Text>();
        defaultColors();
        setUpSlots();
        listCollectibles();
    }

    public void open()
    {
        listCollectibles();
    }

    private void defaultColors()
    {
        col1 = ColorBlock.defaultColorBlock;
        col1.highlightedColor = new Color(114f / 255f, 198f / 255f, 255f / 255f);

        col2 = ColorBlock.defaultColorBlock;
        col2.normalColor =  new Color(93f/255f,138f/255f,232f/255f);
        col2.highlightedColor = new Color(114f/255f, 198f/255f, 255f/255f); ;
    }

    private void setUpSlots()
    {
        Transform moneyDisplay = transform.FindChild("UI_MoneyDisplay");
        moneyDisplay.FindChild("Amount").GetComponent<Text>().text = "" + inventory.money.amount+"$";
        Sprite img = inventory.money.loadImage();
        if (img != null)
        {
            moneyDisplay.transform.FindChild("Image").GetComponent<Image>().overrideSprite = img;
            moneyDisplay.transform.FindChild("Image").gameObject.SetActive(true);
        }
        else
        {
            moneyDisplay.transform.FindChild("Image").gameObject.SetActive(false);
        }

        Transform slotHolder = transform.FindChildByRecursive("UI_Inventory_Slots");

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject slot = Instantiate(SlotInstance);
                slots.Add(slot);
                slot.transform.SetParent(slotHolder);
                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(j * 80 - 120, -(i*80 - 120), 0);
            }
        }
    }

    public void listQuestItems()
    {
        transform.FindChildByRecursive("UI_Inventory_CI").GetComponent<Button>().colors = col1;
        transform.FindChildByRecursive("UI_Inventory_EI").GetComponent<Button>().colors = col1;
        transform.FindChildByRecursive("UI_Inventory_QI").GetComponent<Button>().colors = col2;


        currentList = ItemType.Quest;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int x = i * 4 + j;
                GameObject slot = slots[x];
                if (inventory.questItems.Count > x)
                {
                    QuestItem item = inventory.questItems[x];
                    if (item != null)
                    {
                        slotActivate(slot, item, x);
                    }
                }
                else
                {
                    slotDeactivate(slot);
                }
            }
        }
    }

    public void listEquipment()
    {
        transform.FindChildByRecursive("UI_Inventory_CI").GetComponent<Button>().colors = col1;
        transform.FindChildByRecursive("UI_Inventory_EI").GetComponent<Button>().colors = col2;
        transform.FindChildByRecursive("UI_Inventory_QI").GetComponent<Button>().colors = col1;
        currentList = ItemType.Equipable;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                int x = i * 4 + j;
                GameObject slot = slots[x];
                if (inventory.equipableItems.Count > x)
                {
                    EquipableItem item = inventory.equipableItems[x];
                    if (item != null)
                    {
                        slotActivate(slot, item, x);
                    }
                }
                else
                {
                    slotDeactivate(slot);
                }
            }
        }
    }


    public void listCollectibles()
    {
        transform.FindChildByRecursive("UI_Inventory_CI").GetComponent<Button>().colors = col2;
        transform.FindChildByRecursive("UI_Inventory_EI").GetComponent<Button>().colors = col1;
        transform.FindChildByRecursive("UI_Inventory_QI").GetComponent<Button>().colors = col1;

        currentList = ItemType.Collectible;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4;  j++)
            {
                int x = i * 4 + j;
                GameObject slot = slots[x];
                if (inventory.collectibleItems.Count > x)
                {
                    Collectible item = inventory.collectibleItems[x];
                    if (item != null)
                    {
                        slotActivate(slot, item, x);
                    }
                }
                else
                {
                    slotDeactivate(slot);
                }
            }
        } 
    }

    private void slotActivate(GameObject slot, Item item, int x)
    {
        slot.transform.FindChild("Name").GetComponent<Text>().text = item.name;
        if (currentList == ItemType.Collectible)
        {
            Collectible citem = (Collectible)item;
            slot.transform.FindChild("Amount").GetComponent<Text>().text = ""+citem.amount;
        }
        else
        {
            slot.transform.FindChild("Amount").GetComponent<Text>().text = "";
        }

        slot.GetComponent<Button>().interactable = true;
        Sprite img = item.loadImage();
        if (img != null) {
            slot.transform.FindChild("Image").GetComponent<Image>().overrideSprite = img;
            slot.transform.FindChild("Image").gameObject.SetActive(true);
        }
        else
        {
            slot.transform.FindChild("Image").gameObject.SetActive(false);
        }
        slot.GetComponent<Button>().onClick.AddListener(() => { displayDescription(x); });
    }
    private void slotDeactivate(GameObject slot)
    {
        slot.transform.FindChild("Image").gameObject.SetActive(false);
        slot.transform.FindChild("Name").GetComponent<Text>().text = "";
        slot.transform.FindChild("Amount").GetComponent<Text>().text = "";
        slot.GetComponent<Button>().interactable = false;
    }

    void displayDescription(int x)
    {
        if(currentList == ItemType.Collectible)
            description.text = inventory.collectibleItems[x].description;
        else if (currentList == ItemType.Equipable)
            description.text = inventory.equipableItems[x].description;
        else if (currentList == ItemType.Quest)
            description.text = inventory.questItems[x].description;

    }
}
