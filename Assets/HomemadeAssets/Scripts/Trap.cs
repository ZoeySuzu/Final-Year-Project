using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    private SpellType element;
    private Material material;

    [SerializeField]
    private GameObject iceCube, windArea;

    public void Initialize(SpellType _element)
    {
        element = _element;
    }

    public SpellType getSpellType()
    {
        return element;
    }

    // Use this for initialization
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

    public void setOff()
    {
        GameObject obj = null;
        switch (element)
        {
            case SpellType.Fire:
                {
                    
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
        catch (Exception e) { }
        try
        {
            obj.transform.SetParent(transform.parent);
        }
        catch (Exception e) { }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
