using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : Interactable {

    private int fp;

    [SerializeField]
    private string name;
    [SerializeField]
    private string dialogueID; 

    private Queue<string> textQueue;


    public TestDialogue()
    {
        interaction = "Talk";
    }

    public void talk()
    {
            
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && !TextController.Instance.gameObject.activeSelf)
        {
            TextController.Instance.sendDialogueRequest(dialogueID, name);
        }
    }
}
