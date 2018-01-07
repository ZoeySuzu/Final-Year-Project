using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Interactable {

    private bool free, pickUpFrame;
    private Transform defaultOwner;
    private Rigidbody rb;

    public override void interact()
    {
        if (free)
        {
            rb.isKinematic = true;
            transform.parent = (PlayerController.Instance.getHand().transform);
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            gameUI.setActionButton("Drop");
            toggleIndicator();
            free = false;
            pickUpFrame = true;
        }
    }

    public void Update()
    {
        if (!free && Input.GetButtonDown("A")&& !pickUpFrame)
        {
            rb.isKinematic = false;
            transform.parent = defaultOwner;
            gameUI.setActionButton(interaction);
            toggleIndicator();
            free = true;
        }
        pickUpFrame = false;
    }

    public void Start()
    {
        gameUI = UIController.Instance;
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
