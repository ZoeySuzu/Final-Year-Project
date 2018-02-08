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
    private Vector3 scale;

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
    private bool crouching;
    private int health,mana,maxHealth,maxMana;

    //Combat data
    private ArrayList nearbyEnnemies;
    public GameObject attackCollider;
    private bool canAttack, canStrafe;
    private float spellCooldownStart;

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
        canStrafe = true;
        maxMana = 100;
        maxHealth = 100;
        health = maxHealth;
        mana = maxMana;
        ui.updateHealth(health);
        ui.updateMana(mana);
        scale = GetComponent<BoxCollider>().size;
        scale.y = scale.y * 0.9f;
        Debug.Log(scale);
    }



    //------------------------------------------------------Update Code
    void FixedUpdate()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        /*var mag = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
        if ( mag > speed)
        {
            rb.velocity = new Vector3(rb.velocity.x / mag * speed, rb.velocity.y, rb.velocity.z / mag * speed);
        }*/

        if(rb.velocity.y<0 && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }
	
	void Update () {


        //check out of bounds
        if(transform.position.y < -20)
        {
            setHealth(-10);
            transform.position = spawn;
        }

        //Climbing
        if (playerState.Equals("climbing"))
        {
            float z = Input.GetAxis("LS-Y");
            rb.velocity = Vector3.zero;
            if (z != 0)
            {
                transform.Translate(Vector3.up * z*0.4f);
            }
        }

        //Pushing
        else if (playerState.Equals("pushing"))
        {
            ui.setInteractButton("Let go");
        }

        //Not Doing other action
        else
        {
            //Check for directional input
            var x = Input.GetAxis("LS-X");
            var z = Input.GetAxis("LS-Y");
            Vector3 targetDirection = new Vector3(x, 0f, z);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            lockRotation = false;

            //Check for crouch input
            if (IsGrounded() && Input.GetButton("L1"))
            {
                playerState = "crouching";
            }
            else if (IsGrounded())
            {
                ui.setActionButton("Jump");
                playerState = "idle";
            }
            else
            {
                playerState = "falling";
                ui.setActionButton("");
            }

            GameObject focus = FollowCamera.Instance.getFocus();
            if (focus != null)
            {
                lockRotation = true;
                Vector3 orientation = new Vector3(focus.transform.position.x, playerModel.position.y, focus.transform.position.z);
                playerModel.LookAt(orientation);
            }

            if (!FollowCamera.Instance.getLerp() && Input.GetAxis("L2") > 0.5f)
            {
                speed = basespeed * 0.30f;
                transform.GetChild(0).forward = new Vector3(FollowCamera.Instance.transform.forward.x, 0, FollowCamera.Instance.transform.forward.z);
            }

            //Check movement state
            if (targetDirection != Vector3.zero && canStrafe)
            {
                if(canAttack == false)
                {
                    playerState = "attacking";
                    speed = basespeed * 0.30f;
                }
                else if(Input.GetAxis("L2") > 0.5f)
                {
                    playerState = "aiming";
                    speed = basespeed * 0.30f;
                }
                else
                {
                    playerState = "walking";
                    speed = basespeed;
                }


                if (element == SpellType.Electric)
                {
                    speed = speed * 1.25f;
                }
                
                if (!lockRotation && !FollowCamera.Instance.getLerp() && focus == null && Input.GetAxis("L2") <= 0.5f)
                {
                    playerModel.forward = targetDirection;
                }
                Movement(targetDirection);
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
            if (Input.GetButtonDown("D-U") || (pad == -1 && dPadUpdate))
            {
                if (element == SpellType.Electric)
                    element = SpellType.Normal;
                else {
                    element = SpellType.Electric;
                    if (spell)
                    {
                        spell.setMorph(SpellType.Electric);
                    }
                }
            }
            else if (Input.GetButtonDown("D-L") || (pad == -2 && dPadUpdate))
            {
                if (element == SpellType.Fire)
                    element = SpellType.Normal;
                else
                {
                    element = SpellType.Fire;
                    if (spell)
                    {
                        spell.setMorph(SpellType.Fire);
                    }
                }
            }
            else if (Input.GetButtonDown("D-D") || (pad == 1 && dPadUpdate))
            {
                if (element == SpellType.Wind)
                    element = SpellType.Normal;
                else
                {
                    element = SpellType.Wind;
                    if (spell)
                    {
                        spell.setMorph(SpellType.Wind);
                    }
                }
            }
            else if(Input.GetButtonDown("D-R") || (pad == 2 && dPadUpdate))
            {
                if (element == SpellType.Ice)
                    element = SpellType.Normal;
                else
                {
                    element = SpellType.Ice;
                    if (spell)
                    {
                        spell.setMorph(SpellType.Ice);
                    }
                }
            }
            dPadUpdate = false;

            //Check for jump input
            if (Input.GetButtonDown("A") && IsGrounded() && !interacting)
            {
                rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
                playerState = "jumping";
                ui.setActionButton("");
            }

            if ((Input.GetButtonDown("B") || (Input.GetButtonDown("LS"))) && canStrafe)
            {
                canStrafe = false;
                StartCoroutine(Strafe(targetDirection));
            }

            //check for attack imput
            if (Input.GetButtonDown("X") && !interacting)
            {
                if(element == SpellType.Normal && canAttack)
                {
                    attack();
                }
                else if(!IsGrounded() && element == SpellType.Wind)
                {
                    airPulse();
                }
                else if(element == SpellType.Electric && canAttack)
                {
                    blink();
                    attack();
                }
            }

            if (IsGrounded() && Input.GetButtonDown("Y"))
            {
                int i = (int)element;
                if (spellTrap[i] == null)
                {
                    Debug.Log("Set trap " + i);
                    spellTrap[i] = Instantiate(trap, transform.position - new Vector3(0, transform.localScale.y, 0), playerModel.transform.rotation).GetComponent<Trap>();
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
            if (Input.GetAxis("R2")>0.3f && !spellCasting && Time.time - spellCooldownStart >= 0.5f)
            {
                spellCooldownStart = Time.time;
                spell = Instantiate(projectile, transform.position + playerModel.forward*2 + Vector3.up * 0.5f, playerModel.transform.rotation).GetComponent<Spell>();
                if (element == SpellType.Fire|| element == SpellType.Ice)
                {
                    spellCasting = true;
                    spell.transform.parent = transform;
                }
                spell.Initialize(element, false);
            }
            if (spellCasting && (Input.GetAxis("R2")<=0.3f || !IsGrounded()))
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
 
    IEnumerator Strafe(Vector3 direction)
    {
        float x = Mathf.Sign(Input.GetAxis("LS-X"));

        for (int i = 1; i <= 30; i++)
        {
            if (!lockRotation)
            {
                Movement(direction*1.5f);
            }
            else
            {
                Movement(transform.GetChild(0).right*x*2);
            }
            yield return null;
        }
        canStrafe = true;
    }


    private void blink()
    {
        var go = FollowCamera.Instance.getFocus();
        if(go != null)
        {
            FollowCamera.Instance.setFocus(go);
        }
        else
        {
            FollowCamera.Instance.setFollow();
        }

        transform.position = transform.position += playerModel.transform.forward * 8;
        if (go != null) {
            transform.GetChild(0).LookAt(new Vector3(go.transform.position.x, transform.position.y, go.transform.position.z));
        }
    }

    private void airPulse()
    {
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(velocity.x, 15, velocity.z);
        rb.AddForce(playerModel.transform.forward*-100);
    }

    private void attack()
    {
        canAttack = false;
        Instantiate(attackCollider, playerModel.transform.position + playerModel.forward * 1.5f + playerModel.up * 0.5f + playerModel.right * 1f, playerModel.transform.rotation).transform.parent = playerModel.transform;
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
    public GameObject getNextEnnemy(GameObject current)
    {
        if(nearbyEnnemies.Count == 1)
        {
            return current;
        }
        else
        {
            int i = nearbyEnnemies.IndexOf(current);
            i++;
            if (i >= nearbyEnnemies.Count)
            {
                i = 0;
            }
            return (GameObject)nearbyEnnemies[i];

        }
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

    private bool IsGrounded() {
        RaycastHit hit;
        return Physics.BoxCast(transform.position, scale/2, Vector3.down, out hit, transform.rotation, 0.2f, -1, QueryTriggerInteraction.Ignore);
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
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            if (Physics.BoxCast(transform.position, scale/2, Vector3.right*Mathf.Sign(targetDirection.x), out hit,transform.rotation, 0.2f))
            {
                transform.position += Vector3.right * hit.distance * 0.99f * Mathf.Sign(targetDirection.x) * speed * Time.deltaTime;// - Vector3.right * transform.localScale.x / 2 * Mathf.Sign(targetDirection.x);
            }
            else
            {
                transform.Translate(Vector3.right * targetDirection.x * speed * Time.deltaTime);
            }
        }
        if (targetDirection.z != 0)
        {
            if (Physics.BoxCast(transform.position, scale/2, Vector3.forward * Mathf.Sign(targetDirection.z), out hit, transform.rotation, 0.2f))
            {
                transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z)*speed * Time.deltaTime;// - Vector3.forward * transform.localScale.z/2 * Mathf.Sign(targetDirection.z);
            }
            else
            {
                transform.Translate(Vector3.forward * targetDirection.z * speed * Time.deltaTime);
            }
        }
    }
    
    /*
    private void Movement(Vector3 targetDirection)
    {
        playerModel.forward = targetDirection;
        RaycastHit hit;
        if (targetDirection.magnitude != 0)
        {
            if (Physics.BoxCast(transform.position, scale / 2, Vector3.right * Mathf.Sign(targetDirection.x), out hit, transform.rotation, 0.2f))
            {
                rb.velocity = new Vector3 (0, rb.velocity.y, 0);
                transform.position += Vector3.right * hit.distance * 0.99f * Mathf.Sign(targetDirection.x) * speed * Time.deltaTime;// - Vector3.right * transform.localScale.x / 2 * Mathf.Sign(targetDirection.x);
            }
            else
            {
                rb.AddForce(playerModel.forward *targetDirection.magnitude*30);
            }
        }
    }
    */
}
