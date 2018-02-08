using System.Collections;
using UnityEngine;

//Last clean: 29/11/2017

public class PlayerController : MonoBehaviour {

    public static PlayerController Instance { get; set; }
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
    }

    //-----------------------------------------------------------------------------------

    //Public Variables for debuging
    private int pad;
    private Vector3 scale;
    private string playerState;
    private Vector3 spawn;

    //Pointers to other classes
    private Rigidbody rb;
    private UIController ui;
    private Transform playerModel;
    public GameObject attackCollider;

    //Ability related variables
    [SerializeField]
    private GameObject projectile = null, trap = null;

    private SpellType element;
    private Spell spell;
    private bool spellCasting;
    private Trap[] spellTrap = new Trap[5];
    private float spellCooldownStart;

    //State related variables
    [SerializeField]
    private float speed = 0, jumpHeight = 0;

    private bool lockRotation,interacting,dPadUpdate, grounded;

    private int health,mana,maxHealth,maxMana;

    //Combat data
    private ArrayList nearbyEnnemies;
    private bool canAttack, canStrafe;
    

    //------------------------------------------------------Initialising Code
   
    void Start () {
        playerState = "";
        nearbyEnnemies = new ArrayList();
        pad = 0;
        spawn = transform.position;
        scale = GetComponent<BoxCollider>().size;
        scale.y = scale.y * 0.9f;
        element = SpellType.Normal;
        GameController.Instance.addEntity(this.gameObject);

        rb = GetComponent<Rigidbody>();
        ui = UIController.Instance;
        playerModel = transform.GetChild(0);

        spellCasting = false;
        lockRotation = false;
        interacting = false;
        grounded = false;
        canAttack = true;
        canStrafe = true;

        maxMana = 100;
        maxHealth = 100;
        setHealth(maxHealth);
        setMana(maxMana);
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

        if(rb.velocity.y<0 && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }
    }
	
	void Update () {


        CheckBounds();

        //Climbing
        if (playerState.Contains("Climbing"))
        {
            float z = Input.GetAxis("LS-Y");
            rb.velocity = Vector3.zero;
            if (z != 0)
            {
                transform.Translate(Vector3.up * z*0.4f);
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
            Jump();
            Move(targetDirection);
        }

        ui.setPlayerState(playerState);
        ui.setSpellState(element.ToString());
        interacting = false;
    }

    //------------------------------------------------------Action Methods
    private void Aim()
    {
        if (!FollowCamera.Instance.getLerp() && Input.GetAxis("L2") > 0.5f)
        {
            playerState += "Aiming";
            playerModel.forward = new Vector3(FollowCamera.Instance.transform.forward.x, 0, FollowCamera.Instance.transform.forward.z);
        }
    }

    private void SpellCast()
    {
        if (Input.GetAxis("R2") > 0.3f && !spellCasting && Time.time - spellCooldownStart >= 0.5f)
        {
            spellCooldownStart = Time.time;
            spell = Instantiate(projectile, transform.position + playerModel.forward * 2 + Vector3.up * 0.5f, playerModel.transform.rotation).GetComponent<Spell>();
            if (element == SpellType.Fire || element == SpellType.Ice)
            {
                spellCasting = true;
                spell.transform.parent = transform;
            }
            spell.Initialize(element, false);
        }
        if (spellCasting && (Input.GetAxis("R2") <= 0.3f || !grounded))
        {
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
                element = SpellType.Normal;
            else
            {
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
        else if (Input.GetButtonDown("D-R") || (pad == 2 && dPadUpdate))
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
    }

    private void Attack()
    {
        if (Input.GetButtonDown("X") && !interacting && canAttack)
        {
            canAttack = false;
            Instantiate(attackCollider, playerModel.transform.position + playerModel.forward * 1.5f + playerModel.up * 0.5f + playerModel.right * 1f, playerModel.transform.rotation).transform.parent = playerModel.transform;
        }else if (!canAttack)
        {
            playerState += "Attacking";
        }
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

    public GameObject getHand()
    {
        return transform.FindChild("holder").gameObject;
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
        mana = _mana;
        ui.updateMana(mana);
    }

    public void setHealth(int _health)
    {
        health += _health;
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
            setHealth(health - 10);
            transform.position = spawn;
        }
    }

    private bool IsGrounded() {
        if(Physics.BoxCast(transform.position, scale/2, Vector3.down, transform.rotation, 0.2f, -1, QueryTriggerInteraction.Ignore))
        {
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
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "EnnemyTrigger")
        {
            if(nearbyEnnemies.Contains(other.transform.parent.gameObject))
                nearbyEnnemies.Remove(other.transform.parent.gameObject);
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
            }
            else
            {
                Movement(direction, speed * 1f);
            }

            if (!lockRotation && !FollowCamera.Instance.getLerp() && FollowCamera.Instance.getFocus() == null && Input.GetAxis("L2") <= 0.5f)
            {
                playerModel.forward = direction;
            }
        }
        else
        {
            playerState += "Idle";
        }
    }


    IEnumerator Strafe(Vector3 direction)
    {
        float x = Mathf.Sign(Input.GetAxis("LS-X"));

        for (int i = 1; i <= 30; i++)
        {
            if (!lockRotation)
            {
                Movement(direction, speed * 2f);
            }
            else
            {
                Movement(playerModel.right * x, speed* 2.5f);
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
            rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
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

    private void Movement(Vector3 targetDirection,float _speed)
    {
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            if (Physics.BoxCast(transform.position, scale/2, Vector3.right*Mathf.Sign(targetDirection.x), out hit,transform.rotation, 0.2f))
            {
                transform.position += Vector3.right * hit.distance * 0.99f * Mathf.Sign(targetDirection.x) * _speed * Time.deltaTime;// - Vector3.right * transform.localScale.x / 2 * Mathf.Sign(targetDirection.x);
            }
            else
            {
                transform.Translate(Vector3.right * targetDirection.x * _speed * Time.deltaTime);
            }
        }
        if (targetDirection.z != 0)
        {
            if (Physics.BoxCast(transform.position, scale/2, Vector3.forward * Mathf.Sign(targetDirection.z), out hit, transform.rotation, 0.2f))
            {
                transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z)*_speed * Time.deltaTime;// - Vector3.forward * transform.localScale.z/2 * Mathf.Sign(targetDirection.z);
            }
            else
            {
                transform.Translate(Vector3.forward * targetDirection.z * _speed * Time.deltaTime);
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
