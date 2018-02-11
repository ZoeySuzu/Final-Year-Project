using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : NPCController {

    private string charName;
    private string dialogueID;
    public FriendPoints friendPoints;

    public void Initialize(string _charName, string _dialogueID, int _friendPoints)
    {
        charName = _charName;
        dialogueID = _dialogueID;
        friendPoints = new FriendPoints(charName+"FriendPoints",_friendPoints);
    }

    public override void interact()
    {
        if (!TextController.Instance.gameObject.activeSelf)
        {
            FollowCamera.Instance.setSpecific(gameObject, transform.forward * 5 + transform.up * 2 + transform.right * -3);
            TextController.Instance.sendDialogueRequest(dialogueID, this, true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
