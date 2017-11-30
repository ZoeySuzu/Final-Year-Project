using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class SpellReactor : MonoBehaviour
{
    [SerializeField]
    private SpellType triggerSpell;

    private enum Reaction {destroy};
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
            }
            else
            {
                Destroy(other.gameObject);
            }
        }
    }
}