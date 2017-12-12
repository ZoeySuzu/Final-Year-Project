using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public class Switch : MonoBehaviour {

    [SerializeField]
    private bool staysDown;
    [SerializeField]
    public GameObject targetObject;
    [SerializeField]
    private bool targetActive;

    private GameObject button;
    private Material material;
    private Color color;

    private int counter;
    private bool pressed = false;

    private void Start()
    {
        counter =  0;
        button = gameObject.transform.GetChild(0).gameObject;
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
        targetObject.SetActive(targetActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Spell" && other.tag != "Zone")
        {
            if (!pressed)
            {
                button.transform.position += Vector3.down * 0.05f;
                material.color = color;
                pressed = true;
                switchActive();
            }
            counter++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Spell" && other.tag != "Zone")
        {
            counter--;
            if (!staysDown & counter == 0)
            {
                button.transform.position += Vector3.up * 0.05f;
                material.color = Color.red;
                pressed = false;
                switchActive();
            }
        }
    }

    public void switchActive()
    {
        targetObject.SetActive(!targetObject.activeSelf);
    }
}
