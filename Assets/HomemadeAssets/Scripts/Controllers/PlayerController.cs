using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        stats.Add(mana = new Stat("Mana", 100));
        stats.Add(health = new Stat("Health", 100));
    }

    //-----------------------------------------------------------------------------------

    //Public Variables for debuging
    private int pad;
    private Vector3 scale;
    private string playerState;
    private Vector3 spawn;
    [SerializeField]
    private bool unlockAbilities= false;

    //Pointers to other classes
    private Rigidbody rb;
    private UIController ui;
    private Transform playerModel;
    private Material mat;
    private Color defaultColor;
    public GameObject attackCollider;
    public Animator anim;
    public Unlocks unlocked;

    //Ability related variables
    [SerializeField]
    private GameObject projectile = null, trap = null, shield = null;

    private SpellType element;
    private Spell spell;
    private bool spellCasting;
    private Trap[] spellTrap = new Trap[5];
    private Cooldown spellCooldown = new Cooldown(0.5f);

    //State related variables
    [SerializeField]
    private float speed, jumpHeight;
    private bool inAir;

    private bool lockRotation,dPadUpdate, grounded;

    public bool interacting;
    public List<Stat> stats = new List<Stat>();
    private Stat health, mana;

    //Combat data
    private ArrayList nearbyEnnemies;
    private bool canAttack, canStrafe;
    

    //------------------------------------------------------Initialising Code


    void Start () {
        unlocked = new Unlocks();
        if (unlockAbilities)
        {
            unlocked.abilities[1] = true;
            unlocked.abilities[2] = true;
            unlocked.abilities[3] = true;
            unlocked.abilities[4] = true;
        }

        nearbyEnnemies = new ArrayList();
        mat = GetComponentInChildren<Renderer>().material;
        defaultColor = mat.color;
        anim = transform.GetChild(0).GetComponent<Animator>();
        ui = UIController.Instance;
        rb = GetComponent<Rigidbody>();
        scale = GetComponent<BoxCollider>().size;
        playerModel = transform.GetChild(0);

        ui.updateHealth(health);
        ui.updateMana(mana);
        playerState = "";
        pad = 0;
        spawn = transform.position;
        scale.y = scale.y * 0.9f;
        element = SpellType.Normal;

        inAir = false;
        spellCasting = false;
        lockRotation = false;
        grounded = false;
        canAttack = true;
        canStrafe = true;

    }

    public void updateStats(List<Stat> _stats)
    {
        ui = UIController.Instance;
        foreach (Stat stat in _stats)
        {
            Debug.Log("updating stat");

            if (stat.statName == "Health")
            {
                Debug.Log("health " + stat.statValue);
                health = stat;
                ui.updateHealth(health);
            }
            else if(stat.statName == "Mana")
            {
                mana = stat;
                ui.updateMana(mana);
            }
        }
    }


    //------------------------------------------------------Update Code
    void FixedUpdate()
    {
        CheckBounds();
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        /*var mag = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
        if ( mag > speed)
        {
            rb.velocity = new Vector3(rb.velocity.x / mag * speed, rb.velocity.y, rb.velocity.z / mag * speed);
        }*/
        if (rb.velocity.y < 0 && grounded)
        {
            rb.velocity = Vector3.zero;
        }
        else {
            anim.ResetTrigger("Grounded");
            anim.SetTrigger("Falling");
        }
        
        if (rb.velocity.y< 0 && !inAir)
        {
            rb.velocity += Physics.gravity*1.5f * Time.fixedDeltaTime;
        }
    }
	
	void Update () {
        //Climbing
        if (playerState.Contains("Climbing"))
        {
            float z = Input.GetAxis("LS-Y");
            rb.velocity = Vector3.zero;
            if (z != 0)
            {
                transform.Translate(Vector3.up * z*3f*Time.deltaTime);
            }
        }
        //Pushing
        else if (playerState.Contains("Pushing"))
        {
            ui.setInteractButton("Let go");
        }
        //Not Doing other action
        else
        {
            playerState = "";
            grounded = IsGrounded();
            //Check for directional input
            var x = Input.GetAxis("LS-X");
            var z = Input.GetAxis("LS-Y");
            Vector3 targetDirection = new Vector3(x, 0f, z);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;
            lockRotation = false;

            GameObject focus = FollowCamera.Instance.getFocus();
            if (focus != null)
            {
                lockRotation = true;
                Vector3 orientation = new Vector3(focus.transform.position.x, playerModel.position.y, focus.transform.position.z);
                playerModel.LookAt(orientation);
            }

            SelectElement();
            Aim();
            TrapCast();
            SpellCast();
            Dash(targetDirection);
            Attack();
            Shield();
            Jump();
            Move(targetDirection);
        }

        ui.setPlayerState(playerState);
        ui.setSpellState(element.ToString());
        interacting = false;
    }

    //------------------------------------------------------Action Methods

    private void Shield()
    {
        if (Input.GetButtonDown("R1"))
        {
            var activeShield = Instantiate(shield, transform.position, playerModel.transform.rotation);
        }
    }

    private void Aim()
    {
        if (!FollowCamera.Instance.lerping && (Input.GetAxis("R2") > 0.5f || Input.GetButton("R2")) && !FollowCamera.Instance.aim && FollowCamera.Instance.getFocus() == null)
        {
            playerState += "Aiming";
            FollowCamera.Instance.setAim(true);    
        }
        else if ((Input.GetAxis("R2") > 0.5f || Input.GetButton("R2")) && FollowCamera.Instance.aim)
        {
            ui.toggleCrosshair(true);
            playerState += "Aiming";
            playerModel.forward = new Vector3(FollowCamera.Instance.transform.forward.x, 0, FollowCamera.Instance.transform.forward.z);
        }
        else if(FollowCamera.Instance.aim)
        {
            ui.toggleCrosshair(false);
            FollowCamera.Instance.setAim(false);
        }
    }

    private void SpellCast()
    {
        if (((Input.GetAxis("R2")>0.5f || Input.GetButton("R2")) && (FollowCamera.Instance.getFocus()!= null) || (playerState.Contains("Aiming") && Input.GetButtonDown("X"))) && !spellCasting && spellCooldown.ready)
        {
            StartCoroutine(spellCooldown.StartCooldown());
            spell = Instantiate(projectile, transform.position + playerModel.forward * 0.5f+playerModel.right*0.5f, playerModel.transform.rotation).GetComponent<Spell>();
            if (element == SpellType.Fire || element == SpellType.Ice)
            {
                spellCasting = true;
                spell.transform.parent = transform;
                anim.SetBool("Casting", true);
            }
            spell.Initialize(element, false);
            PlayerController.Instance.anim.SetTrigger("Attack1");
        }
        if (spellCasting && ((Input.GetAxis("R2") <= 0.3f || Input.GetButtonUp("R2")) || !grounded || Input.GetButtonUp("X")))
        {
            anim.SetBool("Casting", false);
            spell.stopCast();
            spellCasting = false;
        }
        if (spellCasting)
        {
            playerState += "Casting";
        }
    }

    private void TrapCast()
    {
        if (grounded && Input.GetButtonDown("Y"))
        {
            int i = (int)element;
            if (spellTrap[i] == null)
            {
                spellTrap[i] = Instantiate(trap, transform.position - new Vector3(0, transform.localScale.y, 0), playerModel.transform.rotation).GetComponent<Trap>();
                spellTrap[i].transform.SetParent(transform.parent);
                spellTrap[i].Initialize(element);
            }
            else
            {
                spellTrap[i].setOff();
            }
        }
    }

    private void SelectElement()
    {
        if (pad == 0)
        {
            dPadUpdate = true;
            if (Input.GetAxisRaw("D-Y") < 0) { pad = -1; }
            else if (Input.GetAxisRaw("D-Y") > 0) { pad = 1; }
            else if (Input.GetAxisRaw("D-X") < 0) { pad = -2; }
            else if (Input.GetAxisRaw("D-X") > 0) { pad = 2; }
        }
        else if (Input.GetAxisRaw("D-Y") == 0 && Input.GetAxisRaw("D-X") == 0)
        {
            pad = 0;
        }

        if (Input.GetButtonDown("D-U") || (pad == -1 && dPadUpdate))
        {
            if (element == SpellType.Electric)
            {
                ui.setActiveElement(SpellType.Normal);
                element = SpellType.Normal;
            }
            else if (unlocked.abilities[4])
            {
                ui.setActiveElement(SpellType.Electric);
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
            {
                ui.setActiveElement(SpellType.Normal);
                element = SpellType.Normal;
            }
            else if (unlocked.abilities[1])
            {
                ui.setActiveElement(SpellType.Fire);
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
            {
                ui.setActiveElement(SpellType.Normal);
                element = SpellType.Normal;
            }
            else if (unlocked.abilities[3])
            {
                ui.setActiveElement(SpellType.Wind);
                element = SpellType.Wind;
                if (spell)
                {
                    spell.setMorph(SpellType.Wind);
                }
            }
        }
        else if (Input.GetButtonDown("D-R") || (pad == 2 && dPadUpdate))
        {
            if (element == SpellType.Ice)
            {
                ui.setActiveElement(SpellType.Normal);
                element = SpellType.Normal;
            }
            else if (unlocked.abilities[2])
            {
                ui.setActiveElement(SpellType.Ice);
                element = SpellType.Ice;
                if (spell)
                {
                    spell.setMorph(SpellType.Ice);
                }
            }
        }
        dPadUpdate = false;
    }

    private void Attack()
    {

        if (!playerState.Contains("Aiming") && Input.GetButtonDown("X") && !interacting && canAttack)
        {
            canAttack = false;
            Instantiate(attackCollider, playerModel.transform.position + playerModel.forward * 1.5f + playerModel.up * 0.5f + playerModel.right * 1f, playerModel.transform.rotation).transform.parent = playerModel.transform;
        }else if (playerState.Contains("Aiming") && !canAttack)
        {
            playerState += "Attacking";
        }
    }


    //------------------------------------------------------Get Methods
    public float getSpeed(){return speed;}
    public string getPlayerState() {return playerState;}
    public int getMaxHP() {return health.statMax;}
    public int getMaxMana(){return mana.statMax;}
    public int getHP() {return health.statValue;}

    public GameObject getHand()
    {
        return transform.FindChildByRecursive("LeftHand_end").gameObject;
    }

    public GameObject getWand()
    {
        return transform.FindChildByRecursive("Wand").gameObject;
    }

    public GameObject getNextEnnemy(GameObject current)
    {
        if (nearbyEnnemies.Count == 0)
            return null;
        else if (nearbyEnnemies.Count == 1)
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
                if(ennemy == null)
                {
                    nearbyEnnemies.Remove(ennemy);
                    return null;
                }
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

    private void setMana(int _mana)
    {
        mana.changeStat(_mana);
        ui.updateMana(mana);
    }

    public void setHealth(int _health)
    {
        if(_health < 0)
        {
            anim.SetTrigger("Damage");
            Debug.Log("Took Damage");
            StartCoroutine(TakeDamage());
        }
        health.changeStat(_health);
        ui.updateHealth(health);
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

    private void CheckBounds()
    {
        if (transform.position.y < -20)
        {
            setHealth(-10);
            transform.position = spawn;
        }
    }

    IEnumerator TakeDamage()
    {
        for (int i = 0; i <= 4; i++)
        {
            mat.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            mat.color = defaultColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator PushBack(Vector3 force)
    {
        for (int i = 0; i <= 30; i++)
        {
            transform.position = transform.position + force * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }

    private bool IsGrounded() {
        RaycastHit hit;
        if(Physics.BoxCast(transform.position, scale/2, Vector3.down, out hit, transform.rotation, 0.1f, -1, QueryTriggerInteraction.Ignore))
        {
            anim.SetTrigger("Grounded");
            anim.ResetTrigger("Falling");
            ui.setActionButton("Jump");
            playerState += "Grounded";
            return true;
        }
        else
        {
            playerState += "Falling";
            ui.setActionButton("");
            return false;
        }
     }


    //------------------------------------------------------Collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnnemyTrigger")
        {
            nearbyEnnemies.Add(other.transform.parent.gameObject);
        }
        if (other.GetComponent<WindArea>() != null)
        {
            inAir = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "EnnemyTrigger")
        {
            if(nearbyEnnemies.Contains(other.transform.parent.gameObject))
                nearbyEnnemies.Remove(other.transform.parent.gameObject);
        }
        if (other.GetComponent<WindArea>() != null)
        {
            inAir = false;
        }
    }

    //------------------------------------------------------Movement Methods
    private void Move(Vector3 direction)
    {
        //Check movement state
        if (direction != Vector3.zero && canStrafe)
        {
            playerState += "Walking";
            if (playerState.Contains("Aiming") || playerState.Contains("Attacking"))
            {
                Movement(direction, speed * 0.30f);
                anim.SetFloat("Movement", direction.magnitude*0.30f);
            }
            else
            {
                Movement(direction, speed * 1f);
                if (!lockRotation && !FollowCamera.Instance.lerping && FollowCamera.Instance.getFocus() == null && Input.GetAxis("L2") <= 0.2f)
                {
                    playerModel.forward = direction;
                }
                anim.SetFloat("Movement", direction.magnitude);
            } 
        }
        else
        {
            playerState += "Idle";
            anim.SetFloat("Movement", direction.magnitude);
        }
        
    }


    IEnumerator Strafe(Vector3 direction)
    {
        for (int i = 1; i <= 30; i++)
        {
            if (!lockRotation)
            {
                if (Movement(direction, speed * 2f) == true)
                    break;
            }
            else
            {
                float x = Mathf.Sign(Input.GetAxis("LS-X"));
                if (Movement(playerModel.right * x, speed * 2.5f) == true)
                    break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        canStrafe = true;
    }


    private void Blink(Vector3 targetDirection)
    {
        var go = FollowCamera.Instance.getFocus();
        if (go != null)
        {
            FollowCamera.Instance.setFocus(go);
        }
        else
        {
            FollowCamera.Instance.setFollow();
        }

        transform.position = transform.position += targetDirection * 5;
        if (go != null)
        {
            playerModel.LookAt(new Vector3(go.transform.position.x, transform.position.y, go.transform.position.z));
        }
    }

    private void DoubleJump()
    {
        Vector3 velocity = rb.velocity;
        rb.velocity = new Vector3(velocity.x, 15, velocity.z);
        rb.AddForce(playerModel.transform.forward * -100);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("A") && grounded && !interacting)
        {
            anim.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.fixedDeltaTime);
            playerState += "Jumping";
            ui.setActionButton("");
        }
    }

    private void Dash(Vector3 direction)
    {
        if ((Input.GetButtonDown("B") || (Input.GetButtonDown("LS"))) && canStrafe)
        {
            if (element == SpellType.Electric)
            {
                Blink(direction);
            }
            else if (grounded)
            {
                canStrafe = false;
                StartCoroutine(Strafe(direction));
            }
            else if (element == SpellType.Wind)
            {
                canStrafe = false;
                StartCoroutine(Strafe(direction));
            }
        }
    }

    private bool Movement(Vector3 targetDirection,float _speed)
    {
        
        bool collision = false;
        float dashCheck = 0.2f;
        if (_speed >= speed * 2f) { dashCheck = 0.4f; }
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            if (Physics.BoxCast(transform.position + Vector3.up * (dashCheck - 0.2f), scale/2, Vector3.right*Mathf.Sign(targetDirection.x), out hit,transform.rotation, dashCheck))
            {
                transform.position += Vector3.right * hit.distance * 0.99f * Mathf.Sign(targetDirection.x) * _speed * Time.deltaTime;// - Vector3.right * transform.localScale.x / 2 * Mathf.Sign(targetDirection.x);
                collision = true;
            }
            else
            {
                transform.position += (Vector3.right * targetDirection.x * _speed * Time.deltaTime);
            }
        }
        if (targetDirection.z != 0)
        {
            if (Physics.BoxCast(transform.position+Vector3.up*(dashCheck-0.2f), scale/2, Vector3.forward * Mathf.Sign(targetDirection.z), out hit, transform.rotation, dashCheck))
            {
                transform.position += Vector3.forward * hit.distance * 0.99f* Mathf.Sign(targetDirection.z)*_speed * Time.deltaTime;// - Vector3.forward * transform.localScale.z/2 * Mathf.Sign(targetDirection.z);
                collision = true;
            }
            else
            {
                transform.position += (Vector3.forward * targetDirection.z * _speed * Time.deltaTime);
            }
        }
        return collision;
    }
}
