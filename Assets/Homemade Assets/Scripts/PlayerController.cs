using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed;
    public float jumpHeight;
    Rigidbody rb;
    UIController ui;

    private string playerState;
    private bool startPushing;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        ui = GetComponentInParent<UIController>();
        playerState = "idle";
        startPushing = false;
    }
	
	// Update is called once per frame
	void Update () {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * speed;

        Vector3 targetDirection = new Vector3(x, 0f, z);
        targetDirection = Camera.main.transform.TransformDirection(targetDirection);
        targetDirection.y = 0.0f;

        //Pushing something
        if (playerState.Equals("pushing"))
        {
            Vector3 pushDirection;
            //Get most emphasized direction
            if (Mathf.Abs(targetDirection.x) > Mathf.Abs(targetDirection.z))
            {
                pushDirection = Vector3.right*Mathf.Sign(targetDirection.x);
            }
            else
            {
                pushDirection = Vector3.forward * Mathf.Sign(targetDirection.z);
            }
            if (targetDirection != Vector3.zero)
            {
                Movement(pushDirection*0.05f);
            } 
        }


        else 
        {
            
            if (targetDirection != Vector3.zero)
            {
                playerState = "walking";
                Movement(targetDirection);
            }
            else
            {
                playerState = "idle";
            }

            if (IsGrounded())
            {
                ui.setActionButton("Jump");
            }
            else
            {
                playerState = "jumping";
                ui.setActionButton("");
            }

            if (Input.GetButtonDown("Jump") && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpHeight * 15000 * Time.deltaTime);
            }
        }

        ui.setPlayerState(playerState);
    }

    public float getSpeed()
    {
        return speed;
    }

    public string getPlayerState()
    {
        return playerState;
    }

    public void setPlayerState(string state)
    {
        playerState = state;
        Debug.Log("Set player interaction:" + state);
    }

    private bool IsGrounded(){
        RaycastHit hit;
        return Physics.SphereCast(transform.position,0.3f,Vector3.down, out hit,1f);
     }

    private void Movement(Vector3 targetDirection)
    {
        float scale = transform.localScale.x;
        RaycastHit hit;
        if (targetDirection.x != 0)
        {
            Ray rayright = new Ray(transform.position, Vector3.right * Mathf.Sign(targetDirection.x));
            if (Physics.Raycast(rayright, out hit, scale/2+0.05f))
            {
                transform.position += Vector3.right * hit.distance * Mathf.Sign(targetDirection.x) - Vector3.right * scale / 2 * Mathf.Sign(targetDirection.x);
            }
            else
            {
                transform.Translate(Vector3.right * targetDirection.x);
            }
        }
        if (targetDirection.z != 0)
        {
            Ray rayforward = new Ray(transform.position, Vector3.forward * Mathf.Sign(targetDirection.z));
            if (Physics.Raycast(rayforward, out hit, scale / 2 + 0.05f))
            {
                if (Mathf.Abs(hit.distance) > 0.05 )
                {
                    transform.position += Vector3.forward * hit.distance * Mathf.Sign(targetDirection.z) - Vector3.forward * scale/2 * Mathf.Sign(targetDirection.z);
                }
            }
            else
            {
                transform.Translate(Vector3.forward * targetDirection.z);
            }
        }
    }

}
