using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : Interactable {

    [SerializeField]
    private string charName;
    [SerializeField]
    private string dialogueID;

    private Transform head;
    private Vector3 ppos = Vector3.zero;
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

            ppos = PlayerController.Instance.transform.position;
            if (Input.GetButtonDown("X"))
            {
                ppos = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
                PlayerController.Instance.transform.GetChild(0).LookAt(ppos);
                interact();
                PlayerController.Instance.switchInteracting();
            }
        }
        else
        {
            ppos = Vector3.zero;
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
