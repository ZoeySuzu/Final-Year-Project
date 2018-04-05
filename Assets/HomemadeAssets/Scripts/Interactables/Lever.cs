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
    private Cooldown move;
    private Vector3 pushDirection;

    public void Start()
    {
        gameUI = UIController.Instance;
        transform.localPosition = new Vector3(defaultPos,1,0);
        lr = go.GetComponent<LeverReactor>();
        lr.updateReactor(transform.localPosition.x*reactionMagnitude, reactionType);
        move = new Cooldown(0.5f);
    }

    public void Update()
    {

        if (move.ready)
        {
            transform.localPosition = new Vector3(Mathf.Round(transform.localPosition.x * 4) / 4, transform.localPosition.y, transform.localPosition.z);
            pushDirection = Vector3.zero;
            if (inUse)
            {
                var x = Input.GetAxis("LS-X") * Time.deltaTime * speed;
                var z = Input.GetAxis("LS-Y") * Time.deltaTime * speed;

                Vector3 targetDirection = new Vector3(x, 0f, z);
                targetDirection = Camera.main.transform.TransformDirection(targetDirection);
                targetDirection.y = 0.0f;
                
                if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z))
                {
                    Push(Vector3.right * Mathf.Sign(targetDirection.x));
                }
                else if (Mathf.Abs(targetDirection.x) < Mathf.Abs(targetDirection.z))
                {
                    Push(Vector3.right * Mathf.Sign(targetDirection.z));
                }
            } 
        }
        else if (inUse)
        {
            PlayerController.Instance.transform.position = new Vector3(transform.position.x + pOffset.x, PlayerController.Instance.transform.position.y, transform.position.z + pOffset.z);
        }

        transform.localPosition += (pushDirection * Time.deltaTime * 0.5f);
        lr.updateReactor(transform.localPosition.x * reactionMagnitude, reactionType);
    }

    public void Push(Vector3 dir)
    {
        if (move.ready)
        {
            if ((dir.x > 0 && transform.localPosition.x < 1) || (dir.x < 0 && transform.localPosition.x > -1))
            {
                pushDirection = dir;
                StartCoroutine(move.StartCooldown());
            }
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
            PlayerController.Instance.anim.SetTrigger("Push");
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
            PlayerController.Instance.anim.SetTrigger("StopPush");
            gameUI.setInteractButton(interaction);
            toggleIndicator();
            PlayerController.Instance.setPlayerState("Idle");
            inUse = false;
        }
    }
}
