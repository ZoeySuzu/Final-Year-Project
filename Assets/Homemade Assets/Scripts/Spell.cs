using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {


    private SpellType element;
    private float speed = 20;
    private bool alternative;
    private Material material;
    private ParticleSystem activeParticle;

    public ParticleSystem fireParticle;
    public ParticleSystem iceParticle;
    public ParticleSystem windParticle;

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
        alternative = false;
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
                    break;
                }
        }
	}

    public void stopCast()
    {
        try { Destroy(gameObject); }
        catch (Exception e) { }
        }

    private void OnCollisionEnter(Collision other)
    {
        

        if(element != SpellType.Fire && element != SpellType.Wind)
            stopCast();
    }

    
    private void OnTriggerEnter(Collider other)
    {
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
