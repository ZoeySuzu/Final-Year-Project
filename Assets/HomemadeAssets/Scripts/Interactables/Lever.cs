using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable {

    [SerializeField][Range(-1,1)]
    private float defaultPos;

    [SerializeField]
    private GameObject go;
    private LeverReactor lr;

    [SerializeField]
    private ReactionType reactionType;

    [SerializeField]
    private float reactionMagnitude;

    private bool inUse, startPushing = false;
    private float speed = 3;
    private Vector3 pOffset;

    public void Start()
    {
        gameUI = UIController.Instance;
        transform.localPosition = new Vector3(defaultPos,1,0);
        lr = go.GetComponent<LeverReactor>();
        lr.updateReactor(transform.localPosition.x*reactionMagnitude, reactionType);
    }

    public void Update()
    {
        if (inUse)
        {
            Vector3 pushDirection;

            var x = Input.GetAxis("LS-X") * Time.deltaTime *  speed;
            var z = Input.GetAxis("LS-Y") * Time.deltaTime * speed;

            Vector3 targetDirection = new Vector3(x, 0f, z);
            targetDirection = Camera.main.transform.TransformDirection(targetDirection);
            targetDirection.y = 0.0f;

            if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z))
            {
                pushDirection = Vector3.right * Mathf.Sign(targetDirection.x);
            }
            else
            {
                pushDirection = Vector3.forward * Mathf.Sign(targetDirection.z);
            }
            if (pushDirection == Vector3.right && transform.localPosition.x < 1.0f)
            {
                transform.Translate(Vector3.right * speed* Time.deltaTime);
            }
            else if(pushDirection == Vector3.left && transform.localPosition.x > -1.0f)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            lr.updateReactor(transform.localPosition.x*reactionMagnitude, reactionType);
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
        }
    }


    public Lever()
    {
        interaction = "Grab";
    }

    public override void collisionExit()
    {
        if (inUse)
        {
            PlayerController.Instance.setPlayerState("idle");
            inUse = false;
        }
    }

    private void playerRepos(Vector3 offset)
    {
        var scale = transform.localScale.x;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.z))
        {
            pOffset = new Vector3((scale + 0.2f) * Mathf.Sign(offset.x), 0, 0);
        }
        else
        {
            pOffset = new Vector3(0, 0, (scale + 0.2f) * Mathf.Sign(offset.z));
        }
        PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
    }

    public override void interact()
    {
        if (!inUse)
        {
            Vector3 orientation = new Vector3(transform.position.x, PlayerController.Instance.transform.GetChild(0).position.y, transform.position.z);
            PlayerController.Instance.transform.GetChild(0).LookAt(orientation);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Pushing");
            pOffset = PlayerController.Instance.transform.position - transform.position;
            playerRepos(pOffset);
            inUse = true;
            startPushing = true;
        }
        else
        {
            gameUI.setInteractButton(interaction);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Idle");
            inUse = false;
        }
    }
}
