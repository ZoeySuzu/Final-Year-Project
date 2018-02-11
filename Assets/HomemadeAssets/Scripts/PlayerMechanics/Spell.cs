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

    [SerializeField]
    private ParticleSystem defaultParticle = null, fireParticle = null, iceParticle = null, windParticle = null, electricParticle = null, contactParticle = null;


    public void Initialize(SpellType _element, bool _alternative)
    {
        element = _element;
        normalElement = element;
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
                    var target = FollowCamera.Instance.targetedEnnemy();
                    if (target != null)
                    {
                        transform.position = target.transform.position + Vector3.up * 5;
                    }
                    
                    else if (Input.GetAxis("R2") > 0.5f)
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
                    activeParticle = windParticle;
                    speed = 0;
                    material.color = Color.green;
                    break;
                }
            case SpellType.Normal:
                {
                    var target = FollowCamera.Instance.targetedEnnemy();
                    if (target != null)
                    {
                        direction = Vector3.Normalize(target.transform.position - transform.position);
                        transform.rotation = Quaternion.LookRotation(direction);
                    }
                    else if(Input.GetAxis("R2") > 0.5f){
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
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward * 1.1f + Vector3.up * 0.5f;
                    if (Input.GetAxis("R2") > 0.5f)
                    {
                        direction = FollowCamera.Instance.transform.position + FollowCamera.Instance.transform.forward * 50 - transform.position;
                        transform.forward = direction;
                    }
                    break;
                }
            case SpellType.Ice:
                {
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward * 1.1f + Vector3.up * 0.5f;
                    transform.rotation = transform.parent.GetChild(0).rotation;
                    break;
                }
            case SpellType.Electric:
                {
                    break;
                }
            case SpellType.Wind:
                {
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

            EnemyController ec = other.GetComponent<EnemyController>();
            if(element == SpellType.Normal)
            {
                ec.setHealth(-2);
                ec.knockback(transform.forward+Vector3.up);
            }
        }

        if (other.tag == "Zone")
            return;
        if (element != SpellType.Fire && element != SpellType.Wind)
            stopCast();

    }
    private void OnTriggerExit(Collider other)
    {
    }
}
