﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float basespeed;
    public float jumpHeight;
    Rigidbody rb;
    UIController ui;
    public GameObject projectile;

    private SpellType element;
    private float speed;
    private string playerState;
    private Transform playerModel;
    public bool fighting;

    private bool active;

    // Use this for initialization
    private void Awake()
    {
        speed = basespeed;
    }
    void Start () {
        rb = GetComponent<Rigidbody>();
        ui = GetComponentInParent<UIController>();
        playerState = "idle";
        playerModel = transform.Find("Model_Player");
        element = SpellType.Normal;
        active = true;
    }
	
	void Update () {
        if (active)
        {
            //Check for directional input
            var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 targetDirection = new Vector3(x, 0f, z);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            //Pushing something
            if (playerState.Equals("pushing"))
            {
                Vector3 pushDirection;
                //Get most emphasized direction
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

            //Not pushing something
            else
            {

                //Check movement state
                if (targetDirection != Vector3.zero)
                {
                    if (Input.GetButton("Run"))
                    {
                        playerState = "running";
                        speed = basespeed + basespeed / 2;
                    }
                    else
                    {
                        playerState = "walking";
                        speed = basespeed;
                    }
                    playerModel.forward = targetDirection;
                    Movement(targetDirection);

                }
                else
                {
                    playerState = "idle";
                }

                //Check if on ground
                if (IsGrounded())
                {
                    ui.setActionButton("Jump");
                }
                else
                {
                    playerState = "jumping";
                    ui.setActionButton("");
                }
                //Check for spellchange input
                if (Input.GetAxis("D-Y") > 0)
                {
                    element = SpellType.Electric;
                }
                if (Input.GetAxis("D-X") < 0)
                {
                    element = SpellType.Fire;
                }
                if (Input.GetAxis("D-Y") < 0)
                {
                    element = SpellType.Wind;
                }
                if (Input.GetAxis("D-X") > 0)
                {
                    element = SpellType.Ice;
                }

                //Check for jump input
                if (Input.GetButtonDown("Jump") && IsGrounded())
                {
                    rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
                }

                //Check for spell input
                if (Input.GetButtonDown("SmallSpell") && IsGrounded())
                {

                    Spell spell = Instantiate(projectile, playerModel.transform.position + playerModel.forward * 2 + Vector3.up * 0.4f, playerModel.transform.rotation).GetComponent<Spell>();
                    spell.Initialize(element, false);
                }
            }

            ui.setPlayerState(playerState);
            ui.setSpellState(element.ToString());
        }
    }

    public float getSpeed()
    {
        return speed;
    }

    public string getPlayerState()
    {
        return playerState;
    }

    public void setPlayerState(string state)
    {
        playerState = state;
        Debug.Log("Set player interaction:" + state);
    }

    private bool IsGrounded(){
        RaycastHit hit;
        return Physics.SphereCast(transform.position,0.3f,Vector3.down, out hit,1f);
     }

    public void playerActive()
    {
        active = true;
    }

    public void playerInactive()
    {
        active = false;
    }

    //Movement colision

    private void Movement(Vector3 targetDirection)
    {
        float scale = transform.localScale.x;
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            Ray rayright = new Ray(transform.position, Vector3.right * Mathf.Sign(targetDirection.x));
            if (Physics.Raycast(rayright, out hit, scale/2+0.05f))
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
                if (Mathf.Abs(hit.distance) > 0.05 )
                {
                    transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z) - Vector3.forward * scale/2 * Mathf.Sign(targetDirection.z);
                }
            }
            else
            {
                transform.Translate(Vector3.forward * targetDirection.z);
            }
        }
    }

}
