using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 31/01/2018
public class TeleportPad : Interactable {

    [SerializeField]
    private string locationName;

    [SerializeField]
    private bool activeAtStart;
    private bool active;

    private bool inUse;

    //Activate an inactive teleporter
    public override void interact()
    {
        if (!active)
        {
            active = true;
            GameController.Instance.addTeleportPad(this);
            interaction = "Warp";
            gameUI.setInteractButton(interaction);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (!inUse)
        {
            TeleportController.Instance.gameObject.SetActive(true);
            TeleportController.Instance.openController(this);
            inUse = true;
        }
    }

    public string getName()
    {
        return locationName;
    }

    //Mark teleporters as active or inactive
    public void Start()
    {
        inUse = false;
        gameUI = UIController.Instance;
        transform.GetChild(1).gameObject.SetActive(false);
        active = activeAtStart;
        if (activeAtStart)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            GameController.Instance.addTeleportPad(this);
            interaction = "Warp";
        }
    }

    public void closePad()
    {
        inUse = false;
    }

    //UI interaction reference
    public TeleportPad()
    {
        interaction = "Activate";
    }
}
