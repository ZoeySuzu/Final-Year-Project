using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDialogue : Interactable {

    [SerializeField]
    private int fp;

    [SerializeField]
    private string speakerName = "Default";
    [SerializeField]
    private string dialogueID; 

    private Queue<string> textQueue;


    private void Start()
    {
        gameUI = UIController.Instance;
        GameController.Instance.addEntity(this.gameObject);
    }

    public TestDialogue()
    {
        interaction = "Talk";
    }

    public string getName()
    {
        return speakerName;
    }

    public void setDialogue(string _dialogueID)
    {
        dialogueID = _dialogueID;
    }

    public void setFriendPoints(int _value)
    {
        fp += _value;
    }

    public bool checkFriendPoints(bool _higher, int _value)
    {
        if (_higher)
        {
            if (fp >= _value) return true;
        }
        else
        {
            if (fp < _value) return true;
        }
        return false;
    }

    public override void interact()
    {
        if (!TextController.Instance.gameObject.activeSelf)
        {
            TextController.Instance.sendDialogueRequest(dialogueID, this);
        }
    }
}
