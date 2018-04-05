using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    [SerializeField]
    private int maxHp;
    private int hp;

    private Rigidbody rb;
    private Material mat;
    private Color defaultColor;
    [SerializeField]
    private EnemyAIType aiType;

    private Animator anim;
    private Vector3 attackDir= Vector3.zero;

    public Transform projectile;

    private bool aggro = false;

    private Cooldown attack = new Cooldown(3.0f);

	// Use this for initialization
	void Start () {
        mat = GetComponentInChildren<Renderer>().material;
        defaultColor = mat.color;
        hp = maxHp ;
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (aiType == EnemyAIType.ground)
        {
            if (aggro && hp >0)
            {
                Vector3 playerHPos = new Vector3(PlayerController.Instance.transform.position.x, 0, PlayerController.Instance.transform.position.z);
                transform.LookAt(playerHPos + Vector3.up * transform.position.y);

                Vector3 dir = PlayerController.Instance.transform.position - transform.position;
                dir = new Vector3(dir.x, 0, dir.z);
                if (dir.magnitude >= 2)
                {
                    if(anim.GetBool("Moving") != true)
                        anim.SetBool("Moving", true);
                    dir = dir.normalized;
                    transform.position += dir * 2 * Time.fixedDeltaTime;
                }
                else if (attack.ready)
                {
                    anim.SetBool("Moving", false);
                    StartCoroutine(attack.StartCooldown());
                    Debug.Log("Attack");
                    anim.SetTrigger("Attack");
                    dir = dir.normalized;
                    attackDir = dir;
                    StartCoroutine(Attack());
                }
                else
                {
                    transform.position += attackDir * Time.fixedDeltaTime;
                }
            }
            else
            {
                anim.SetBool("Moving", false);
            }

        }
        if (aiType == EnemyAIType.flying)
        {
            if (aggro && hp > 0)
            {
                transform.LookAt(PlayerController.Instance.transform.position);

                Vector3 dir = PlayerController.Instance.transform.position+ Vector3.up - transform.position;
                if (dir.magnitude >= 5)
                {
                    if (anim.GetBool("Moving") != true)
                        anim.SetBool("Moving", true);
                    dir = dir.normalized;
                    transform.position += dir * 2 * Time.fixedDeltaTime;
                }
                else if (attack.ready)
                {
                    StartCoroutine(attack.StartCooldown());
                    anim.SetBool("Moving", false);
                    Debug.Log("Attack");
                    anim.SetTrigger("Attack");
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                anim.SetBool("Moving", false);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("Should Damage");
            PlayerController.Instance.setHealth(-5);
            StartCoroutine(PlayerController.Instance.PushBack((PlayerController.Instance.transform.position - transform.position)*5));
        }
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == PlayerController.Instance.gameObject)
        {
            Debug.Log("EnemmyTriggered");
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            aggro = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == PlayerController.Instance.gameObject)
        {
            aggro = false;
        }
    }



    public void knockback(Vector3 force)
    {
        if(aiType == EnemyAIType.ground)
            rb.AddForce(Vector3.up*force.y*100);
        StartCoroutine(PushBack(new Vector3(force.x, 0, force.z)));
    }

    public void setHealth(int value)
    {
        hp = hp + value;
        StartCoroutine(TakeDamage());
        if(aiType == EnemyAIType.flying)
        {
            anim.SetTrigger("Damage");
        }
        if (hp <= 0)
        {
            onDeath();
        }
    }

    public void onDeath()
    {
        StartCoroutine(Die());
    }

    IEnumerator PushBack(Vector3 force)
    {
        for (int i = 0; i <= 30; i++)
        {
            transform.position = transform.position + force * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
        var spell = Instantiate(projectile, transform.position + transform.forward + transform.up, transform.rotation).GetComponent<Spell>();
        spell.Initialize(SpellType.Normal, true);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.8f);
        attackDir = Vector3.zero;
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
    IEnumerator Die()
    {
        defaultColor = Color.black;
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.8f);
        if (FollowCamera.Instance.getFocus() == gameObject)
        {
            FollowCamera.Instance.setFollow();
        }
        Destroy(gameObject);
    }

}
