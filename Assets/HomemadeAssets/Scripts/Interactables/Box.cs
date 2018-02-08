using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Box : Interactable {
    private bool startPushing = false;
    private bool inUse = false;
    private bool grounded = false;
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

    /*public void boostVSpeed()
    {
        if (IsGrounded())
        {
            transform.position+= Vector3.up*0.1f;
        }
        if (vSpeed <= 8f)
            vSpeed += 2f;
        else
            vSpeed = 8f;
    }*/

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }

    void Update()
    {
        /*
        if (!IsGrounded())
        {
            if (vSpeed < -20f) { }

            else
            {
                vSpeed -= 1f;
            }
        }
        else
        {
            vSpeed = 0;
        }

        transform.position += Vector3.up* vSpeed*Time.deltaTime;
        */
        
        if (inUse)
        {
            Vector3 pushDirection;

            var x = Input.GetAxis("LS-X");
            var z = Input.GetAxis("LS-Y");

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
               Movement(pushDirection * 0.05f);
            }
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
        }
    }
    /*private void Movement(Vector3 targetDirection)
    {
        if(Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z) && Mathf.Abs(pOffset.x) > Mathf.Abs(pOffset.z))
        {
            rb.velocity = targetDirection * 50;
        }
        else if (Mathf.Abs(targetDirection.x) < Mathf.Abs(targetDirection.z) && Mathf.Abs(pOffset.x) < Mathf.Abs(pOffset.z)){
            rb.velocity = targetDirection * 50;
        }

        
    }*/


    
    private void Movement(Vector3 targetDirection)
    {
        RaycastHit hit;
        if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z) && Mathf.Abs(pOffset.x) > Mathf.Abs(pOffset.z))
        {
            if (Mathf.Sign(pOffset.x) == Mathf.Sign(targetDirection.x))
            {
                if (Physics.BoxCast(transform.position, transform.localScale, Vector3.right * Mathf.Sign(targetDirection.x), out hit, transform.rotation, 0.1f+PlayerController.Instance.transform.localScale.z, -1, QueryTriggerInteraction.Ignore))
                {
                    transform.position += Vector3.right * hit.distance * Mathf.Sign(targetDirection.x) +  Vector3.right * -PlayerController.Instance.transform.localScale.z * Mathf.Sign(targetDirection.x);
                }
                else
                {
                    transform.Translate(Vector3.right * targetDirection.x);
                }
            }
            else
            {
                if (Physics.BoxCast(transform.position, transform.localScale, Vector3.right * Mathf.Sign(targetDirection.x), out hit, transform.rotation, 0.1f, -1, QueryTriggerInteraction.Ignore))
                {
                    transform.position += Vector3.right * hit.distance * Mathf.Sign(targetDirection.x);// - Vector3.right * scale * Mathf.Sign(targetDirection.x);
                }
                else
                {
                    transform.Translate(Vector3.right * targetDirection.x);
                }
            }
        }
        else if (Mathf.Abs(targetDirection.x) < Mathf.Abs(targetDirection.z) && Mathf.Abs(pOffset.x) < Mathf.Abs(pOffset.z))
        {
            if (Mathf.Sign(pOffset.z) == Mathf.Sign(targetDirection.z))
            {
                if (Physics.BoxCast(transform.position, transform.localScale, Vector3.forward * Mathf.Sign(targetDirection.z), out hit, transform.rotation, 0.1f + PlayerController.Instance.transform.localScale.z, -1, QueryTriggerInteraction.Ignore))
                {
                    if (Mathf.Abs(hit.distance) > 0.05)
                    {
                        transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z) + Vector3.forward * -PlayerController.Instance.transform.localScale.z * Mathf.Sign(targetDirection.z);
                    }
                }
                else
                {
                    transform.Translate(Vector3.forward * targetDirection.z);
                }
            }
            else
            {
                if (Physics.BoxCast(transform.position, transform.localScale, Vector3.forward * Mathf.Sign(targetDirection.z), out hit, transform.rotation, 0.1f, -1, QueryTriggerInteraction.Ignore))
                {
                    if (Mathf.Abs(hit.distance) > 0.05)
                    {
                        transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z);// - Vector3.forward * scale * Mathf.Sign(targetDirection.z);
                    }
                }
                else
                {
                    transform.Translate(Vector3.forward * targetDirection.z);
                }
            }
        }
    }
    

    public override void interact()
    {
        if (!inUse)
        {
            Vector3 orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
            PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("pushing");
            pOffset = PlayerController.Instance.transform.position - transform.position;
            playerRepos(pOffset);
            inUse = true;
        }
        else
        {
            gameUI.setActionButton(interaction);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("idle");
            inUse = false;
        }
    }

    private void playerRepos(Vector3 offset)
    {
        var scale = transform.localScale.x;
        if(Mathf.Abs(offset.x) > Mathf.Abs(offset.z)){
            pOffset = new Vector3((scale+0.5f) * Mathf.Sign(offset.x), 0, 0);
        }
        else
        {
            pOffset = new Vector3(0, 0, (scale + 0.5f) * Mathf.Sign(offset.z));
        }
        PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
    }

    public override void collisionExit()
    {
        if (inUse)
        {
            PlayerController.Instance.setPlayerState("idle");
            inUse = false;
        }
    }

}
