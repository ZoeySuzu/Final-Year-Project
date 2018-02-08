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
    private ParticleSystem defaultParticle = null, fireParticle = null, iceParticle = null, windParticle = null, electricParticle = null, contactParticle;


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
                    Vector3 rayDir;
                    var target = FollowCamera.Instance.targetedEnnemy();
                    if (target != null)
                    {
                        transform.position = target.transform.position + Vector3.up * 5;
                    }
                    
                    else if (Input.GetAxis("L2") > 0.5f)
                    {
                        rayDir = transform.position - FollowCamera.Instance.transform.position;
                        Physics.Raycast(transform.position, rayDir, out hit, 100, -1, QueryTriggerInteraction.Ignore);
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
                    else if(Input.GetAxis("L2") > 0.5f){
                        direction = transform.position - FollowCamera.Instance.transform.position;
                        transform.rotation = Quaternion.LookRotation(direction);
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
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward * 1.1f + Vector3.up * 0.5f ;
                    transform.rotation = transform.parent.GetChild(0).rotation;
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
        if (other.tag == "Zone")
            return;
        if (element != SpellType.Fire && element != SpellType.Wind)
            stopCast();

        if (element == SpellType.Wind && other.gameObject.name == "Object_Player")
        {
            Debug.Log("Increase jump");
            other.gameObject.GetComponent<PlayerController>().setJumpHeight(2.0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (element == SpellType.Wind && other.gameObject.name == "Object_Player")
        {
            other.gameObject.GetComponent<PlayerController>().setJumpHeight(1.5f);
        }
    }
}
