using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{
    protected UIController gameUI;
    protected string interaction = "";

    private GameObject indicator;
    private GameObject indicatorLive;

    public abstract void OnTriggerStay(Collider other);

    private void OnTriggerEnter(Collider other)
    {
         
        indicator = gameUI.indicator;
        indicatorLive = Instantiate(indicator);
        indicatorLive.transform.parent = gameObject.transform;
        indicatorLive.transform.position = transform.position ;
        gameUI.setInteractButton(interaction);
    }

    private void OnTriggerExit(Collider other)
    {
        collisionExit();
        Destroy(indicatorLive);
        gameUI.setInteractButton("");
    }

    public virtual void collisionExit() {}

}