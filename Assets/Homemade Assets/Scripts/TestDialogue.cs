using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : Interactable {

    private int fp;

    public int line;

    private Queue<string> textQueue;

    void Start()
    {
        gameUI = GetComponentInParent<UIController>();
    }

    public TestDialogue()
    {
        interaction = "Talk";
    }

    public void talk()
    {
            TextController.Instance.setText(sayDialogue(line), "Empty");
            TextController.Instance.displayText();
    }

    private Queue<string> sayDialogue(int line)
    {
        Queue<string> toSay = new Queue<string>(); 
        switch(line){
            case 0:
                {
                    toSay.Enqueue("Hi I'm a test character");
                    toSay.Enqueue("I will ask you a few questions");
                    toSay.Enqueue("Question");
                    toSay.Enqueue("Do you like cake?");
                    toSay.Enqueue("Yes, duhhhh");
                    toSay.Enqueue("eww, No");
                    break;
                }
            default:
                {
                    toSay.Enqueue("I forgot what I was going to say");
                    break;
                }
        }
        return toSay;
    }


    public override void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Interact") && !TextController.Instance.gameObject.activeSelf)
        {
            talk();
        }
    }
}
