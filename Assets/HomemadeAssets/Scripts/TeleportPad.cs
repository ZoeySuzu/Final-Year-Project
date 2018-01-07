using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : Interactable {

    [SerializeField]
    private bool activeAtStart;

    private bool active;

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


    public TeleportPad()
    {
        interaction = "Activate";
        
    }
}
