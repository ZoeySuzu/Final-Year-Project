using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cleanup: 08/01/2018
//Todo: Implement automatic activation; add traps for fire, electricity and normal elements.

public class Trap : MonoBehaviour {

    private SpellType element;
    private Material material;

    [SerializeField]
    private GameObject fireArea = null, iceCube = null, electricityArea = null, windArea= null, explosion = null;

    //Set the type of trap
    public void Initialize(SpellType _element)
    {
        element = _element;
    }

    public SpellType getSpellType()
    {
        return element;
    }

    //Set the display based on the type and add trap to the entity list.
    void Start()
    {
        transform.SetParent(transform.parent.FindChild("Spells"));
        material = transform.GetComponent<Renderer>().material;
        switch (element)
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
            case SpellType.Normal:
                {
                    material.color = Color.magenta;
                    break;
                }
        }
    }

    //Detonate the trap and get rid of previous trap of same type.
    //Fire: Creates a ball of fire
    //Ice: Creates a block of Ice
    //Electric: Creates an electricity conductor
    //Wind: Creates a wind area
    //Normal: creates an explosion

    public void setOff()
    {
        GameObject obj = null;
        switch (element)
        {
            case SpellType.Fire:
                {
                    obj = Instantiate(fireArea, transform.position, Quaternion.identity) as GameObject;
                    break;
                }
            case SpellType.Ice:
                {
                    obj = Instantiate(iceCube, transform.position + transform.up, Quaternion.identity) as GameObject;
                    break;
                }
            case SpellType.Electric:
                {
                    
                    break;
                }
            case SpellType.Wind:
                {
                    obj = Instantiate(windArea, transform.position, Quaternion.identity) as GameObject;
                    break;
                }
            case SpellType.Normal:
                {
                    obj = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
                    break;
                }
        }
        try
        {
            Transform t = transform.parent.FindChild(obj.name);
            if (t != null)
            {
                Destroy(t.gameObject);
            }
        }
        catch (Exception) { }
        try
        {
            obj.transform.SetParent(transform.parent);
        }
        catch (Exception) { }
        Destroy(gameObject);
    }

    //Automatically activate 
    private void OnTriggerEnter(Collider other)
    {
    }
}
