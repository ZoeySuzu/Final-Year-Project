using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : Interactable {

    public List<string> signText;
    private Queue<string> textQueue;
    public TextController textDisplay;

    void Start()
    {
        gameUI = GetComponentInParent<UIController>();
    }

    public Signpost()
    {
        
        interaction = "Read";
    }

    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && !textDisplay.getState())
        {
            textQueue = new Queue<string>();
            foreach (string element in signText)
            {
                textQueue.Enqueue(element);
            }
            textDisplay.setText(textQueue);
        }
    }
}
