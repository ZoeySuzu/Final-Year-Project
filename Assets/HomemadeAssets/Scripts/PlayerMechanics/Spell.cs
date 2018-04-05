using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Spell : MonoBehaviour {


    private SpellType element, normalElement;
    private float speed;
    private Material material;
    private ParticleSystem activeParticle;
    private Vector3 direction;
    private ParticleSystem particle;
    private bool morph;
    private bool ennemySpell;

    [SerializeField]
    private ParticleSystem defaultParticle = null, fireParticle = null, iceParticle = null, windParticle = null, electricParticle = null, contactParticle = null;


    public void Initialize(SpellType _element, bool _ennemySpell)
    {
        element = _element;
        normalElement = element;
        ennemySpell = _ennemySpell;
    }

    public SpellType getSpellType()
    {
        return normalElement;
    }

	// Use this for initialization
	void Start () {
        //alternative = false;
        material = transform.GetComponent<Renderer>().material;
        morph = false;
        switch (element)
        {
            case SpellType.Fire:
                {
                    activeParticle = fireParticle;
                    speed = 0;
                    material.color = new Color(225,0,0,100);
                    break;
                }
            case SpellType.Ice:
                {
                    activeParticle = iceParticle;
                    speed = 0;
                    material.color = new Color(0,0,225,100);
                    break;
                }
            case SpellType.Electric:
                {
                    RaycastHit hit;
                    var target = FollowCamera.Instance.getFocus();
                    if (target != null)
                    {
                        transform.position = target.transform.position + Vector3.up * 5;
                    }
                    
                    else if (PlayerController.Instance.getPlayerState().Contains("Aiming"))
                    {
                        Physics.Raycast(FollowCamera.Instance.transform.position, FollowCamera.Instance.transform.forward, out hit, 100, -1, QueryTriggerInteraction.Ignore);
                        transform.position = hit.point + Vector3.up * 5;
                    }
                    else
                    {
                        transform.position = transform.position + transform.forward * 5 + Vector3.up * 5;
                    }
                    
                    material.color = Color.yellow;
                    break;
                }
            case SpellType.Wind:
                {
                    var target = FollowCamera.Instance.getFocus();
                    if (target != null)
                    {
                        direction = Vector3.Normalize(target.GetComponentInChildren<Collider>().transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if (PlayerController.Instance.getPlayerState().Contains("Aiming"))
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(FollowCamera.Instance.transform.position, FollowCamera.Instance.transform.forward, out hit, 50, -1, QueryTriggerInteraction.Ignore))
                        {
                            direction = hit.point - transform.position;
                        }
                        else
                        {
                            direction = FollowCamera.Instance.transform.position + FollowCamera.Instance.transform.forward * 50 - transform.position;
                        }

                        transform.forward = direction;
                    }
                    activeParticle = windParticle;
                    speed = 15;
                    material.color = Color.green;
                    break;
                }
            case SpellType.Normal:
                {
                    var target = FollowCamera.Instance.getFocus();
                    if (ennemySpell)
                    {
                        direction = Vector3.Normalize(PlayerController.Instance.transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if (target != null)
                    {
                        direction = Vector3.Normalize(target.GetComponentInChildren<Collider>().transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if(PlayerController.Instance.getPlayerState().Contains("Aiming"))
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(FollowCamera.Instance.transform.position, FollowCamera.Instance.transform.forward, out hit, 50, -1, QueryTriggerInteraction.Ignore))
                        {
                            direction = hit.point - transform.position;
                        }
                        else
                        {
                            direction = FollowCamera.Instance.transform.position+ FollowCamera.Instance.transform.forward*50 - transform.position;
                        }

                        transform.forward = direction;
                    }

                    else{
                        direction = Vector3.forward;
                    }
                    speed = 15;
                    activeParticle = defaultParticle;
                    material.color = Color.magenta;
                    break;
                }
        }
         particle = Instantiate(activeParticle,transform);
    }
	
	// Update is called once per frame
	void Update () {
        switch (element)
        {
            case SpellType.Fire:
                {
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward * 0.6f + transform.parent.GetChild(0).right*0.5f;
                    var target = FollowCamera.Instance.getFocus();
                    if (target != null)
                    {
                        direction = Vector3.Normalize(target.GetComponentInChildren<Collider>().transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if (PlayerController.Instance.getPlayerState().Contains("Aiming"))
                    {
                        direction = FollowCamera.Instance.transform.position + FollowCamera.Instance.transform.forward * 50 - transform.position;
                        transform.forward = direction;
                    }
                    break;
                }
            case SpellType.Ice:
                {
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward * 0.6f + Vector3.up * 0.5f + transform.parent.GetChild(0).right * 0.5f;
                    var target = FollowCamera.Instance.getFocus();
                    if (target != null)
                    {
                        direction = Vector3.Normalize(target.GetComponentInChildren<Collider>().transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if (PlayerController.Instance.getPlayerState().Contains("Aiming"))
                    {
                        direction = FollowCamera.Instance.transform.position + FollowCamera.Instance.transform.forward * 50 - transform.position;
                        transform.forward = direction;
                    }
                    break;
                }
            case SpellType.Electric:
                {
                    break;
                }
            case SpellType.Wind:
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                }
            case SpellType.Normal:
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
                    break;
                }
        }
	}

    public void setMorph(SpellType morphElement)
    {
        if(element == SpellType.Normal && !morph)
        {
            morph = true;
            Destroy(particle);
            normalElement = morphElement;
            switch (morphElement)
            {
                case SpellType.Fire:
                    {
                        material.color = new Color(225, 0, 0, 100);
                        break;
                    }
                case SpellType.Ice:
                    {
                        material.color = new Color(0, 0, 225, 100);
                        break;
                    }
                case SpellType.Electric:
                    {
                        material.color = Color.yellow;
                        break;
                    }
                case SpellType.Wind:
                    {
                        material.color = Color.green;
                        break;
                    }
            }
        }
    }

    public void stopCast()
    {
        Instantiate(contactParticle,transform.position,transform.rotation);
        try { Destroy(gameObject); }
        catch (Exception e) { throw e; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            EnemyController ec = other.GetComponentInParent<EnemyController>();
            if(element == SpellType.Normal)
            {
                ec.setHealth(-3);
                ec.knockback(transform.forward+Vector3.up);
            }
            if (element == SpellType.Wind)
            {
                ec.setHealth(-2);
                ec.knockback(transform.forward*15f);
            }
        }
        if(other.tag == "Player" && ennemySpell == true)
        {
            stopCast();
            Debug.Log("Should Damage");
            PlayerController.Instance.setHealth(-3);
            StartCoroutine(PlayerController.Instance.PushBack((PlayerController.Instance.transform.position - transform.position) * 2));
        }

        if (other.tag == "Zone" || other.tag == "Player")
            return;
        if (element != SpellType.Fire && element != SpellType.Ice)
            stopCast();

    }
    private void OnTriggerExit(Collider other)
    {
    }
}
