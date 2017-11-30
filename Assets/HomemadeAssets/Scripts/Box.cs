using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Box : Interactable {

    private bool inUse = false;
    private bool grounded = false;
    private PlayerController p;
    private float speed;

    private void Start()
    {
        gameUI = GetComponentInParent<UIController>();
        p = GameObject.Find("Object_Player").GetComponent<PlayerController>();
        speed = p.getSpeed();
    }

    public Box()
    {
        interaction = "Grab";
    }

    private bool IsGrounded()
    {
        float scale = transform.localScale.x / 2;
        bool a =Physics.Raycast(transform.position+ Vector3.forward * scale + Vector3.right * scale, Vector3.down, scale + 0.1f);
        bool b =Physics.Raycast(transform.position+ Vector3.forward * scale - Vector3.right * scale, Vector3.down, scale + 0.1f);
        bool c =Physics.Raycast(transform.position- Vector3.forward * scale + Vector3.right * scale, Vector3.down, scale + 0.1f);
        bool d =Physics.Raycast(transform.position- Vector3.forward * scale - Vector3.right * scale, Vector3.down, scale+0.1f);
        if (a || b || c || d)
        {
            if (grounded == false)
            {
                Ray raydown = new Ray(transform.position, Vector3.down);
                RaycastHit hit;
                if (Physics.Raycast(raydown, out hit, 10.00f))
                {
                    transform.position += Vector3.down * hit.distance + Vector3.up*scale;
                }
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

    void Update()
    {
        if(!IsGrounded())
        {
            transform.Translate(Vector3.down*8.0f* Time.deltaTime) ;
        }


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
               Movement(pushDirection * 0.05f);
            }
        }
    }

    private void Movement(Vector3 targetDirection)
    {
        float scale = transform.localScale.x;
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            Ray rayright = new Ray(transform.position, Vector3.right * Mathf.Sign(targetDirection.x));
            if (Physics.Raycast(rayright, out hit, scale / 2 + 0.05f))
            {
                transform.position += Vector3.right * hit.distance * Mathf.Sign(targetDirection.x) - Vector3.right * scale / 2 * Mathf.Sign(targetDirection.x);
            }
            else
            {
                transform.Translate(Vector3.right * targetDirection.x);
            }
        }
        if (targetDirection.z != 0)
        {
            Ray rayforward = new Ray(transform.position, Vector3.forward * Mathf.Sign(targetDirection.z));
            if (Physics.Raycast(rayforward, out hit, scale / 2 + 0.05f))
            {
                if (Mathf.Abs(hit.distance) > 0.05)
                {
                    transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z) - Vector3.forward * scale / 2 * Mathf.Sign(targetDirection.z);
                }
            }
            else
            {
                transform.Translate(Vector3.forward * targetDirection.z);
            }
        }
    }


    public override void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (Input.GetButtonDown("Interact"))
            {
                p.setPlayerState("pushing");
                inUse = true;
            }
            if (Input.GetButtonUp("Interact"))
            {
                p.setPlayerState("idle");
                inUse = false;
            }
        }
    }
    public override void collisionExit()
    {
        if (inUse)
        {
            p.setPlayerState("idle");
            inUse = false;
        }
    }

}
