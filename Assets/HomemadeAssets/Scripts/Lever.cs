using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable {


    private bool inUse, startPushing;
    private float speed = 3;

    public void Update()
    {
        if (inUse)
        {
            Vector3 pushDirection;

            var x = Input.GetAxis("Horizontal") * Time.deltaTime *  speed;
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
            if (pushDirection == Vector3.right && transform.position.x < transform.parent.transform.position.x + 1.0f)
            {
                transform.Translate(Vector3.right * speed* Time.deltaTime);
            }
            else if(pushDirection == Vector3.left && transform.position.x > transform.parent.transform.position.x - 1.0f)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }


    public Lever()
    {
        interaction = "Grab";
    }


    public override void collisionExit()
    {
        if (inUse)
        {
            PlayerController.Instance.setPlayerState("idle");
            inUse = false;
        }
    }

    public override void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (Input.GetButtonDown("Interact") && !inUse)
            {
                toggleIndicator();
                PlayerController.Instance.setPlayerState("pushing");
                inUse = true;
                startPushing = true;
            }
            if (Input.GetButtonUp("Interact"))
            {
                if (startPushing)
                {
                    startPushing = false;
                }
                else
                {
                    toggleIndicator();
                    PlayerController.Instance.setPlayerState("idle");
                    inUse = false;
                }
            }
        }
    }
}
