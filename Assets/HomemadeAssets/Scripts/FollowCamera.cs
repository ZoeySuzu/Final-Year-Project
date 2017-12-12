using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//to do: make a method to set target
//to do: Make a method to set ennemy

public class FollowCamera : MonoBehaviour
{
    private GameObject player;

    private GameObject targetEnnemy;
    private bool enemyFocused;
    private Vector3 center;

    public float maxHeight;
    public float maxDistance;

    private Vector3 init;
    private Vector3 offset;

    private bool lerping;
    private float lerpStartTime;
    private Vector3 lerpStartPos;

    void Start()
    {
        player = transform.parent.gameObject;
        init = player.transform.position - transform.position;
        offset = init;
    }

    void LateUpdate()
    {
        if (enemyFocused) {
            if (Input.GetButtonDown("CameraLock"))
            {
                enemyFocused = false;
                Debug.Log("Camera Focused");
                offset = player.transform.position - transform.parent.rotation * init;
                recenter();
            }
            center = player.transform.position+ (targetEnnemy.transform.position - player.transform.position)/2;
            transform.position = player.transform.position+(player.transform.position - targetEnnemy.transform.position)/2 + Vector3.up*5;
            transform.LookAt(center);
            Debug.Log(center);
        }

        else
        {
            var cameraX = Input.GetAxis("Joystick_B X") * 6;
            var cameraHeight = new Vector3(0, Input.GetAxis("Joystick_B Y") * 0.3f, 0);
            //var cameraDistance = -Input.GetAxis("Joystick_B Y") * 0.1f;


            if ( Input.GetButtonDown("CameraLock"))
            {
                GameObject ennemy = PlayerController.Instance.getNearestEnnemy();
                if (ennemy != null)
                {
                    targetEnnemy = ennemy;
                    enemyFocused = true;
                }
                else
                {
                    Debug.Log("Camera Focused");
                    offset = player.transform.position - transform.parent.rotation * init;
                    recenter();
                }
            }

            if (lerping)
            {
                float lerpTime = Time.time - lerpStartTime;
                float lerpPercent = lerpTime / 0.1f;
                transform.position = Vector3.Lerp(lerpStartPos, offset, lerpPercent);
                if (lerpPercent >= 1.0f)
                {
                    Debug.Log("End lerp");
                    lerping = false;
                    offset = player.transform.position - transform.position;
                }
            }
            else
            {
                Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
                transform.position = player.transform.position - (rotation * offset) + cameraHeight;



                if (transform.position.y > player.transform.position.y + maxHeight)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y + maxHeight, transform.position.z);
                }
                if (transform.position.y < player.transform.position.y)
                {
                    transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
                }

                offset = player.transform.position - transform.position;
            }
            transform.LookAt(player.transform);
        }
    }


    public void recenter()
    {
        Debug.Log("start lerp");
        lerping = true;
        lerpStartTime = Time.time;
        lerpStartPos = transform.position;
    }
}