using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017

public abstract class Interactable : MonoBehaviour{
    protected UIController gameUI;
    protected string interaction = "";

    private GameObject indicatorLive;

    public abstract void OnTriggerStay(Collider other);

    private void Start()
    {
        gameUI = GetComponentInParent<UIController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Object_Player")
        {
            indicatorLive = Instantiate(gameUI.getInteractIcon());
            indicatorLive.transform.parent = gameObject.transform;
            indicatorLive.transform.position = transform.position;
            gameUI.setInteractButton(interaction);
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
            gameUI.setInteractButton("");
        }
    }

    public virtual void collisionExit() {}

}