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
    private Cooldown move;
    private Vector3 pOffset;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameUI = UIController.Instance;
        move = new Cooldown(0.25f);
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
    }

    void Update()
    {
        if (inUse)
        {
            if (move.ready)
            {
                transform.position = new Vector3(Mathf.Round(transform.position.x), transform.position.y, Mathf.Round(transform.position.z));
                PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
                var z = Input.GetAxis("LS-Y");
                if (z > 0.3)
                {
                    Push(PlayerController.Instance.transform.GetChild(0).forward);
                }
                else if (z < -0.3)
                {
                    Pull(PlayerController.Instance.transform.GetChild(0).forward*-1);
                }
            }
            else
            {
                transform.Translate(direction * 4 * Time.deltaTime);
                PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
            }

            
        }
    }

    private void Push(Vector3 dir)
    {
        if (!Physics.BoxCast(transform.position, transform.localScale/2, dir, transform.rotation, 1f, -1, QueryTriggerInteraction.Ignore))
        {
            direction = dir;
            StartCoroutine(move.StartCooldown());
        }
    }

    private void Pull(Vector3 dir)
    {
        LayerMask lm = 1 << 8;
        if (!Physics.BoxCast(transform.position, transform.localScale/2, dir, transform.rotation, 1f + PlayerController.Instance.transform.localScale.z,~lm, QueryTriggerInteraction.Ignore))
        {
            direction = dir;
            StartCoroutine(move.StartCooldown());
        }
    }



    public override void interact()
    {
        if (!inUse)
        {
            
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Pushing");
            pOffset = PlayerController.Instance.transform.position - transform.position;
            playerRepos(pOffset);
            Vector3 orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
            PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
            
            inUse = true;
            FollowCamera.Instance.setFollow();
        }
        else if(move.ready)
        {
            gameUI.setActionButton(interaction);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Idle");
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
            PlayerController.Instance.setPlayerState("Idle");
            inUse = false;
        }
    }

}
