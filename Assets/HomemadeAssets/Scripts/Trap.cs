using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    private SpellType element;
    private Material material;

    [SerializeField]
    private GameObject iceCube;

    public void Initialize(SpellType _element)
    {
        element = _element;
        transform.SetParent(transform.Find("GameLevel"));
    }

    public SpellType getSpellType()
    {
        return element;
    }

    // Use this for initialization
    void Start()
    {
        //alternative = false;
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
        switch (element)
        {
            case SpellType.Fire:
                {
                    
                    break;
                }
            case SpellType.Ice:
                {
                    GameObject obj = Instantiate(iceCube, transform.position + transform.up, Quaternion.identity) as GameObject;
                    obj.transform.SetParent(transform.parent);
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
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
    }
}
