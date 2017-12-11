using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable {

    private bool free, pickUpFrame;
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
                pickUpFrame = true;
            }
        }
    }

    public void Update()
    {
        if (!free && Input.GetButtonDown("Interact")&& !pickUpFrame)
        {
            rb.isKinematic = false;
            transform.parent = defaultOwner;
            gameUI.setInteractButton(interaction);
            toggleIndicator();
            free = true;
        }
        pickUpFrame = false;
    }

    public void Start()
    {
        pickUpFrame = false;
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(rb.GetComponent<Collider>(), PlayerController.Instance.GetComponent<Collider>(), true);
        defaultOwner = transform.parent.transform;
    }

    public Weapon()
    {
        interaction = "Pick up";
        free = true;
    }




}
