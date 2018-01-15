using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018
//todo: Add functionality to active teleporters 

public class TeleportPad : Interactable {

    [SerializeField]
    private bool activeAtStart;
    private bool active;

    //Activate an inactive teleporter
    public override void interact()
    {
        if (!active)
        {
            active = true;
            GameController.Instance.addTeleportPad(this);
            interaction = "Warp";
            gameUI.setActionButton(interaction);
            transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    //Mark teleporters as active or inactive
    public void Start()
    {
        gameUI = UIController.Instance;
        transform.GetChild(1).gameObject.SetActive(false);
        active = activeAtStart;
        if (activeAtStart)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            GameController.Instance.addTeleportPad(null);
            interaction = "Warp";
        }
    }

    //UI interaction reference
    public TeleportPad()
    {
        interaction = "Activate";
    }
}
