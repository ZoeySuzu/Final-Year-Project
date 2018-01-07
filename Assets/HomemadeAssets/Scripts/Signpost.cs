using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signpost : Interactable {

    [SerializeField]
    private List<string> signText;
    private Queue<string> textQueue;

    [SerializeField]
    private bool isDark;

    public Signpost()
    {
        interaction = "Read";
    }

    public override void interact()
    {
        if (!TextController.Instance.getState())
        {
            textQueue = new Queue<string>();
            if (isDark && !GetComponentInChildren<LightTrigger>().getIsLit())
            {
                textQueue.Enqueue("It's too dark to read this sign...");
                TextController.Instance.displayText(textQueue, "Player");
            }
            else
            {
                foreach (string element in signText)
                {
                    textQueue.Enqueue(element);
                }
                TextController.Instance.displayText(textQueue, "Sign");
            }
        }
    }
}
