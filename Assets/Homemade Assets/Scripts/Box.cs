using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable {

    public GameObject player;
    private bool inUse;
    private PlayerController p;
    private float speed;

    void Start()
    {
        p = player.GetComponent<PlayerController>();
        speed = p.getSpeed();
        gameUI = GetComponentInParent<UIController>();
    }

    public Box()
    {
        interaction = "Grab";
        this.inUse = false;
    }

    void Update()
    {

        if (inUse)
        {
            Vector3 pushDirection;

            var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 targetDirection = new Vector3(x, 0f, z);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z))
            {
                pushDirection = Vector3.right * Mathf.Sign(targetDirection.x);
            }
            else
            {
                pushDirection = Vector3.forward * Mathf.Sign(targetDirection.z);
            }
            if (targetDirection != Vector3.zero)
            {
                transform.Translate(pushDirection * 0.05f);
            }
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact"))
        {
            p.setPlayerState("pushing");
            this.inUse = true;
        }
        if (Input.GetButtonUp("Interact"))
        {
            p.setPlayerState("idle");
            this.inUse = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameUI.setInteractButton("");
        p.setPlayerState("idle");
        this.inUse = false;
    }

}
