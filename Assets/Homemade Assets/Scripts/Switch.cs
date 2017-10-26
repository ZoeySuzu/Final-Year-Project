using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public bool staysDown;
    public GameObject reactionTarget;

    private SwitchReactor targetTrigger;
    private Color color;
    private bool pressed = false;
    Transform button;
    Material material;

    // Use this for initialization
    void Start() {
        button = transform.GetChild(0);
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
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (!pressed) {
            button.transform.position += Vector3.down* 0.2f;
            material.color = color;
            pressed = true;
            targetTrigger.switchOn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!staysDown)
        {
            button.transform.position += Vector3.up * 0.2f;
            material.color = Color.red;
            pressed = false;
            targetTrigger.switchOff();
        }
    }

    // Update is called once per frame
    void Update () {
        
    }
}
