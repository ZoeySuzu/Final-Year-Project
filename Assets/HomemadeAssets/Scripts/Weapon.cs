using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable {

    private bool free;
    private Transform defaultOwner;
    private Rigidbody rb;

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && other.name == "Object_Player")
        {
            if (free)
            {
                rb.isKinematic = true;
                transform.parent = (PlayerController.Instance.getHand().transform);
                transform.position = transform.parent.position;
                transform.rotation = transform.parent.rotation;
                gameUI.setInteractButton("Drop");
                toggleIndicator();
                free = false;
            }
            else
            {
                rb.isKinematic = false;
                transform.parent = defaultOwner;
                gameUI.setInteractButton(interaction);
                toggleIndicator();
                free = true;
            }
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        defaultOwner = transform.parent.transform;
    }

    public Weapon()
    {
        interaction = "Pick up";
        free = true;
    }

}
