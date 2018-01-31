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

            var x = Input.GetAxis("LS-X") * Time.deltaTime *  speed;
            var z = Input.GetAxis("LS-Y") * Time.deltaTime * speed;

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

    public override void interact()
    {
        if (!inUse)
        {
            toggleIndicator();
            PlayerController.Instance.setPlayerState("pushing");
            inUse = true;
            startPushing = true;
        }
        else
        {
            if (startPushing)
            {
                startPushing = false;
            }
            else
            {
                gameUI.setActionButton(interaction);
                toggleIndicator();
                PlayerController.Instance.setPlayerState("idle");
                inUse = false;
            }
        }
    }
}
