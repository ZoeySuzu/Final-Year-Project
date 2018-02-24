using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class SpellReactor : MonoBehaviour
{
    [SerializeField]
    private SpellType triggerSpell;

    [SerializeField]
    private Reaction reaction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Spell>())
        {
            if (other.gameObject.GetComponent<Spell>().getSpellType() == triggerSpell)
            {
                if (reaction == Reaction.destroy)
                {
                    Destroy(gameObject);
                }
                if(reaction == Reaction.push)
                {
                    Lever l = GetComponent<Lever>();
                    Debug.Log("push");
                    if( (transform.parent.localPosition + other.transform.forward).x >= 0)
                    {
                        Debug.Log("right");
                        l.Push(transform.right);
                    }
                    else
                    {
                        Debug.Log("left");
                        l.Push(-transform.right);
                    }
                }
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}