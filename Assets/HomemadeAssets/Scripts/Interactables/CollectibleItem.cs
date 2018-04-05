using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleItem : Interactable{

    [SerializeField]
    private string itemName;
    [SerializeField]
    private int itemQuantity = 1;

    private Collectible citem;

    public void Start()
    {
        gameUI = UIController.Instance;
        citem = new Collectible(itemName,itemQuantity);
    }

    public override void interact()
    {
        GameController.Instance.inventory.addItem(citem);
        gameUI.setInteractButton("Attack");
        Destroy(gameObject);
    }

    public CollectibleItem() {
        interaction = "Pick up";
    }
}
