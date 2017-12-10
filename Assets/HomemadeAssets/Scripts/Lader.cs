using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//Todo Implement climbing:


public class Lader : Interactable {
    
    public override void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (Input.GetButtonDown("Interact")){
                if (PlayerController.Instance.getPlayerState() != "climbing")
                {
                    PlayerController.Instance.transform.position = transform.position + transform.right * -0.5f;
                    PlayerController.Instance.getModel().rotation = Quaternion.LookRotation(transform.right);
                    PlayerController.Instance.setPlayerState("climbing");
                }
                else
                {
                    PlayerController.Instance.setPlayerState("");
                }
            }
        }
    }

    public Lader()
    {
        interaction = "Climb";
    }
}
