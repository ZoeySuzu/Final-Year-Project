using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Last clean: 29/11/2017
//to do: make a method to set target
//to do: Make a method to set ennemy

public class FollowCamera : MonoBehaviour
{
    public GameObject target;
    public GameObject ennemy;

    public float maxHeight;
    public float maxDistance;
    private bool enemyFocused;
    private Vector3 center;

    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
        enemyFocused = GetComponentInParent<PlayerController>().fighting;
    }

    void LateUpdate()
    {
        if (enemyFocused) {
            center = (ennemy.transform.position - target.transform.position) /2;
            transform.position = 1.5f* (target.transform.position - ennemy.transform.position) + Vector3.up*5;
            transform.LookAt(target.transform.position + center);
        }

        else
        {
            var cameraX = Input.GetAxis("Joystick_B X") * 6;
            var cameraHeight = new Vector3(0, Input.GetAxis("Joystick_B Y") * 0.3f, 0);
            var cameraDistance = -Input.GetAxis("Joystick_B Y") * 0.1f;


            if ( Input.GetAxis("CameraLock") == 1)
            {
                Debug.Log("Camera Focused");
                transform.position = target.transform.position + target.transform.forward * -8 + target.transform.up * 5;
                offset = target.transform.position - transform.position;
                transform.LookAt(target.transform);
            }

            Quaternion rotation = Quaternion.Euler(0, cameraX, 0);
            transform.position = target.transform.position - (rotation * offset) + cameraHeight;



            if (transform.position.y > target.transform.position.y + maxHeight)
            {
                transform.position = new Vector3(transform.position.x, target.transform.position.y + maxHeight, transform.position.z);
            }
            if (transform.position.y < target.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
            }

            offset = target.transform.position - transform.position;
            transform.LookAt(target.transform);

        }
    }
}