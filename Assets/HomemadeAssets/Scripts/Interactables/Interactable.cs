using System.Collections;
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
            if (Input.GetButtonDown("X"))
            {
                Vector3 orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y,transform.position.z);
                PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
                interact();
                PlayerController.Instance.switchInteracting();
            }
        }
    }


    private void Awake()
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
            gameUI.setInteractButton(interaction);
        }
    }

    protected void toggleIndicator()
    {
        indicatorLive.SetActive(!indicatorLive.activeSelf);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Object_Player")
        {
            collisionExit();
            Destroy(indicatorLive);
            gameUI.setInteractButton("Attack");
        }
    }

    public virtual void collisionExit() {}

}