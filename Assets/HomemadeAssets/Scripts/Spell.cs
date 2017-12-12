using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Spell : MonoBehaviour {


    private SpellType element;
    private float speed;
    //private bool alternative;
    private Material material;
    private ParticleSystem activeParticle;

    [SerializeField]
    private ParticleSystem defaultParticle = null, fireParticle = null, iceParticle = null, windParticle = null, electricParticle = null;


    public void Initialize(SpellType _element, bool _alternative)
    {
        element = _element;      
    }

    public SpellType getSpellType()
    {
        return element;
    }

	// Use this for initialization
	void Start () {
        //alternative = false;
        material = transform.GetComponent<Renderer>().material;
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
                    speed = 5;
                    material.color = new Color(0,0,225,100);
                    break;
                }
            case SpellType.Electric:
                {
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
                    speed = 10;
                    activeParticle = defaultParticle;
                    material.color = Color.magenta;
                    break;
                }
        }
        Instantiate(activeParticle,transform);
    }
	
	// Update is called once per frame
	void Update () {
        switch (element)
        {
            case SpellType.Fire:
                {
                    transform.position = transform.parent.position + transform.parent.GetChild(0).forward*2;
                    transform.rotation = transform.parent.GetChild(0).rotation;
                    break;
                }
            case SpellType.Ice:
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime);
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

    public void stopCast()
    {
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
