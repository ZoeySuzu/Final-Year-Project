using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour{
    protected UIController gameUI;
    protected string interaction = "";

    public abstract void OnTriggerStay(Collider other);


    private void OnTriggerEnter(Collider other)
    {
        gameUI.setInteractButton(interaction);
    }

    private void OnTriggerExit(Collider other)
    {
        gameUI.setInteractButton("");
    }

}