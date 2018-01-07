using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; set; }

    //Public Variables for debuging
    public float basespeed;
    public float jumpHeight;
    public GameObject projectile;
    public GameObject trap;
    public int pad;

    //Pointers to other classes
    private Rigidbody rb;
    private UIController ui;
    private GameObject hand;

    //Ability related variables
    private SpellType element;
    private Spell spell;
    private bool spellCasting;
    private Trap[] spellTrap = new Trap[5];

    //State related variables
    private float speed;
    private string playerState;
    private Transform playerModel;
    private bool lockRotation;
    private Vector3 spawn;
    private bool interacting;
    private bool dPadUpdate;

    private int health,mana,maxHealth,maxMana;

    //Combat data
    private ArrayList nearbyEnnemies;
    public GameObject attackCollider;
    private bool canAttack;

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
        nearbyEnnemies = new ArrayList();
        pad = 0;
        spawn = transform.position;
        spellCasting = false;
        lockRotation = false;
        rb = GetComponent<Rigidbody>();
        ui = GetComponentInParent<UIController>();
        playerState = "idle";
        playerModel = transform.Find("Model_Player");
        element = SpellType.Normal;
        hand = transform.GetChild(0).FindChild("holder").gameObject;
        GameController.Instance.addEntity(this.gameObject);
        interacting = false;
        canAttack = true;
        maxMana = 100;
        maxHealth = 100;
        health = maxHealth;
        mana = maxMana;
        ui.updateHealth(health);
        ui.updateMana(mana);
    }



    //------------------------------------------------------Update Code
    void FixedUpdate()
    {
        if (playerState != "Swiming")
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
            }
            else if (rb.velocity.y > 0.1f && !Input.GetButton("A"))
            {
                rb.velocity += Vector3.up * Physics.gravity.y * 2f * Time.deltaTime;
            }
        }
    }
	
	void Update () {
        //check out of bounds
        if(transform.position.y < -20)
        {
            setHealth(-10);
            transform.position = spawn;
        }

        //Check for directional input
        var x = Input.GetAxis("LS-X") * Time.deltaTime * speed;
        var z = Input.GetAxis("LS-Y") * Time.deltaTime * speed;
        Vector3 targetDirection = new Vector3(x, 0f, z);
        targetDirection = Camera.main.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;


        if (playerState.Equals("climbing"))
        {
            rb.velocity = Vector3.zero;
            if (z != 0)
            {
                transform.Translate(Vector3.up * z*0.4f);
            }
        }
        else if (playerState.Equals("Swiming"))
        {
            rb.velocity = Vector3.up*-6f;
            if (Input.GetButton("A"))
            {
                rb.velocity = Vector3.up * 6f;
            }
            Movement(targetDirection);
        }


        //Pushing something
        else if (playerState.Equals("pushing"))
        {
            ui.setActionButton("Let go");
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
            if (targetDirection != Vector3.zero && canAttack == true)
            {
                if (Input.GetButton("B"))
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
                if(ui.getActionButton() == "")
                    ui.setActionButton("Jump");
            }
            else
            {
                playerState = "jumping";
                ui.setActionButton("");
            }

            if(pad == 0) {
                dPadUpdate = true;
                if(Input.GetAxisRaw("D-Y") < 0) { pad = -1; }
                else if (Input.GetAxisRaw("D-Y") > 0) { pad = 1; }
                else if (Input.GetAxisRaw("D-X") < 0) { pad = -2; }
                else if (Input.GetAxisRaw("D-X") > 0) { pad = 2; }
            }
            else if(Input.GetAxisRaw("D-Y") == 0 && Input.GetAxisRaw("D-X") == 0)
            {
                pad = 0;
            }
               

                //Check for spellchange input
            if (Input.GetButtonDown("D-Y") || (pad == -1 && dPadUpdate))
            {
                if (element == SpellType.Electric)
                    element = SpellType.Normal;
                else
                    element = SpellType.Electric;
            }
            else if (Input.GetButtonDown("D-X") || (pad == -2 && dPadUpdate))
            {
                if (element == SpellType.Fire)
                    element = SpellType.Normal;
                else
                    element = SpellType.Fire;
            }
            else if (Input.GetButtonDown("D-Y") || (pad == 1 && dPadUpdate))
            {
                if (element == SpellType.Wind)
                    element = SpellType.Normal;
                else
                    element = SpellType.Wind;
            }
            else if(Input.GetButtonDown("D-X") || (pad == 2 && dPadUpdate))
            {
                if (element == SpellType.Ice)
                    element = SpellType.Normal;
                else
                    element = SpellType.Ice;
            }
            dPadUpdate = false;

            //Check for jump input
            if (Input.GetButtonDown("A") && IsGrounded() && !interacting)
            {
                rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
            }

            //check for attack imput
            if (Input.GetButtonDown("X"))
            {
                if(element == SpellType.Normal && canAttack)
                {
                    canAttack = false;
                    Instantiate(attackCollider, playerModel.transform.position + playerModel.forward * 2f + playerModel.up*1+playerModel.right*1f, playerModel.transform.rotation).transform.parent = transform;
                }
                else if(!IsGrounded() && element == SpellType.Wind)
                {
                    airPulse();
                }
                else if(element == SpellType.Electric)
                {
                    blink();
                }
            }

            //Check for trap input
            if (IsGrounded() && Input.GetButtonDown("R1"))
            {
                int i = (int)element;
                if (spellTrap[i] == null)
                {
                    Debug.Log("Set trap " + i);
                    spellTrap[i] = Instantiate(trap, playerModel.transform.position - playerModel.up*0.95f, playerModel.transform.rotation).GetComponent<Trap>();
                    spellTrap[i].transform.SetParent(transform.parent);
                    spellTrap[i].Initialize(element);
                }
                else
                {
                    Debug.Log("Detonate trap " + i);
                    spellTrap[i].setOff();
                }
            }


            //Check for spell input
            if (Input.GetButtonDown("Y") && IsGrounded())
            {
                    
                spell = Instantiate(projectile, playerModel.transform.position + playerModel.forward*2 + Vector3.up * 0.4f, playerModel.transform.rotation).GetComponent<Spell>();
                if (element == SpellType.Fire || element == SpellType.Wind)
                {
                    spellCasting = true;
                    spell.transform.parent = transform;
                }
                spell.Initialize(element, false);
            }
            if (spellCasting && (!Input.GetButton("Y") || !IsGrounded()))
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
        interacting = false;
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

    public int getMaxHP()
    {
        return maxHealth;
    }

    public int getMaxMana()
    {
        return maxMana;
    }

    public int getHP()
    {
        return health;
    }

    public void setHealth(int difference)
    {
        health += difference;
        ui.updateHealth(health);
    }

    public Transform getModel()
    {
        return playerModel;
    }

    public GameObject getNearestEnnemy()
    {
        float distance;
        float maxDistance = 100;
        GameObject nearest = null;
        if (nearbyEnnemies.Count == 0)
            return null;
        else if (nearbyEnnemies.Count == 1)
            return (GameObject)nearbyEnnemies[0];
        else
        {
            foreach (GameObject ennemy in nearbyEnnemies)
            {
                distance = Vector3.Magnitude(ennemy.transform.position - transform.position);
                if (distance < maxDistance)
                {
                    nearest = ennemy;
                    maxDistance = distance;
                }
            }
            return nearest;
        }
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

    public void setSpawn(Vector3 spawnPoint)
    {
        spawn = spawnPoint;
    }

    public void switchInteracting()
    {
        interacting = true;
    }

    public void refreshAttack()
    {
        canAttack = true;
    }

    //------------------------------------------------------State Check methods

    private bool IsGrounded(){
        RaycastHit hit;
        return Physics.SphereCast(transform.position,0.3f,Vector3.down, out hit,1f);
     }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnnemyTrigger")
        {
            nearbyEnnemies.Add(other.transform.parent.gameObject);
        }
        else if(other.gameObject.tag == "Water")
        {
            playerState = "Swiming";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "EnnemyTrigger")
        {
            if(nearbyEnnemies.Contains(other.transform.parent.gameObject))
                nearbyEnnemies.Remove(other.transform.parent.gameObject);
        }
        else if (other.gameObject.tag == "Water")
        {
            playerState = "idle";
        }
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
