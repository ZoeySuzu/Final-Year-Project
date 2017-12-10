﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; set; }

    //Public Variables for debuging
    public float basespeed;
    public float jumpHeight;
    public GameObject projectile;
    public bool fighting;

    //Pointers to other classes
    private Rigidbody rb;
    private UIController ui;

    private GameObject hand;

    //Ability related variables
    private SpellType element;
    private Spell spell;
    private bool spellCasting;

    //State related variables
    private float speed;
    private string playerState;
    private Transform playerModel;
    private bool lockRotation;

    //------------------------------------------------------Initialising Code
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        speed = basespeed;
    }
    void Start () {
        spellCasting = false;
        lockRotation = false;
        rb = GetComponent<Rigidbody>();
        ui = GetComponentInParent<UIController>();
        playerState = "idle";
        playerModel = transform.Find("Model_Player");
        element = SpellType.Normal;
        hand = transform.GetChild(0).FindChild("holder").gameObject;
    }



    //------------------------------------------------------Update Code
    void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
        }
        else if (rb.velocity.y > 0.1f && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
        }
    }
	
	void Update () {
        //check out of bounds
        if(transform.position.y < -20)
        {
            transform.position = new Vector3(0, 0, 0);
        }

        //Check for directional input
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
        Vector3 targetDirection = new Vector3(x, 0f, z);
        targetDirection = Camera.main.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;


        //Pushing something
        if (playerState.Equals("pushing"))
        {
            ui.setInteractButton("Let go");
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
            playerState = "idle";

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
                if (!lockRotation)
                {
                    playerModel.forward = targetDirection;
                }
                Movement(targetDirection);

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
            if (Input.GetButtonDown("D-D"))
            {
                if (element == SpellType.Electric)
                    element = SpellType.Normal;
                else
                    element = SpellType.Electric;
            }
            else if (Input.GetButtonDown("D-L"))
            {
                if (element == SpellType.Fire)
                    element = SpellType.Normal;
                else
                    element = SpellType.Fire;
            }
            else if (Input.GetButtonDown("D-U"))
            {
                if (element == SpellType.Wind)
                    element = SpellType.Normal;
                else
                    element = SpellType.Wind;
            }
            else if(Input.GetButtonDown("D-R"))
            {
                if (element == SpellType.Ice)
                    element = SpellType.Normal;
                else
                    element = SpellType.Ice;
            }

            //Check for jump input
            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
            }

            //check for attack imput
            if (Input.GetButtonDown("Attack"))
            {
                if(!IsGrounded() && element == SpellType.Wind)
                {
                    airPulse();
                }
                else if(element == SpellType.Electric)
                {
                    blink();
                }
            }


            //Check for spell input
            if (Input.GetButtonDown("Cast") && IsGrounded())
            {
                    
                spell = Instantiate(projectile, playerModel.transform.position + playerModel.forward*2 + Vector3.up * 0.4f, playerModel.transform.rotation).GetComponent<Spell>();
                if (element == SpellType.Fire || element == SpellType.Wind)
                {
                    spellCasting = true;
                    spell.transform.parent = transform;
                }
                spell.Initialize(element, false);
            }
            if (spellCasting && (!Input.GetButton("Cast") || !IsGrounded()))
            {
                spell.stopCast();
                spellCasting = false;
            }
        }

        if (spellCasting)
        {
            playerState = "casting";
        }

        ui.setPlayerState(playerState);
        ui.setSpellState(element.ToString());
    }

    //------------------------------------------------------Attack Methods
 

    private void blink()
    {
        transform.position = transform.position += playerModel.transform.forward * 8;
    }

    private void airPulse()
    {
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(velocity.x, 15, velocity.z);
        rb.AddForce(playerModel.transform.forward*-100);
    }

    private void attack()
    {

    }


    //------------------------------------------------------Get Methods
    public float getSpeed()
    {
        return speed;
    }

    public string getPlayerState()
    {
        return playerState;
    }

    public GameObject getHand()
    {
        return hand;
    }


    //------------------------------------------------------Set Methods
    public void setJumpHeight(float height)
    {
        jumpHeight = height;
    }

    public void setPlayerState(string state)
    {
        playerState = state;
        Debug.Log("Set player interaction:" + state);
    }

    //------------------------------------------------------State Check methods

    private bool IsGrounded(){
        RaycastHit hit;
        return Physics.SphereCast(transform.position,0.3f,Vector3.down, out hit,1f);
     }

    //------------------------------------------------------Movement colision
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
