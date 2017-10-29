using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {


    private SpellType element;
    private float speed;
    private bool alternative;
    private Material material;

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
        speed = 20;
        material = transform.GetComponent<Renderer>().material;
        switch (element)
        {
            case SpellType.Fire:
                {
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
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }
}
