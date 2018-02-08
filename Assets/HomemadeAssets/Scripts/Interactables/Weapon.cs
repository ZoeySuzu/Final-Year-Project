using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018

public class Weapon : Interactable {

    private bool free;
    private Transform defaultOwner;
    private Rigidbody rb;


    //Pickup weapon.
    public override void interact()
    {
        if (free)
        {
            rb.isKinematic = true;
            transform.parent = (PlayerController.Instance.getHand().transform);
            transform.position = transform.parent.position;
            transform.rotation = transform.parent.rotation;
            toggleIndicator();
            free = false;
        }
    }

    //Check for input to drop weapon
    public void Update()
    {
        if (!free && Input.GetButtonDown("B"))
        {
            rb.isKinematic = false;
            transform.parent = defaultOwner;
            toggleIndicator();
            free = true;
        }
    }

    public void Start()
    {
        gameUI = UIController.Instance;
        rb = GetComponent<Rigidbody>();
        Physics.IgnoreCollision(rb.GetComponent<Collider>(), PlayerController.Instance.GetComponent<Collider>(), true);
        defaultOwner = transform.parent.transform;
        free = true;
    }

    //UI interaction reference
    public Weapon()
    {
        interaction = "Pick up";
    }




}
