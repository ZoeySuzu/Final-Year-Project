using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Switch : MonoBehaviour {

    [SerializeField]
    private bool staysDown;
    [SerializeField]
    public GameObject reactionTarget;
    private SwitchReactor targetTrigger;

    private GameObject button;
    private Material material;
    private Color color;

    private bool pressed = false;
    private bool unUsed = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (unUsed)
            {
                button = gameObject.transform.GetChild(0).gameObject;
                Debug.Log(button.ToString());
                material = button.transform.GetComponent<Renderer>().material;
                material.color = Color.red;
                if (staysDown)
                {
                    color = Color.blue;
                }
                else
                {
                    color = Color.green;
                }
                targetTrigger = reactionTarget.GetComponent<SwitchReactor>();
                unUsed = false;
            }

            if (!pressed)
            {
                button.transform.position += Vector3.down * 0.2f;
                material.color = color;
                pressed = true;
                targetTrigger.switchActive();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (!staysDown)
            {
                button.transform.position += Vector3.up * 0.2f;
                material.color = Color.red;
                pressed = false;
                targetTrigger.switchActive();
            }
        }
    }
}
