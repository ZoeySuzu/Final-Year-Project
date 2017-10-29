using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellReactor : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Spell>())
        {

            if (other.gameObject.GetComponent<Spell>().getSpellType() == SpellType.Fire)
            {
                Destroy(other.gameObject);
                Destroy(gameObject);
            }
        }
    }
}