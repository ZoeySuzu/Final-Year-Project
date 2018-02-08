using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//Todo Implement climbing:


public class Lader : Interactable {

    public override void interact()
    {
        if (PlayerController.Instance.getPlayerState() != "Climbing")
        {
            PlayerController.Instance.transform.position = transform.position + transform.right * -0.5f;
            PlayerController.Instance.transform.GetChild(0).rotation = Quaternion.LookRotation(transform.right);
            PlayerController.Instance.setPlayerState("Climbing");
        }
        else
        {
            PlayerController.Instance.setPlayerState("");
        }
    }

    public Lader()
    {
        interaction = "Climb";
    }
}
