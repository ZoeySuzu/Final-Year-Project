using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Interactable {

    [SerializeField]
    private string charName;
    [SerializeField]
    private string dialogueID;

    private Queue<string> textQueue;

    public void Initialize(string _charName)
    {
        name = charName;
    }

    public void Initialize(string _charName, string _dialogueID)
    {
        charName = _charName;
        dialogueID = _dialogueID;
    }
    public NPCController()
    {
        interaction = "Talk";
    }

    public string getName()
    {
        return charName;
    }

    public void setDialogue(string _dialogueID)
    {
        dialogueID = _dialogueID;
    }

    public new void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            Vector3 orientation = new Vector3(PlayerController.Instance.transform.position.x, transform.position.y, PlayerController.Instance.transform.position.z);
            transform.LookAt(orientation);
            if (Input.GetButtonDown("X"))
            {
                orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
                PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
                interact();
                PlayerController.Instance.switchInteracting();
            }
        }
    }

    public override void interact()
    {
        if (!TextController.Instance.gameObject.activeSelf)
        {
            FollowCamera.Instance.setSpecific(gameObject, transform.forward * 5 + transform.up * 2 + transform.right * -3);
            TextController.Instance.sendDialogueRequest(dialogueID, this,false);
        }
    }
}
