using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Box : Interactable {
    private bool startPushing = false;
    private bool inUse = false;
    private bool grounded = false;
    private Vector3 direction;
    private Vector3 pOffset;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameUI = UIController.Instance;
    }

    public Box()
    {
        interaction = "Grab";
    }

    private bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, transform.localScale, Vector3.down, out hit, transform.rotation, 0.1f, -1, QueryTriggerInteraction.Ignore))
        {
            if (grounded == false)
            {
                transform.position += Vector3.down * hit.distance;
                grounded = true;
            }
            return true;
        }
        else
        {
            grounded = false;
            return false;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        if (inUse)
        {
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
            var z = Input.GetAxis("LS-Y");
            if (z > 0.3)
            {
                Push(Vector3.Normalize((PlayerController.Instance.transform.position - transform.position) * -1f));
            }
            else if (z < -0.3)
            {
                Pull(Vector3.Normalize((PlayerController.Instance.transform.position - transform.position)));
            }
        }
    }
    private void Push(Vector3 dir)
    {
        if (!Physics.BoxCast(transform.position, transform.localScale/2, dir, transform.rotation, 1f, -1, QueryTriggerInteraction.Ignore))
        {
            direction = dir;
            transform.position += direction * 4 * Time.deltaTime;
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
        }
    }

    private void Pull(Vector3 dir)
    {
        LayerMask lm = 1 << 8;
        if (!Physics.BoxCast(transform.position, transform.localScale/2, dir, transform.rotation, 1f + PlayerController.Instance.transform.localScale.z,~lm, QueryTriggerInteraction.Ignore))
        {
            direction = dir;
            transform.position += direction * 4 * Time.deltaTime;
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
        }
    }



    public override void interact()
    {
        if (!inUse)
        {
            PlayerController.Instance.anim.SetTrigger("Push");
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Pushing");
            pOffset = transform.rotation*(PlayerController.Instance.transform.position - transform.position);
            playerRepos(pOffset);
            Vector3 orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
            PlayerController.Instance.transform.rotation = transform.rotation;
            PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
            gameUI.setActionButton(interaction);
            inUse = true;
            FollowCamera.Instance.setFollow();
        }
        else
        {
            PlayerController.Instance.anim.SetTrigger("StopPush");
            gameUI.setInteractButton(interaction);
            toggleIndicator();
            PlayerController.Instance.transform.forward = Vector3.forward;
            PlayerController.Instance.setPlayerState("Idle");
            inUse = false;
        }
    }

    private void playerRepos(Vector3 offset)
    {
        var scale = transform.localScale.x;
        if(Mathf.Abs(offset.x) > Mathf.Abs(offset.z)){
            pOffset = Quaternion.Inverse(transform.rotation) * new Vector3((scale+0.75f) * Mathf.Sign(offset.x), 0, 0);
        }
        else
        {
            pOffset = Quaternion.Inverse(transform.rotation)*new Vector3(0, 0, (scale + 0.75f) * Mathf.Sign(offset.z));
        }
        PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
    }

    public override void collisionExit()
    {
        if (inUse)
        {
            gameUI.setInteractButton(interaction);
            PlayerController.Instance.anim.SetTrigger("StopPush");
            PlayerController.Instance.transform.forward = Vector3.forward;
            PlayerController.Instance.setPlayerState("Idle");
            inUse = false;
        }
    }

}
