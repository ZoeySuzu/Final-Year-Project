using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : Interactable {

    public List<string> signText;
    private Queue<string> textQueue;

    public Signpost()
    {
        interaction = "Read";
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && !TextController.Instance.getState())
        {
            textQueue = new Queue<string>();
            foreach (string element in signText)
            {
                textQueue.Enqueue(element);
            }
            TextController.Instance.setText(textQueue,"Sign");
            TextController.Instance.displayText();
        }
    }
}
