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
                    material.color = Color.red;
                    break;
                }
            case SpellType.Ice:
                {
                    material.color = Color.cyan;
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
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    public void stopCast()
    {
        try { Destroy(gameObject); }
        catch (Exception e) { }
        }

    private void OnCollisionEnter(Collision other)
    {
        if(element != SpellType.Fire)
            stopCast();
    }
}
