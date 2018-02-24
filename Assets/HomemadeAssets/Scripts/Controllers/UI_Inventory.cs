using System.Collections;
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

    // Use this for initialization
    void Start() {
        inventory = GameController.Instance.inventory;
        slots = new List<GameObject>();
        description = transform.FindChild("UI_Inventory_Description").GetComponent<Text>();
        setUpSlots();
        listCollectibles();
    }

    private void setUpSlots()
    {
        Transform slotHolder = transform.FindChildByRecursive("UI_Inventory_Slots");

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                GameObject slot = Instantiate(SlotInstance);
                slots.Add(slot);
                slot.transform.SetParent(slotHolder);
                RectTransform rt = slot.GetComponent<RectTransform>();
                rt.localPosition = new Vector3(j * 110 + (j - 1) * 10 - 170, -(i * 110 + (i - 1) * 10 - 170), 0);
            }
        }
    }

    public void listQuestItems()
    {
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
        currentList = ItemType.Collectible;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
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
        slot.GetComponentInChildren<Text>().text = item.name;
        slot.GetComponent<Button>().interactable = true;
        slot.transform.FindChild("Image").gameObject.SetActive(true);
        slot.transform.FindChild("Image").GetComponent<Image>().overrideSprite = item.image;
        slot.GetComponent<Button>().onClick.AddListener(() => { displayDescription(x); });
    }
    private void slotDeactivate(GameObject slot)
    {
        slot.transform.FindChild("Image").gameObject.SetActive(false);
        slot.GetComponentInChildren<Text>().text = "";
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
