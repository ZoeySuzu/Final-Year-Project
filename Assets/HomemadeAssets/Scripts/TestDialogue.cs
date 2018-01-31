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


    public void OnTriggerStay(Collider other)
    {
        if (other.name == "Object_Player")
        {
            Vector3 orientation = new Vector3(PlayerController.Instance.transform.position.x, transform.position.y, PlayerController.Instance.transform.position.z);
            transform.LookAt(orientation);
            if (Input.GetButtonDown("A"))
            {
                orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
                PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
                interact();
                PlayerController.Instance.switchInteracting();
            }
        }
    }

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
           
            FollowCamera.Instance.setSpecific(gameObject, transform.forward * 5 + transform.up * 2 + transform.right * -3);
            
            TextController.Instance.sendDialogueRequest(dialogueID, this);
        }
    }
}
