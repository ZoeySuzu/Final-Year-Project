using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : NPCController {

    private string charName;
    private string dialogueID;
    private Relationship relationship;

    private FriendPoints friendPoints;

    private bool canNemesis, canPartner;


    public FriendPoints getFP()
    {
        return friendPoints;
    }

    public int getFPValue()
    {
        return friendPoints.value;
    }

    public void updateFP(int n)
    {
        friendPoints.changeStat(n);
    }
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

    private void updateRelationship()
    {
        int value = friendPoints.value;
        if (value < -500 && canNemesis)
            relationship = Relationship.Nemesis;
        else if ((value >= -500 && value < -300) || (value < -500 && !canNemesis))
            relationship = Relationship.Enemy;
        else if (value >= -300 && value < -100)
            relationship = Relationship.Rival;
        else if (value >= -100 && value <= 100)
            relationship = Relationship.Aquaintance;
        else if (value > 100 && value <= 300)
            relationship = Relationship.Friend;
        else if ((value > 300 && value <= 500) || (value > 500 && !canPartner))
            relationship = Relationship.BestFriend;
        else if (value > 500 && canPartner)
            relationship = Relationship.Partner;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
