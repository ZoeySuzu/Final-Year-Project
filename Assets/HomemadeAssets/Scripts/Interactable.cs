﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public abstract class Interactable : MonoBehaviour{
    protected UIController gameUI;
    protected string interaction = "";

    private GameObject indicatorLive;

    public abstract void interact();

    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (Input.GetButtonDown("A"))
            {
                interact();
                PlayerController.Instance.switchInteracting();
            }
        }
    }


    private void Start()
    {
        gameUI = UIController.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Object_Player")
        {
            if (indicatorLive == null)
            {
                indicatorLive = Instantiate(gameUI.getInteractIcon());
            }
            indicatorLive.transform.parent = gameObject.transform;
            indicatorLive.transform.position = transform.position;
            gameUI.setActionButton(interaction);
        }
    }

    public void toggleIndicator()
    {
        indicatorLive.SetActive(!indicatorLive.activeSelf);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Object_Player")
        {
            collisionExit();
            Destroy(indicatorLive);
            gameUI.setActionButton("");
        }
    }

    public virtual void collisionExit() {}

}